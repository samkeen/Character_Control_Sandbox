using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [SerializeField] private float interactingRange = 2;

    void Update()
    {
        if (Input.GetKeyDown(this.interactionKey))
        {
            AttemptInteraction();
        }
    }

    private void AttemptInteraction()
    {
        // create a ray from the current position & fwd direction
        var ray = new Ray(this.transform.position, this.transform.forward);
        // var to store info about hit
        RaycastHit hit;
        // create a layer mask that represents every layer except the player's layer
        //
        // The ~ operator performs a bitwise complement operation on its operand, which has the effect of
        // reversing each bit
        //
        // Binary Left Shift Operator <<. The left operands value is moved left by the number of bits specified by
        // the right operand.
        var everythingExceptPlayers = ~(1 << LayerMask.NameToLayer("Player"));

        // combine this layer mask with the one that raycasts usually use; this has the effect of
        // removing the player layer from the list of layers to raycast against
        //
        // Binary AND Operator (&) copies a bit to the result if it exists in both operands.
        var layerMask = Physics.DefaultRaycastLayers & everythingExceptPlayers;

        // perform the raycast out, hitting only objects that are on the layers described by the
        // layer mask just assembled
        if (Physics.Raycast(ray, out hit, this.interactingRange, layerMask))
        {
            // try to get the interactable of what we hit
            var interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject);
            }
        }
    }
}