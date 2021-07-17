using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelScore : MonoBehaviour
{
    [SerializeField] private TargetDamager _damager;
    [SerializeField] private KrakenChild _krakenChild;

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

    public void SaveScore()
    {
        var allScore = SaveDataBase.GetScore();
        allScore += Value;
        SaveDataBase.SetScore(allScore);
    }
}
