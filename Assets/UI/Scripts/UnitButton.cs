using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class UnitButton : MonoBehaviour {
    [SerializeField] private Unit _unit;
    [SerializeField] private TextMeshProUGUI _priceLabel;
    [SerializeField] private UnityEvent<Unit> _onBuyEvent;

    private void Start() {
        _priceLabel.text = $"{_unit.Price} $";
    }

    public void TryBuy() {
        float price = _unit.Price;

        if (Resources.Instance.Money < price) return;

        Resources.Instance.Money -= price;
        _onBuyEvent.Invoke(_unit);
    }
}
