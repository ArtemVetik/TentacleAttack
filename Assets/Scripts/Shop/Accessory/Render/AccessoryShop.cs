using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryShop : MonoBehaviour
{
    [SerializeField] private AccessoryDataBase _dataBase;
    [SerializeField] private AccessoryListView _listView;
    [SerializeField] private ScrollViewAssistant _scrollAssistant;
    [SerializeField] private Button _unlockButton;

    private IEnumerable<AccessoryPresenter> _presenters;
    private AccessoryPresenter _selectedPresenter;

    private void OnEnable()
    {
        var inventory = new AccessoryInventory(_dataBase);
        inventory.Load();

        _presenters = _listView.Render(_dataBase.Data, inventory);

        if (inventory.SelectedSkin == null)
            _selectedPresenter = null;
        else
            _selectedPresenter = _presenters.First(presenter => presenter.Data.Equals(inventory.SelectedSkin));

        foreach (var presenter in _presenters)
            presenter.Clicked += OnPresenterClicked;

        _unlockButton.onClick.AddListener(OnUnlockButtonClicked);

        var isAllUnlocked = inventory.Data.Count() == _dataBase.Data.Count();
        _unlockButton.gameObject.SetActive(!isAllUnlocked);
    }

    private void OnPresenterClicked(AccessoryPresenter presenter)
    {
        if (presenter.IsRenderSelected)
            DeselectPresenter(presenter);
        else
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

    private void SelectPresenter(AccessoryPresenter presenter)
    {
        _selectedPresenter?.RenderBuyed(_selectedPresenter.Data);
        presenter.RenderSelected(presenter.Data);
        _selectedPresenter = presenter;

        var inventory = new AccessoryInventory(_dataBase);
        inventory.Load();
        inventory.SelectAccessory(presenter.Data);
        inventory.Save();
    }

    public void DeselectPresenter(AccessoryPresenter presenter)
    {
        presenter.RenderBuyed(presenter.Data);
        _selectedPresenter = null;

        var inventory = new AccessoryInventory(_dataBase);
        inventory.Load();
        inventory.DeselectAccessory();
        inventory.Save();
    }

    private void OnUnlockButtonClicked()
    {
        var inventory = new AccessoryInventory(_dataBase);
        inventory.Load();

        var lockedDatas = new List<AccessoryData>();

        foreach (var accessoryData in _dataBase.Data)
            if (inventory.Contains(accessoryData) == false)
                lockedDatas.Add(accessoryData);

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
