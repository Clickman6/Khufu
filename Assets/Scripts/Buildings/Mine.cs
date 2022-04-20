using System;
using UnityEngine;

public class Mine : Building {
    [SerializeField] private float _incomePeriod;
    [SerializeField] private int _incomeAmount;
    [SerializeField] private ParticleSystem _incomeEffect;
    private float _timer;

    private void Update() {
        _timer += Time.deltaTime;
        
        if (_timer > _incomePeriod) {
            _timer = 0;

            _incomeEffect.Play();
            Resources.Instance.IncrementMoney(_incomeAmount);
        }
    }
}
