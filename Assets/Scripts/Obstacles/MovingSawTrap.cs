using System.Collections;
using UnityEngine;
using SplineMesh;

public class MovingSawTrap : ActiveObject
{
    [SerializeField] private float _speed;
    [SerializeField] private ParticleSystem _destroyEffect;

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
    }

    public override void Action()
    {
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
