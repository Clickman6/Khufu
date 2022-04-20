using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _interval;
    private float _timer;

    private void Update() {
        _timer += Time.deltaTime;

        if (_timer > _interval) {
            _timer = 0f;
            Instantiate(_prefab, transform.position, transform.rotation);
        }
    }
}
