using UnityEngine;

public class StuckState : State
{
    private EnemyAnimations _animations;

    private void Awake()
    {
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

    private void OnEnable()
    {
        _animations.EnemyStuck();
    }
}
