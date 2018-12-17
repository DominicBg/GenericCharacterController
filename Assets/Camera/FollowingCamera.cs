using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour {

    [SerializeField] Transform target;

    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] float speed;
    [SerializeField] float minAngle;
    [SerializeField] float height;
    [SerializeField] float lookOverTarget;


	void Update ()
    {
        if(TooFarOrCloseFromTarget())
            ReplaceCamera();

        transform.LookAt(target.position + Vector3.up * lookOverTarget);
    }

    bool TooFarOrCloseFromTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance > maxDistance || distance < minDistance;
    }

    bool IsOverMinAngle()
    {
        return Mathf.Abs(target.eulerAngles.y - transform.eulerAngles.y) > minAngle;
    }

    void ReplaceCamera()
    {
        Vector3 destination = target.position + -target.forward * maxDistance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
    }
}
