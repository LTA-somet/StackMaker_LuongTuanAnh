using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private Quaternion initRotate;
    // Start is called before the first frame update
    void Start()
    {
        initRotate = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = initRotate;
    }
}
