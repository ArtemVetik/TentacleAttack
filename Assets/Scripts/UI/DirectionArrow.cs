using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _maxWidth = 150f;

    private TargetMovement _targetMovement;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _targetMovement = FindObjectOfType<TargetMovement>();
        _rectTransform = GetComponent<RectTransform>();
        OnTentacleDied(false);

        GlobalEventStorage.GameOvering += OnTentacleDied;
        _targetMovement.TragetMoved += OnTargetMoved;
        _targetMovement.Rewinding += OnTargetRewinding;
    }

    private void OnDestroy()
    {
        GlobalEventStorage.GameOvering -= OnTentacleDied;
        _targetMovement.TragetMoved -= OnTargetMoved;
        _targetMovement.Rewinding -= OnTargetRewinding;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        transform.position = Camera.main.WorldToScreenPoint(_targetMovement.transform.position);

        float angle = Vector2.SignedAngle(Vector2.left, _joystick.Direction);
        transform.eulerAngles = Vector3.forward * angle;
        _rectTransform.sizeDelta = new Vector2(_maxWidth * _joystick.Direction.magnitude, 65.0f);
    }

    private void OnTargetMoved(Vector3 position)
    {
        enabled = true;
    }

    private void OnTargetRewinding(Transform target, float speedRate, float accelerationRate)
    {
        enabled = false;
        _rectTransform.sizeDelta *= Vector2.zero; 
    }

    private void OnTentacleDied(bool isWin)
    {
        enabled = false;
        _rectTransform.sizeDelta *= Vector2.zero;
    }
}
