using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleLostTranasition : Transition
{
    [SerializeField] private PlayerContainer _container;
    [SerializeField] private float _lostDistance;

    private FooPlayer _player;

    protected override void Enable()
    {
        _player = _container.Player;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > _lostDistance)
            NeedTransit = true;
    }
}
