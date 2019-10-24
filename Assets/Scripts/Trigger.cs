#pragma warning disable CS0649
using System;
using UnityEngine;
using JoelQ.Helper;

public abstract class Trigger : MonoBehaviour {
    [SerializeField] protected LayerMask triggerMask;
    public event Action OnTrigger;

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.layer.CompareLayer(triggerMask))
            return;
        HasEnter();
    }

    protected abstract void HasEnter();

    private void OnTriggerStay(Collider other) {
        if (!other.gameObject.layer.CompareLayer(triggerMask))
            return;
        HasStay();
    }

    protected virtual void HasStay() {
        InvokeTriggerEvent();
    }

    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.layer.CompareLayer(triggerMask))
            return;
        HasExit();
    }

    protected abstract void HasExit();

    protected void InvokeTriggerEvent() {
        OnTrigger.Invoke();
    }
}
