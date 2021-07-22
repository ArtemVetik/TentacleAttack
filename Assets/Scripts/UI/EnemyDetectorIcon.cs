using System.Collections;
using UnityEngine;

public class EnemyDetectorIcon : MonoBehaviour
{
    [SerializeField] private GameObject _patrolIcon;
    [SerializeField] private GameObject _attackIcon;
    [SerializeField] private GameObject _sleepIcon;

    public void ChangeIcon(EnemyTransitState state)
    {
        switch(state)
        {
            case EnemyTransitState.Attack:
                ActivationGameObjects(_attackIcon, _patrolIcon, _sleepIcon);
                break;
            case EnemyTransitState.Patrol:
                ActivationGameObjects(_patrolIcon, _attackIcon, _sleepIcon);
                break;
            case EnemyTransitState.Sleep:
                ActivationGameObjects(_sleepIcon, _patrolIcon, _attackIcon);
                break;
        }
    }

    private void ActivationGameObjects(GameObject activeObj, params GameObject[] inActiveObj )
    {
        foreach(var inactive in inActiveObj)
        {
            inactive.SetActive(false);
        }
        activeObj.SetActive(true);
    }
}

public enum EnemyTransitState
{
    Patrol,
    Attack,
    Sleep,
}

