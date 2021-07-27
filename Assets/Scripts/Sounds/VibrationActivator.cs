using UnityEngine;

[RequireComponent(typeof(Android))]
public class VibrationActivator : MonoBehaviour
{
    private Android _android;
    private EatingArea _eatingArea;
    private TargetDamager _targetDamager;
    private KrakenChild _child;
    private EnemyHolder _enemyHolder;
    private TriggerSpeaker[] _triggerSpeakers;

    private void Awake()
    {
        _android = GetComponent<Android>();
        _eatingArea = FindObjectOfType<EatingArea>();
        _targetDamager = FindObjectOfType<TargetDamager>();
        _child = FindObjectOfType<KrakenChild>();
        _enemyHolder = FindObjectOfType<EnemyHolder>();
        _triggerSpeakers = FindObjectsOfType<TriggerSpeaker>();
    }

    private void OnEnable()
    {
        GlobalEventStorage.TentacleAddDamage += OnTentacleDamaged;
        _eatingArea.Eating += OnEating;
        //_targetDamager.EnemyFounded += OnEnemyFounded;
        _enemyHolder.EnemyHold += OnEnemyFounded;

        if(_triggerSpeakers != null)
        {
            foreach(var speaker in _triggerSpeakers)
            {
                speaker.TriggerEnter += OnTriggerSpeakerEnter;
                Debug.Log("Trigger name - " + speaker.name);
            }
        }
        
        if (_child)
            _child.Released += OnChildReleased;
    }

    private void OnDisable()
    {
        GlobalEventStorage.TentacleAddDamage -= OnTentacleDamaged;
        _eatingArea.Eating -= OnEating;
        //_targetDamager.EnemyFounded -= OnEnemyFounded;
        _enemyHolder.EnemyHold -= OnEnemyFounded;

        if (_triggerSpeakers != null)
        {
            foreach (var speaker in _triggerSpeakers)
            {
                speaker.TriggerEnter -= OnTriggerSpeakerEnter;
            }
        }

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

    private void OnEnemyFounded()
    {
        Vibrate(100);
    }

    private void OnChildReleased()
    {
        Vibrate(500);
    }

    private void OnTriggerSpeakerEnter()
    {
        Vibrate(50);
    }

    private void Vibrate(int ms)
    {
        if (SaveDataBase.GetVibrationSetting() == true)
            _android.vibrate(ms);
    }
}
