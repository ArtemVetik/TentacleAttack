using System.Collections.Generic;
using UnityEngine;

public class StuckState : State
{
    [SerializeField] private Transform _skinParent;
    [SerializeField] private PuppetStickman _template;

    private void OnEnable()
    {
        var activeSkins = _skinParent.GetComponentsInChildren<SkinnedMeshRenderer>();

        _skinParent.gameObject.SetActive(false);

        var inst = Instantiate(_template, transform);
        inst.transform.localPosition = Vector3.zero;
        inst.Init(activeSkins);
    }
}
