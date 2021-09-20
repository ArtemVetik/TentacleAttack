using UnityEngine;

public class LevelScore : MonoBehaviour
{
    private TargetDamager _damager;
    private KrakenChild _krakenChild;

    private void Awake()
    {
        _damager = FindObjectOfType<TargetDamager>();
        _krakenChild = FindObjectOfType<KrakenChild>();
    }

    public int Value { get; private set; }

    private void OnEnable()
    {
        if (_damager)
            _damager.EnemyFounded += OnEnemyFounded;
        if (_krakenChild)
            _krakenChild.Released += OnKrakenChildReleased;

        if (_damager == null && _krakenChild == null)
            Debug.LogError("Set TargetDamager or KrakenChild");
    }

    private void OnDisable()
    {
        if (_damager)
            _damager.EnemyFounded -= OnEnemyFounded;
        if (_krakenChild)
            _krakenChild.Released -= OnKrakenChildReleased;
    }

    private void OnEnemyFounded(Enemy enemy)
    {
        Value += 10;
    }

    private void OnKrakenChildReleased()
    {
        Value += 100;
    }
}
