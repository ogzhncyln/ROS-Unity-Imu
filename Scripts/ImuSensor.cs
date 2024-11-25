using UnityEngine;
using System;

public class ImuSensor : MonoBehaviour
{
    [HideInInspector]
    public Vector3 linearAcceleration;
    [HideInInspector]  
    public Vector3 angularVelocity;
    [HideInInspector]     
    public Quaternion orientation;      

    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = (currentPosition - lastPosition) / Time.fixedDeltaTime;
        linearAcceleration = (currentVelocity - lastVelocity) / Time.fixedDeltaTime;
        
        linearAcceleration.y = Physics.gravity.y;

        linearAcceleration = transform.InverseTransformDirection(linearAcceleration);

        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        angularVelocity = axis * angle * Mathf.Deg2Rad / Time.fixedDeltaTime;

        angularVelocity = new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z);

        orientation = transform.rotation;

        lastPosition = currentPosition;
        lastVelocity = currentVelocity;
        lastRotation = transform.rotation;
    }
}
