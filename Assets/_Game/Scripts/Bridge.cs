using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] MeshRenderer MeshRenderer;
    [SerializeField] Collider Collider;
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           MeshRenderer.gameObject.SetActive(false);
            Collider.gameObject.SetActive(true);
        }
    }
}
