using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartMovementTrigger : MonoBehaviour
{
    [SerializeField] private TargetMovement _targetMovement;
    [SerializeField] private GameObject[] _hiddenObjects;

    public event UnityAction MoveStarted;

    private bool _isStarted = false;

    public bool IsStarted => _isStarted;

    private void OnEnable()
    {
        _targetMovement.TragetMoved += OnTargetMoved;
    }

    private void OnDisable()
    {
        _targetMovement.TragetMoved -= OnTargetMoved;
    }

    private void OnTargetMoved(Vector3 position)
    {
        if (_isStarted)
            return;

        _isStarted = true;
        MoveStarted?.Invoke();
        foreach (var hiddenObject in _hiddenObjects)
        {
            hiddenObject.SetActive(false);
        }

        _targetMovement.TragetMoved -= OnTargetMoved;
    }
}
