using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothShop : MonoBehaviour
{
    [SerializeField] private ClothDataBase _dataBase;
    [SerializeField] private ClothListView _listView;
    [SerializeField] private ScrollViewAssistant _scrollAssistant;

    private IEnumerable<ClothPresenter> _presenters;
    private ClothPresenter _selectedPresenter;
    private ClothPresenter _buyPresenter;
    private AdSettings _adSettings;

    private void OnEnable()
    {
        _adSettings = Singleton<AdSettings>.Instance;
        _adSettings.UserEarnedReward += OnUserEarnedReward;

        var inventory = new ClothInventory(_dataBase);
        inventory.Load();

        _presenters = _listView.Render(_dataBase.Data, inventory);

        if (inventory.SelectedSkin == null)
            _selectedPresenter = null;
        else
            _selectedPresenter = _presenters.First(presenter => presenter.Data.Equals(inventory.SelectedSkin));

        foreach (var presenter in _presenters)
        {
            presenter.Clicked += OnPresenterClicked;
            presenter.ClickUnlocked += OnPresenterClickedUnlock;
        }
    }

    private void OnDisable()
    {
        _adSettings.UserEarnedReward -= OnUserEarnedReward;
        _buyPresenter = null;

        foreach (var presenter in _presenters)
        {
            presenter.Clicked -= OnPresenterClicked;
            presenter.ClickUnlocked -= OnPresenterClickedUnlock;
            Destroy(presenter.gameObject);
        }
    }

    private void OnPresenterClickedUnlock(ClothPresenter presenter)
    {
        var inventory = new ClothInventory(_dataBase);
        inventory.Load();

        if (inventory.ContainsBuyed(presenter.Data) == false)
        {
            TryBuy(presenter);
        }
    }

    private void OnPresenterClicked(ClothPresenter presenter)
    {
        var inventory = new ClothInventory(_dataBase);
        inventory.Load();

        if (inventory.ContainsBuyed(presenter.Data))
        {
            if (presenter.IsRenderSelected)
                DeselectPresenter(presenter);
            else
                SelectPresenter(presenter);
        }
    }

    private void SelectPresenter(ClothPresenter presenter)
    {
        _selectedPresenter?.RenderBuyed(_selectedPresenter.Data);
        presenter.RenderSelected(presenter.Data);
        _selectedPresenter = presenter;

        var inventory = new ClothInventory(_dataBase);
        inventory.Load();
        inventory.SelectCloth(presenter.Data);
        inventory.Save();
    }

    public void DeselectPresenter(ClothPresenter presenter)
    {
        presenter.RenderBuyed(presenter.Data);
        _selectedPresenter = null;

        var inventory = new ClothInventory(_dataBase);
        inventory.Load();
        inventory.DeselectAccessory();
        inventory.Save();
    }

    private void TryBuy(ClothPresenter data)
    {
        _buyPresenter = data;
        _adSettings.ShowRewarded();
    }

    private void OnUserEarnedReward()
    {
        if (_buyPresenter == null)
            return;

        var inventory = new ClothInventory(_dataBase);
        inventory.Load();
        inventory.Add(_buyPresenter.Data);
        inventory.SelectCloth(_buyPresenter.Data);
        inventory.Save();

        _selectedPresenter.RenderBuyed(_selectedPresenter.Data);
        _buyPresenter.RenderSelected(_buyPresenter.Data);

        _selectedPresenter = _buyPresenter;
        _buyPresenter = null;
    }
}
