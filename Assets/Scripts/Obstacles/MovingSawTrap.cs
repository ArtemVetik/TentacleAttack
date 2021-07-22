using System.Collections;
using UnityEngine;
using SplineMesh;

public class MovingSawTrap : ActiveObject
{
    [SerializeField] private float _speed;
    [SerializeField] private ParticleSystem _destroyEffect;
    [SerializeField] private bool _moveAwake = false;
    [SerializeField] private bool _destroyWnenActivate = false;

    private Spline _spline;
    private Coroutine _moving;
    private float _distanceCovered;
    private Direction _direction;

    private void Awake()
    {
        _spline = GetComponentInParent<Spline>();
    }

    private void Start()
    {
        transform.position = _spline.GetSampleAtDistance(0).location;
        _distanceCovered = 0f;
        _direction = Direction.Right;

        enabled = _moveAwake;
    }

    private void Update()
    {
        _distanceCovered += _speed * Time.deltaTime * (int)_direction;

        if (_distanceCovered >= _spline.Length)
            _direction = Direction.Left;
        else if (_distanceCovered <= 0)
            _direction = Direction.Right;

        _distanceCovered = Mathf.Clamp(_distanceCovered, 0, _spline.Length);

        transform.position = _spline.GetSampleAtDistance(_distanceCovered).location;
    }

    public override void Action()
    {
        enabled = false;

        if (_destroyWnenActivate)
            DestroySaw();
        else
            _moving = StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            _distanceCovered += _speed * Time.deltaTime * (int)_direction;

            if (_distanceCovered >= _spline.Length)
            {
                StopCoroutine(_moving);
                DestroySaw();
                break;
            }

            transform.position = _spline.GetSampleAtDistance(_distanceCovered).location;

            yield return null;
        }
    }

    private void DestroySaw()
    {
        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
