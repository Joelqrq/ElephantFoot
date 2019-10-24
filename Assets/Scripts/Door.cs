#pragma warning disable CS0649
using UnityEngine;
public class Door : MonoBehaviour {

    [SerializeField] private bool isLocked = false;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider doorCollider;

    private void OnTriggerStay(Collider other) {
        if (isLocked)
            return;

        doorCollider.isTrigger = true;
        animator.SetBool("state", true);
    }

    private void OnTriggerExit(Collider other) {
        if (isLocked)
            return;

        doorCollider.isTrigger = false;
        animator.SetBool("state", false);
    }
}
