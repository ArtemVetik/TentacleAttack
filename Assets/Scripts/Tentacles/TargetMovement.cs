using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class TargetMovement : MonoBehaviour
{
    private Joystick _joystick;
    private float _moveSpeed = 5;
    private bool _isRewind;
    private bool _isUsed = true;

    public event Action<Vector3> TragetMoved;
    public event Action<Transform, float, float> Rewinding;
    public event Action<Transform> RewindFinished;

    private Rigidbody _body;
    private EnemyContainer _enemyContainer;
    private Coroutine _damageCoroutine;
    private bool _isLastLevel;

    public bool IsUsed => _isUsed;

    private void Awake()
    {
        _joystick = FindObjectOfType<FloatingJoystick>();
        _enemyContainer = FindObjectOfType<EnemyContainer>();
        _isLastLevel = FindObjectOfType<KrakenChild>();
        _body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.TentacleAddDamage += OnAddDamage;
        GlobalEventStorage.GameOvering += OnTentacleDied;

        if (!_isLastLevel)
            _enemyContainer.EnemyEnded += OnLevelCompleted;
    }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamage -= OnAddDamage;
        GlobalEventStorage.GameOvering -= OnTentacleDied;

        if (!_isLastLevel)
            _enemyContainer.EnemyEnded -= OnLevelCompleted;
    }

    private void Update()
    {

        if (_isUsed)
        {
            if (Input.GetMouseButton(0) && IsPointerOverIgoreObject(Input.mousePosition) == false)
                Movement();
            if (Input.GetMouseButtonUp(0))
                _body.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public bool IsPointerOverIgoreObject(Vector2 inputPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = inputPosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("IgnoreStart"))
                return true;
        }

        return false;
    }

    private void Movement()
    {
        if (_isRewind)
            StopRewind();

        Vector3 translation = _joystick.Direction * _moveSpeed;
        _body.velocity = translation;

        TragetMoved?.Invoke(transform.position);
    }

    private void Rewind(float speedRate = 1f, float accelerationRate = 1f)
    {
        _body.velocity = Vector3.zero;
        _isRewind = true;
        Rewinding?.Invoke(transform, speedRate, accelerationRate);
    }

    private void StopRewind()
    {
        _isRewind = false;
        RewindFinished?.Invoke(transform);
    }

    private void OnAddDamage(TentacleSegment segment)
    {
        GlobalEventStorage.GameOveringInvoke(false);

        //if (_damageCoroutine != null)
        //    return;

        //_damageCoroutine = StartCoroutine(DamageRewind(segment));
    }

    private void OnLevelCompleted()
    {
        _isUsed = false;
        Rewind(2f, 2f);
    }

    private void OnTentacleDied(bool isWin)
    {
        if (!isWin)
        {
            _isUsed = false;
            var rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rb.drag = 0.5f;
        }
    }

    private IEnumerator DamageRewind(TentacleSegment segment)
    {
        _isUsed = false;
        Rewind();
        while (segment != null)
            yield return null;

        yield return new WaitForSeconds(0.3f);

        StopRewind();
        _isUsed = true;

        _damageCoroutine = null;
    }
}