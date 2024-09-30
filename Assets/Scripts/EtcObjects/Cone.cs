using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            GameManager.Instance.IsFailed = true;
            GameManager.Instance.IsGameOver = true;
        }
    }
}
