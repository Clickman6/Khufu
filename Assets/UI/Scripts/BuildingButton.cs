using TMPro;
using UnityEngine;

public class BuildingButton : MonoBehaviour {
    [SerializeField] private Building _building;
    [SerializeField] private TextMeshProUGUI _priceLabel;

    private void Start() {
        _priceLabel.text = $"{_building.Price} $";
    }

    public void TryBuy() {
        float price = _building.Price;

        if (!BuildingPlacer.Instance.CanBuy) return;
        if (Resources.Instance.Money < price) return;

        Resources.Instance.Money -= price;
        BuildingPlacer.Instance.CreateBuilding(_building);
    }
}
