using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Explosion")
        {
            GameManager.Instance.IsFailed = true;
            GameManager.Instance.IsGameOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Explosion")
        {
            GameManager.Instance.IsFailed = true;
            GameManager.Instance.IsGameOver = true;
        }
    }
}
