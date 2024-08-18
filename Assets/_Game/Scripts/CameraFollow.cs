using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset=new Vector3(0,20,-45);
    public float speed = 10;
  
    private void Start()
    {
        transform.Rotate(30, 0, 0);
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
