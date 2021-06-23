using RootMotion.FinalIK;
using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] private PlayerContainer _container;
    [SerializeField] private Sword _weapon;
    [SerializeField] private float _runSpeed;

    private AimIK _aimIK;
    private Spline _moveSpline;
    private EnemyAnimations _animations;
    private TentacleSegment _tentacle;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _aimIK = GetComponentInChildren<AimIK>(true);
        _moveSpline = GetComponentInParent<Spline>();
        _animations = GetComponentInChildren<EnemyAnimations>();
    }

    private void OnEnable()
    {
        _tentacle = _container.Player;

        _aimIK.solver.target = _tentacle.CenterTransform;
        _aimIK.enabled = true;

        _weapon.gameObject.SetActive(true);

        _animations.EnableAttack();
    }

    private void Update()
    {
        var directionVector = (_tentacle.MeshCenterPosition - transform.position).normalized;
        var sample = _moveSpline.GetProjectionSample(_tentacle.MeshCenterPosition - directionVector * 2f);

        var attackForce = 1f - Vector3.Distance(transform.position, sample.location) / _moveSpline.Length * 2f;
        _animations.SetAttackForce(attackForce);

        _previousPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, sample.location, _runSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _previousPosition) > 0.01f)
            transform.rotation = Quaternion.LookRotation(transform.position - _previousPosition, Vector3.up);
    }

    private void OnDisable()
    {
        _animations.DisableAttack();
        _weapon.gameObject.SetActive(false);
        _aimIK.enabled = false;
    }
}
