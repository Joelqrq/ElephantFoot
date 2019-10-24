using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public float height = 10;
    public float distance = 10;
    public float angle = 0;

    void Start()
    {
        HandleCamera();
    }

    void Update()
    {
        HandleCamera();
        //OrbitCamera();
    }

    public void HandleCamera()
    {
        if (target)
        {
            Vector3 worldPos = (Vector3.forward * -distance) + (Vector3.up * height);

            Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPos;

            Vector3 flatTargetPos = target.position;
            flatTargetPos.y = 0;

            Vector3 finalPos = flatTargetPos + rotatedVector;

            transform.position = finalPos;
            transform.rotation = Quaternion.LookRotation(-rotatedVector);
        }
    }

    public void OrbitCamera()
    {
        float orbitInput = Input.GetAxisRaw("Mouse ScrollWheel") * 100;

        angle += orbitInput;
    }
}
