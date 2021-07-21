using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Android))]
public class VibrationActivator : MonoBehaviour
{
    private Android _android;
    private EatingArea _eatingArea;
    private TargetDamager _targetDamager;
    private KrakenChild _child;

    private void Awake()
    {
        _android = GetComponent<Android>();
        _eatingArea = FindObjectOfType<EatingArea>();
        _targetDamager = FindObjectOfType<TargetDamager>();
        _child = FindObjectOfType<KrakenChild>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.TentacleAddDamage += OnTentacleDamaged;
        _eatingArea.Eating += OnEating;
        _targetDamager.EnemyFounded += OnEnemyFounded;
        
        if (_child)
            _child.Released += OnChildReleased;
    }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamage -= OnTentacleDamaged;
        _eatingArea.Eating -= OnEating;
        _targetDamager.EnemyFounded -= OnEnemyFounded;

        if (_child)
            _child.Released -= OnChildReleased;
    }

    private void OnTentacleDamaged(TentacleSegment segment)
    {
        Vibrate(300);
    }

    private void OnEating()
    {
        Vibrate(200);
    }

    private void OnEnemyFounded(Enemy enemy)
    {
        Vibrate(500);
    }

    private void OnChildReleased()
    {
        Vibrate(500);
    }

    private void Vibrate(int ms)
    {
        if (SaveDataBase.GetVibrationSetting() == true)
            _android.vibrate(ms);
    }
}
