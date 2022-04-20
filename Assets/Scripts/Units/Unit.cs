using System;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject {
    protected NavMeshAgent _navMeshAgent;

    [Header("Health")]
    [SerializeField] private float _health;
    [SerializeField] private HealthBar _healthBar;

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected override void Start() {
        base.Start();

        _healthBar.Init(_health);
    }

    public virtual void TakeDamage(float damage) {
        _health -= damage;
        _healthBar.UpdateHealth(_health);

        if (_health <= 0) {
            Die();
        }
    }

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point);
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    protected virtual bool CheckDistance(Vector3 point, float distance) {
        return Vector3.Distance(transform.position, point) <= distance;
    }
}
