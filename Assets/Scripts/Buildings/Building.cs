using System;
using UnityEngine;
using UnityEngine.AI;

public class Building : SelectableObject {
    public float XSize = 3f;
    public float ZSize = 3f;

    private Material _material;
    private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private Renderer _renderer;

    private void Awake() {
        _material = _renderer.material;
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnDrawGizmos() {
        for (int x = 0; x < XSize; x++) {
            for (int z = 0; z < ZSize; z++) {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0f, z) * BuildingPlacer.CellSize,
                                    new Vector3(1f, 0f, 1f) * BuildingPlacer.CellSize);
            }
        }
    }

    public void DisplayAcceptablePosition() {
        _renderer.material = _material;
    }

    public void DisplayUnacceptablePosition(Material material) {
        _renderer.material = material;
    }

    public void OnBuild() {
        _navMeshObstacle.enabled = true;
        enabled = true;
    }

    public void OnInstantiate() {
        _navMeshObstacle.enabled = false;
        enabled = false;
    }
}
