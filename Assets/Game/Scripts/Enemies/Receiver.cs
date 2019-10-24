#pragma warning disable CS0649
using UnityEngine;

public abstract class Receiver : MonoBehaviour
{
    [SerializeField] protected Trigger trigger;

    protected virtual void Start() {
        trigger.OnTrigger += Activate;
    }

    protected abstract void Activate();
}
