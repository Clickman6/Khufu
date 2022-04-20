using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState {
    UnitsSelected,
    BuildingSelected,
    Frame,
    Other
}

public class Management : Singleton<Management> {
    [SerializeField] private Camera _camera;
    private SelectableObject _hovered;
    private readonly List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    [Header("Frame")]
    [SerializeField] private Image _frameImage;
    private Vector2 _startFrame;
    private Vector2 _endFrame;
    private SelectionState _currentSelectionState;

    private void Update() {
        SelectableObject hitSelectable = null;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.collider.TryGetComponent(out SelectableCollider selectable)) {
                hitSelectable = selectable.SelectableObject;
            }
        }

        OnHover(hitSelectable);

        if (Input.GetMouseButtonUp(0)) {
            if (_hovered) {
                ClickOnSelectableObject(_hovered);
            }
        }

        if (_currentSelectionState == SelectionState.UnitsSelected) {
            if (Input.GetMouseButtonUp(0)) {
                if (hit.collider && hit.collider.CompareTag("Ground")) {
                    ClickOnGround(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            UnselectAll();
        }

        if (Input.GetMouseButtonDown(0)) {
            _startFrame = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            _endFrame = Input.mousePosition;
            Vector2 start = Vector2.Min(_startFrame, _endFrame);
            Vector2 end = Vector2.Max(_startFrame, _endFrame);
            Vector2 size = end - start;

            if (size.magnitude > 10f) {
                DrawSelectionFrame(start, size);
                SelectInFrame(start, size);
                _currentSelectionState = SelectionState.Frame;
            }
        }

        if (_currentSelectionState == SelectionState.Frame) {
            if (!Input.GetMouseButtonUp(0)) return;

            _frameImage.enabled = false;
            _currentSelectionState = SelectionState.Other;

            if (_listOfSelected.Count > 0) {
                _currentSelectionState = SelectionState.UnitsSelected;
            }
        }
    }

    private void DrawSelectionFrame(Vector2 start, Vector2 size) {
        _frameImage.enabled = true;
        _frameImage.rectTransform.anchoredPosition = start;
        _frameImage.rectTransform.sizeDelta = size;
    }

    private void SelectInFrame(Vector2 start, Vector2 size) {
        Rect rect = new Rect(start, size);

        UnselectAll();
        Unit[] units = FindObjectsOfType<Unit>();

        for (int i = 0; i < units.Length; i++) {
            Vector2 pos = _camera.WorldToScreenPoint(units[i].transform.position);

            if (rect.Contains(pos)) {
                Select(units[i]);
            }
        }
    }

    private void ClickOnSelectableObject(SelectableObject selectableObject) {
        if (_currentSelectionState == SelectionState.Frame) return;

        SelectionState newState = SelectionState.Other;

        if (selectableObject is Unit) {
            newState = SelectionState.UnitsSelected;

            if (!Input.GetKey(KeyCode.LeftControl)) {
                UnselectAll();
            }
        } else if (selectableObject is Building) {
            newState = SelectionState.BuildingSelected;
            UnselectAll();
        }

        if (newState != _currentSelectionState) {
            UnselectAll();
        }

        _currentSelectionState = newState;

        Select(selectableObject);
    }

    private void ClickOnGround(Vector3 point) {
        int rows = Mathf.CeilToInt(Mathf.Sqrt(_listOfSelected.Count));

        for (int i = 0; i < _listOfSelected.Count; i++) {
            Vector3 position = point + new Vector3(i / rows, 0f, i % rows);

            _listOfSelected[i].WhenClickOnGround(position);
        }
    }

    private void Select(SelectableObject selectableObject) {
        if (_listOfSelected.Contains(selectableObject)) return;

        _listOfSelected.Add(selectableObject);
        selectableObject.Select();
    }

    private void UnselectAll() {
        for (int i = 0; i < _listOfSelected.Count; i++) {
            _listOfSelected[i].Unselect();
        }

        _listOfSelected.Clear();
        _currentSelectionState = SelectionState.Other;
    }

    private void OnHover(SelectableObject selectableObject) {
        if (_hovered == selectableObject) return;

        if (_hovered) _hovered.OnUnhover();
        _hovered = selectableObject;
        if (_hovered) _hovered.OnHover();
    }

    public void Unselect(SelectableObject selectableObject) {
        if (!_listOfSelected.Contains(selectableObject)) return;

        _listOfSelected.Remove(selectableObject);
        selectableObject.Unselect();

        if (_listOfSelected.Count == 0) {
            _currentSelectionState = SelectionState.Other;
        }
    }
}
