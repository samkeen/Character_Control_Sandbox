using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact(GameObject fromObject)
    {
        Debug.Log($"I've been interacted with by {fromObject.name}");
    }
}
