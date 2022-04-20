using System;
using UnityEditor;
using UnityEngine;

public class Knight : Unit {
    [Header("Distance")]
    [SerializeField] private float _distanceToFollow;
    [SerializeField] private float _distanceToAttack;

    [Header("Attack")]
    [SerializeField] private float _damage;
    [SerializeField] private float _attackPeriod;
    private float _timer;

    private Vector3 _targetPoint = Vector3.positiveInfinity;
    private Enemy _targetEnemy;

    private readonly FsmStack _brain = new FsmStack();

    protected override void Start() {
        base.Start();

        _targetPoint = transform.position;
        _brain.PushState(Idle);
    }

    private void Update() {
        _brain.Update();
    }

    private void Idle() {
        FindClosestUnit();

        float distance = Vector3.Distance(transform.position, _targetPoint);

        if (distance > 2f) {
            _brain.PushState(WalkToPoint);
        }
    }

    private void WalkToPoint() {
        _navMeshAgent.SetDestination(_targetPoint);

        float distance = Vector3.Distance(transform.position, _targetPoint);

        if (distance <= _navMeshAgent.stoppingDistance) {
            _brain.PopState();
        }
    }

    private void WalkToEnemy() {
        if (!_targetEnemy) {
            _brain.PopState();
            return;
        }

        _navMeshAgent.SetDestination(_targetEnemy.transform.position);
        float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);

        if (distance > _distanceToFollow) {
            _brain.PopState();
        }

        if (distance <= _distanceToAttack) {
            _navMeshAgent.SetDestination(transform.position);

            _brain.PushState(Attack);
        }
    }

    private void Attack() {
        if (!_targetEnemy) {
            _brain.PopState();
            _timer = 0;
            FindClosestUnit();

            return;
        }

        float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);

        if (distance > _distanceToAttack) {
            _brain.PopState();
            _timer = 0;
        }

        _timer += Time.deltaTime;

        if (_timer > _attackPeriod) {
            _targetEnemy.TakeDamage(_damage);
            _timer = 0;
        }
    }

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _targetPoint = point;

        _brain.ClearState();
        _brain.PushState(Idle);
        _brain.PushState(WalkToPoint);
    }

    private void FindClosestUnit() {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        for (int i = 0; i < enemies.Length; i++) {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                closestEnemy = enemies[i];
            }
        }

        if (minDistance < _distanceToFollow) {
            _targetEnemy = closestEnemy;

            _brain.PushState(WalkToEnemy);
        }
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        FindClosestUnit();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}
