using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private AudioSource _hitSource;
    [SerializeField]
    private AudioClip _hitWallClip;
    [SerializeField]
    private float _rotationAngle = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        _hitSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotation()
    {
       _hitSource.clip = _hitWallClip;
        _hitSource.Play();
        this.gameObject.transform.Rotate(new Vector3(0, _rotationAngle, 0));
    }
}
