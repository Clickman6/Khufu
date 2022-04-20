using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Transform _foreground;
    private float _maxHealth;

    private Transform _camera;

    private void Start() {
        _camera = Camera.main.transform;
    }

    private void Update() {
        transform.rotation = _camera.rotation;
    }

    public void UpdateHealth(float health) {
        float scale = Mathf.Clamp01(health / _maxHealth);

        _foreground.localScale = new Vector3(scale, 1f, 1f);
    }

    public void Init(float maxHealth) {
        _maxHealth = maxHealth;
    }
}
