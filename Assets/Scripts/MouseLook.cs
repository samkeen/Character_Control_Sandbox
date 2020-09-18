using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// horizontal mouse mvmnt rotates body around the y-axis
// vert mouse mvmnt rotates the head around the x-axis
public class MouseLook : MonoBehaviour
{
    /// <summary>
    /// @REFACTOR as per https://www.youtube.com/watch?v=MfIsp28TYAQ
    /// - read input goes to Update()
    /// - apply forces goes to FixedUpdate
    /// 
    /// </summary>
    [SerializeField] private float turnSpeed = 90f;

    // upper angle in deg
    [SerializeField] private float headUpperAngleLimit = 85f;

    // lower angle in deg
    [SerializeField] private float headLowerAngleLimit = -80f;

    // current rotation from start in deg
    private float _yaw = 0f;
    private float _pitch = 0f;

    // Well deliver new orientations by combining with measured yaw and pitch
    private Quaternion _headStartOrientation;
    private Quaternion _bodyStartOrientation;

    // ref to the head, the object to rotate up and down.  The body in the current 
    // object (this) so we do not need ref to it.
    // Net exposed as SerializedField rather we will locate it by looking for a 
    // camera child object.
    private Transform _head;

    void Start()
    {
        this._head = GetComponentInChildren<Camera>().transform;
        // cache location of body and head
        this._headStartOrientation = this._head.localRotation;
        this._bodyStartOrientation = this.transform.localRotation;
    }

    // Everytime physics updates, update our mvmnt.
    // If you were not interacting with the physics objects, we could do this in Update()
    // see: https://www.youtube.com/watch?v=MfIsp28TYAQ
    void FixedUpdate()
    {
        // Read the current horiz mvmnt and scale it based on the amnt
        // of time that has elapsed adn the mvmnt speed
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        // same for vert
        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        // update yaw and pitch
        this._yaw += horizontal;
        this._pitch += vertical;

        // Clamp pitch between limits
        this._pitch = Mathf.Clamp(this._pitch, this.headLowerAngleLimit, this.headUpperAngleLimit);
        
        // compute the rotation for body by rotating around the y-axis by the
        // number of yaw deg and for the head around the number of pitch deg.
        var bodyRotation = Quaternion.AngleAxis(this._yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(this._pitch, Vector3.right);
        
        // create new rotations by combining with their start rotations
        this.transform.localRotation = bodyRotation * this._bodyStartOrientation;
        this._head.localRotation = headRotation * this._headStartOrientation;
    }
}