using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using KevinCastejon.ConeMesh;

public class Cone : MonoBehaviour
{
    Color color;
    Image image;

    Renderer MyRenderer;

    private void Start()
    {
        MyRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Explosion"))
        {
            GameManager.Instance.IsFailed = true;
            GameManager.Instance.IsGameOver = true;
        }
    }

    public IEnumerator FadeOut()
    {
        float f = 0.3f;
        while (f > 0)
        {
            f -= 0.05f;
            Color ColorAlhpa = MyRenderer.material.color;
            ColorAlhpa.a = f;
            MyRenderer.material.color = ColorAlhpa;
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
    }

}
