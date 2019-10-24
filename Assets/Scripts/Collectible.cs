#pragma warning disable CS0649
using UnityEngine;
using JoelQ.Helper;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float healAmount = 2f;
    [SerializeField] private LayerMask collectibleMask;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer.CompareLayer(collectibleMask)) {
            other.GetComponent<CharacterMovement>()?.AddHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
