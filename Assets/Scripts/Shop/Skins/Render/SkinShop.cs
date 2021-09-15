using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinShop : MonoBehaviour
{
    [SerializeField] private SkinDataBase _dataBase;
    [SerializeField] private SkinListView _listView;
    [SerializeField] private ScrollViewAssistant _scrollAssistant;
    [SerializeField] private Button _unlockButton;

    private IEnumerable<SkinPresenter> _presenters;
    private SkinPresenter _selectedPresenter;

    private void OnEnable()
    {
        var inventory = new SkinInventory(_dataBase);
        inventory.Load();

        _presenters = _listView.Render(_dataBase.Data, inventory);
        _selectedPresenter = _presenters.First(presenter => presenter.Data.Equals(inventory.SelectedSkin));

        foreach (var presenter in _presenters)
            presenter.Clicked += OnPresenterClicked;

        _unlockButton.onClick.AddListener(OnUnlockButtonClicked);

        var isAllUnlocked = inventory.Data.Count() == _dataBase.Data.Count();
        _unlockButton.gameObject.SetActive(!isAllUnlocked);
    }

    private void OnPresenterClicked(SkinPresenter presenter)
    {
        SelectPresenter(presenter);
    }

    private void OnDisable()
    {
        _unlockButton.onClick.RemoveListener(OnUnlockButtonClicked);

        foreach (var presenter in _presenters)
        {
            presenter.Clicked -= OnPresenterClicked;
            Destroy(presenter.gameObject);
        }
    }

    private void SelectPresenter(SkinPresenter presenter)
    {
        _selectedPresenter.RenderBuyed(_selectedPresenter.Data);
        presenter.RenderSelected(presenter.Data);
        _selectedPresenter = presenter;

        var inventory = new SkinInventory(_dataBase);
        inventory.Load();
        inventory.SelectSkin(presenter.Data);
        inventory.Save();
    }

    private void OnUnlockButtonClicked()
    {
        var inventory = new SkinInventory(_dataBase);
        inventory.Load();

        var lockedDatas = new List<SkinData>();

        foreach (var skinData in _dataBase.Data)
            if (inventory.Contains(skinData) == false)
                lockedDatas.Add(skinData);

        if (lockedDatas.Count == 0)
            return;

        var randomIndex = Random.Range(0, lockedDatas.Count);
        var randomUnlockData = lockedDatas[randomIndex];
        var unlockPresenter = _presenters.First(presenter => presenter.Data.Equals(randomUnlockData));

        inventory.Add(randomUnlockData);
        inventory.Save();

        SelectPresenter(unlockPresenter);

        _scrollAssistant.VerticalSnapTo(unlockPresenter.transform as RectTransform);
    }
}
