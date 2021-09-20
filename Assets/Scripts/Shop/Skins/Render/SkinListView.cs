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

            if (skinData.Equals(inventory.SelectedSkin))
                inst.RenderSelected(skinData);
            else if (inventory.Contains(skinData))
                inst.RenderBuyed(skinData);
            else
                inst.RenderLocked(skinData);

            presenters.Add(inst);
        }

        return presenters;
    }
}
