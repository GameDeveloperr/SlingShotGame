using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public Transform Target = null;

    private float Dist = 4.0f;
    private float CamHeight = 1.5f;
    private float RotSpeed = 100.0f;

    public void FollowBoom()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, RotSpeed * Time.deltaTime);

        transform.position = Target.position - (Target.forward * Dist) + (Vector3.up * CamHeight);
        transform.LookAt(Target);
    }
    public void SetPosition()
    {
        if (Target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, RotSpeed * Time.deltaTime);

            transform.position = Target.position - (Target.forward * Dist) + (Vector3.up * CamHeight);
        }
    }
    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
