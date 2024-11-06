using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    private float DestructionDelay = 1.0f;

    void Start()
    {
        Destroy(gameObject, DestructionDelay);
    }
}
