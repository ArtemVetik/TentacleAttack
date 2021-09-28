using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothListView : MonoBehaviour
{
    [SerializeField] private ClothPresenter _template;
    [SerializeField] private Transform _container;

    public IEnumerable<ClothPresenter> Render(IEnumerable<ClothData> skinList, ClothInventory inventory)
    {
        var presenters = new List<ClothPresenter>();

        foreach (var skinData in skinList)
        {
            var inst = Instantiate(_template, _container);

            if (skinData.Equals(inventory.SelectedSkin))
                inst.RenderSelected(skinData);
            else if (inventory.ContainsBuyed(skinData))
                inst.RenderBuyed(skinData);
            else if (inventory.ContainsAvaiable(skinData))
                inst.RenderLocked(skinData);
            else
                inst.RenderNotAviable(skinData);

            presenters.Add(inst);
        }

        return presenters;
    }
}
