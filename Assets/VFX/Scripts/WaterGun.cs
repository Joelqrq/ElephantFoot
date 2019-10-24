using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : MonoBehaviour
{
    public Transform vfxParticleSystem;

    private MeshRenderer vfxMeshRenderer;

    private void Start()
    {
        vfxMeshRenderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerStay(Collider other)
    {
        UpdateStream(GetHeight(other));
    }

    private void OnTriggerExit(Collider other)
    {
        UpdateStream(0);
    }

    private float GetHeight(Collider collider)
    {
        return collider.transform.position.z + collider.bounds.size.z;
    }

    private void UpdateStream(float newHeight)
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, newHeight);
        vfxParticleSystem.position = newPosition;

        //cutoff
        newHeight /= transform.localScale.y;
        vfxMeshRenderer.material.SetFloat("_Cutoff", newHeight);
    }
}
