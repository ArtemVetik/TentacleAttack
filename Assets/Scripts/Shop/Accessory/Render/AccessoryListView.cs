using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryListView : MonoBehaviour
{
    [SerializeField] private AccessoryPresenter _template;
    [SerializeField] private Transform _container;

    public IEnumerable<AccessoryPresenter> Render(IEnumerable<AccessoryData> skinList, AccessoryInventory inventory)
    {
        var presenters = new List<AccessoryPresenter>();

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
