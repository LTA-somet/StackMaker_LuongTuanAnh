using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxDistance = 0.01f;
    [SerializeField] GameObject stackPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform anim;
    [SerializeField] Transform stackHolder;
    [SerializeField] GameObject Stand;
    [SerializeField] GameObject WinUI;

 //   private int StackHolderCount=0;
    private Vector3 aPoint = Vector3.zero;
    private Vector3 bPoint = Vector3.zero;
    private Vector3 dirVector = Vector3.zero;
    private Vector3 moveVector = Vector3.zero;
    private bool isMove = false;
    private Vector3[] unitVectors = new Vector3[]
    {
        Vector3.right,
        Vector3.left,
        Vector3.back,
        Vector3.forward,
    };
    public bool isWin = false;
   
    void Update()
    {
        CheckMove();
        CheckRayCast();     
    }
    private void CheckMove()
    {
        if (!isMove)
        {
            SetInput();
        }

        if (isMove)
        {
            Move();
        }
    }
    private void CheckRayCast()
    {
        Ray ray = new Ray(Stand.transform.position, transform.right);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            float stackHeight = 1.0f;
            if (hitInfo.collider.tag == "Wall")
            {
                StopMove();

            }
           
            if (hitInfo.collider.tag == "Stack")
            {
                Stack(stackHeight,hitInfo);
               
            }

            if (hitInfo.collider.tag == "Bridge")
            {
             Bridge(stackHeight,hitInfo);
            }

            if (hitInfo.collider.tag == "Winpos")
            {
              StartCoroutine(  Winpos(stackHeight));
                
            }
        }
        Debug.DrawRay(Stand.transform.position, transform.right * maxDistance, Color.red);
    }
    private void Stack(float stackHeight, RaycastHit ray)
    {
        
        Instantiate(stackPrefab, spawnPoint.position, Quaternion.Euler(-90, 0, 0), stackHolder);
        spawnPoint.transform.position += new Vector3(0, stackHeight, 0);
        anim.transform.position += new Vector3(0, stackHeight, 0);
        ReturnToPool(ray.collider.gameObject);

    }
    private void Bridge(float stackHeight,RaycastHit ray)
    {
       
        ray.collider.GetComponent<MeshRenderer>().enabled = true;
        ray.collider.GetComponent<BoxCollider>().enabled = false;
        if (stackHolder.childCount > 0)
        {
            Transform topStack = stackHolder.GetChild(stackHolder.childCount - 1);
            Destroy(topStack.gameObject);
            spawnPoint.transform.position -= new Vector3(0, stackHeight, 0);
            anim.transform.position -= new Vector3(0, stackHeight, 0);
        }
        else
        {
            StopMove();
        }
    }
    private IEnumerator Winpos(float stackHeight)
    {
        yield return new WaitForSeconds(0.5f);
        isWin = true; 
        StopMove();
        stackHolder.gameObject.SetActive(false);
        anim.position=Stand.transform.position;
        
    }
    private void StopMove()
    {
        dirVector = Vector3.zero;
        moveVector = Vector3.zero;
        isMove = false;
    }
    public void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aPoint = Input.mousePosition;
            aPoint.z = aPoint.y;
            aPoint.y = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            bPoint = Input.mousePosition;
            bPoint.z = bPoint.y;
            bPoint.y = 0;
            dirVector = bPoint - aPoint;
            if (dirVector.magnitude > 0.01f)
            {
                isMove = true; 
                Move();
            }
        }
    }

    public void Move()
    {
        if (dirVector.magnitude > 0.01f)
        {
            dirVector.Normalize();
            float maxDotProduct = float.MinValue;

            foreach (Vector3 unitVector in unitVectors)
            {
                float dotProduct = Vector3.Dot(dirVector, unitVector);
                if (dotProduct > maxDotProduct)
                {
                    maxDotProduct = dotProduct;
                    moveVector = unitVector;
                }
            }
        }

        if (moveVector .magnitude>0.01f)
        {
            float angle = Mathf.Atan2(moveVector.z, moveVector.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.down);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position += moveVector * speed * Time.deltaTime;
    }

   

    public void ReturnToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
