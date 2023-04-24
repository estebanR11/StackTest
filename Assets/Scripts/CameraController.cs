using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] float distance = 10.0f;
    [SerializeField] float xSpeed = 120.0f;




    private float x = 0.0f;
    private float y = 0.0f;

    private Quaternion rotation;
    private Vector3 position;
    private Vector3 targetOffset;




    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;
        Vector3 angles = rotation.eulerAngles;
        x = angles.y;
        y = angles.x;


        position = transform.position;

        targetOffset = target.position - transform.position;
        rotation = Quaternion.Euler(y, x, 0);

        position = target.position - (rotation * targetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");

            x += mouseX * xSpeed * 0.02f;
            rotation = Quaternion.Euler(y, x, 0);

            position = target.position - (rotation * targetOffset);

            transform.rotation = rotation;
            transform.position = position;
        }
        else if (Input.GetMouseButtonDown(1))
            
        {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    
                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Block"))
                {
                    BlockBehaviour block = hit.collider.GetComponent<BlockBehaviour>();
                    if (block != null)
                    {
                        block.ShowData();
                    }
             }

        }
    }



    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;


        position = target.position - (rotation * targetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }

  
}
