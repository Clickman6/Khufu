using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceLabel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _label;

    public void UpdatePrice(float amount) {
        _label.text = $"{amount} $";
    }
}
