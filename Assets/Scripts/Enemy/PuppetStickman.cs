using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetStickman : MonoBehaviour
{
    [SerializeField] private Transform _skinContainer;
    [SerializeField] private SkinnedMeshRenderer _skinTemplate;

    public void Init(IEnumerable<SkinnedMeshRenderer> skins)
    {
        foreach (var skin in skins)
        {
            var inst = Instantiate(_skinTemplate, _skinContainer);
            inst.gameObject.SetActive(true);
            inst.sharedMesh = skin.sharedMesh;
            inst.materials = skin.materials;
        }
    }
}
