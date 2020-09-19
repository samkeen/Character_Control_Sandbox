using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float gravity = 20;

    // the degree to which we can control movmnt in mid air
    [Range(0, 10), SerializeField] private float airControl = 5f;
    
    // our current mvmnt direction. If we are on the ground we direct control over it
    // else if we are in the air, we have only partial control over it.
    // Start is called before the first frame update
    private Vector3 _moveDirection = Vector3.zero;

    private CharacterController _characterController;
    void Start()
    {
        this._characterController = GetComponent<CharacterController>();
    }
    
    void FixedUpdate()
    {   
        // this vector describes the characters desired local-space mvmnt
        // if we are on the ground, this will immediately become our movmnt
        // but if in air, we'll interpolate between our current mvmnt and this vector
        // to simulate momentum
        var input = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        
        // multiply by our desired movement speed
        input *= this.moveSpeed;
        
        // the controller's move method uses world-space directions, so we need to 
        // convert input's local direction to world space
        input = this.transform.TransformDirection(input);
        
        // Is the controller's bottom most point touching the ground?
        if (this._characterController.isGrounded)
        {
            // determine how much mvmnt we want to apply to local space
            this._moveDirection = input;
            // Is the character pressing the jump button right now?
            if (Input.GetButton("Jump"))
            {
                // calc the amt of upward speed we need
                // considering that we add moveDirection.y to our height every frame, and we
                // reduce moveDirection.y by gravity evey frame
                this._moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
            else
            {
                // we're on the ground, but not jumping. reset our downward mvmnt to zero (otherwise
                // because we're continually reducing our y-movement, if we walked off a ledge,
                // we'd suddenly have a huge amount of downward momentum
                this._moveDirection.y = 0;
            }
            
        }
        else
        {
            // slowly bring the mvmnt to the characters desired input, but preserve our current
            // y-direction (so that the arc of the jump is preserved)
            input.y = this._moveDirection.y;
            this._moveDirection = Vector3.Lerp(this._moveDirection, input, this.airControl * Time.deltaTime);
        }
        // bring our y-mvment down gradually by applying gravity over time
        this._moveDirection.y -= gravity * Time.deltaTime;
        
        // move the controller.  The controller will refuse to move into other colliders, which means
        // that we won't clip through the ground or other colliders.  However, this doesn't
        // stop other colliders from moving into us.  For that, we'd need to detect them.  We'll cover this 
        // another recipe
        this._characterController.Move(this._moveDirection * Time.deltaTime);
    }

    // //////////////////
    // Trigger objects allow items to pass through them and rater than set off onCollision, they 
    //   set off onTrigger.  One of the colliders involved must also have a ridged body. 
    //  Note the param is the Collider, the collision since no collision occured
    // //////////////////
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object {other.name} entered trigger {this.name}");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Object {other.name} exited trigger {this.name}");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Object {other.name} remained in trigger {this.name}");
    }
    
}
