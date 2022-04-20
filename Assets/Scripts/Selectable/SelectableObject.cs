using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {
    public float Price;

    [SerializeField] private GameObject _indicator;

    protected virtual void Start() {
        Unselect();
    }

    public void OnHover() {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnUnhover() {
        transform.localScale = Vector3.one;
    }

    public virtual void Select() {
        _indicator.SetActive(true);
    }

    public virtual void Unselect() {
        _indicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point) { }

    private void OnDestroy() {
        Management.Instance.Unselect(this);
    }
}
