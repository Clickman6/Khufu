using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : Singleton<BuildingPlacer> {
    public static readonly float CellSize = 1f;

    private Plane _plane;
    private Building _currentBuilding;
    private readonly Dictionary<Vector2Int, Building> _buildings = new Dictionary<Vector2Int, Building>();

    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private Material _denyMaterial;

    public bool CanBuy => _currentBuilding == null;

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        if (!_currentBuilding) return;

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);

        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        _currentBuilding.transform.position = new Vector3(x, 0f, z) * CellSize;

        if (CheckAllow(x, z, _currentBuilding)) {
            _currentBuilding.DisplayAcceptablePosition();

            if (Input.GetMouseButtonDown(0)) {
                InstantiateBuilding(x, z, _currentBuilding);
                _currentBuilding.OnBuild();
                _currentBuilding = null;
            }
        } else {
            _currentBuilding.DisplayUnacceptablePosition(_denyMaterial);
        }
    }

    private bool CheckAllow(int xPos, int zPos, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {
                Vector2Int index = new Vector2Int(xPos + x, zPos + z);

                if (_buildings.ContainsKey(index)) return false;
            }
        }

        return true;
    }

    private void InstantiateBuilding(int xPos, int zPos, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {
                Vector2Int index = new Vector2Int(xPos + x, zPos + z);

                _buildings.Add(index, building);
            }
        }
    }

    public void CreateBuilding(Building building) {
        _currentBuilding = Instantiate(building);
        _currentBuilding.OnInstantiate();
    }
}
