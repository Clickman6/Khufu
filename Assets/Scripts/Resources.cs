using UnityEngine;
using UnityEngine.Events;

public class Resources : Singleton<Resources> {
    [SerializeField] private float _money;
    [SerializeField] private UnityEvent<float> _onMoneyChange;

    private void Start() {
        _onMoneyChange.Invoke(_money);
    }

    public float Money {
        get => _money;
        set {
            _money = value;
            _onMoneyChange.Invoke(_money);
        }
    }

    public void IncrementMoney(int amount) {
        Money += amount;
    }
}
