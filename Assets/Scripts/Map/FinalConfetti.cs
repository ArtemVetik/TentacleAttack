using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FinalConfetti : MonoBehaviour
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    [SerializeField] private ParticleSystem[] _templates;

    private Coroutine _spawnCoroutine;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Selection.activeGameObject != gameObject)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
#endif

    private void OnValidate()
    {
        _maxDelay = Mathf.Clamp(_maxDelay, 0, float.MaxValue);
        _minDelay = Mathf.Clamp(_minDelay, 0, _maxDelay);
    }

    private void OnEnable()
    {
        GlobalEventStorage.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GlobalEventStorage.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded(bool isWin, int progress)
    {
        if (isWin)
            StartSpawner();
    }

    public void StartSpawner()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            var particleIndex = Random.Range(0, _templates.Length);
            var particleTemplate = _templates[particleIndex];
            var position = transform.position + Random.insideUnitSphere * _spawnRadius;
            var delay = Random.Range(_minDelay, _maxDelay);

            Instantiate(particleTemplate, position, Quaternion.identity);

            yield return new WaitForSeconds(delay);
        }
    }
}