using System;
using UnityEditor;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    private Camera _camera;
    private Vector3 _startPosition;
    private Plane _plane;

    // [SerializeField] private Vector3 _minBorder;
    // [SerializeField] private Vector3 _maxBorder;

    private void Start() {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void LateUpdate() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        if (Input.GetMouseButtonDown(2) || (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))) {
            _startPosition = point;
        }

        if (Input.GetMouseButton(2) || (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))) {
            transform.position -= point - _startPosition;
        }

        transform.Translate(0f, 0f, Input.mouseScrollDelta.y);
    }

    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(_minBorder, 0.5f);
    //     Gizmos.DrawSphere(_maxBorder, 0.5f);
    //
    //     Vector3 center = _minBorder + _maxBorder;
    //     Vector3 size = _maxBorder - _minBorder;
    //
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireCube(center / 2f, size);
    // }
}
