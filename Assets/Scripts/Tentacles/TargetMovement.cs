using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Joystick _joystick;

    private bool _isRewind;
    private bool _isUsed = true;

    public event Action<Vector3> TragetMoved;
    public event Action<Transform, float, float> Rewinding;
    public event Action<Transform> RewindFinished;

    private Rigidbody _body;
    private EnemyContainer _enemyContainer;
    private Health _health;
    private Coroutine _damageCoroutine;

    public bool IsUsed => _isUsed;

    private void Awake()
    {
        _enemyContainer = FindObjectOfType<EnemyContainer>();
        _health = FindObjectOfType<Health>();
        _body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.TentacleAddDamageAddListener(OnAddDamage);
        GlobalEventStorage.TentacleDiedAddListener(OnTentacleDied);
        _enemyContainer.EnemyEnded += OnLevelCompleted;
    }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamageRemoveListener(OnAddDamage);
        GlobalEventStorage.TentacleDiedRemoveListener(OnTentacleDied);
        _enemyContainer.EnemyEnded -= OnLevelCompleted;
    }

    private void Update()
    {
        if (_isUsed)
        {
            if (Input.GetMouseButton(0))
                Movement();
            if (Input.GetMouseButtonUp(0))
                Rewind();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void Movement()
    {
        if (_isRewind)
            StopRewind();

        Vector3 translation = _joystick.Direction * _moveSpeed * Time.fixedDeltaTime;

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
        if (_damageCoroutine != null)
            return;

        if (_health.TakeDamage() == false)
            return;

        _damageCoroutine = StartCoroutine(DamageRewind(segment));
    }

    private void OnLevelCompleted()
    {
        _isUsed = false;
        Rewind(2f, 2f);
    }

    private void OnTentacleDied()
    {
        _isUsed = false;
        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        rb.drag = 0.5f;
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