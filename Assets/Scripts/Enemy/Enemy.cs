using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [Header("Distance")]
    [SerializeField] private float _distanceToFollow;
    [SerializeField] private float _distanceToAttack;

    [Header("Health")]
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private float _health;

    [Header("Attack")]
    [SerializeField] private float _damage;
    [SerializeField] private float _attackPeriod;
    private float _timer;

    private NavMeshAgent _navMeshAgent;
    private Building _targetBuildings;
    private Unit _targetUnit;

    private readonly FsmStack _brain = new FsmStack();

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _healthBar.Init(_health);

        _brain.PushState(Idle);
    }

    private void Update() {
        _brain.Update();
    }

    private void Idle() {
        FindClosestBuilding();
        FindClosestUnit();
    }

    private void WalkToBuilding() {
        Idle();

        if (!_targetBuildings) {
            _brain.PopState();
            return;
        }

        _navMeshAgent.SetDestination(_targetBuildings.transform.position);
    }

    private void WalkToUnit() {
        if (!_targetUnit) {
            _brain.PopState();
            return;
        }

        _navMeshAgent.SetDestination(_targetUnit.transform.position);
        float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);

        if (distance > _distanceToFollow) {
            _brain.PopState();
        }

        if (distance <= _distanceToAttack) {
            _navMeshAgent.SetDestination(transform.position);

            _brain.PushState(Attack);
        }
    }

    private void Attack() {
        if (!_targetUnit) {
            _brain.PopState();
            _timer = 0;

            return;
        }

        float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);

        if (distance > _distanceToAttack) {
            _brain.PopState();
            _timer = 0;
        }

        _timer += Time.deltaTime;

        if (_timer > _attackPeriod) {
            _targetUnit.TakeDamage(_damage);
            _timer = 0;
        }
    }

    private void FindClosestBuilding() {
        Building[] buildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for (int i = 0; i < buildings.Length; i++) {
            if (!buildings[i].enabled) {
                continue;
            }

            float distance = Vector3.Distance(transform.position, buildings[i].transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                closestBuilding = buildings[i];
            }
        }

        _targetBuildings = closestBuilding;

        if (_targetBuildings) {
            _brain.PushState(WalkToBuilding);
        }
    }

    private void FindClosestUnit() {
        Unit[] units = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < units.Length; i++) {
            float distance = Vector3.Distance(transform.position, units[i].transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                closestUnit = units[i];
            }
        }

        if (minDistance < _distanceToFollow) {
            _targetUnit = closestUnit;

            _brain.PushState(WalkToUnit);
        }
    }

    public void TakeDamage(float damage) {
        _health -= damage;
        _healthBar.UpdateHealth(_health);

        if (_health <= 0) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
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
