using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLogger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"object {other.gameObject.name} started touching {this.name}");
    }
    private void OnCollisionExit(Collision other)
    {
        Debug.Log($"object {other.gameObject.name} stopped touching {this.name}");
    }
    private void OnCollisionStay(Collision other)
    {
        Debug.Log($"object {other.gameObject.name} remained touching {this.name}");
    }
}
