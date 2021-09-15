using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinListView : MonoBehaviour
{
    [SerializeField] private SkinPresenter _template;
    [SerializeField] private Transform _container;

    public IEnumerable<SkinPresenter> Render(IEnumerable<SkinData> skinList, SkinInventory inventory)
    {
        var presenters = new List<SkinPresenter>();

        foreach (var skinData in skinList)
        {
            var inst = Instantiate(_template, _container);

            if (inventory.SelectedSkin.Equals(skinData))
                inst.RenderSelected(skinData);
            else if (inventory.Contains(skinData))
                inst.RenderBuyed(skinData);
            else
                inst.RenderLocked(skinData);

            presenters.Add(inst);
        }

        return presenters;
    }

    public void InitializePresenters(IEnumerable<SkinPresenter> presenters, SkinInventory inventory)
    {
        foreach (var presenter in presenters)
        {
            if (inventory.SelectedSkin.Equals(presenter.Data))
                presenter.RenderSelected(presenter.Data);
            else if (inventory.Contains(presenter.Data))
                presenter.RenderBuyed(presenter.Data);
        }
    }
}
