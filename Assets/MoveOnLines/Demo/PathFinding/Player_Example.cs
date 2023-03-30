using UnityEngine;

public class Player_Example : MonoBehaviour
{
    public bool isActive = true;

    public float speed = 10;
    public int lookNextPosIndex = 2;
    public bool useRayCast = false;
    public Vector3 target = new Vector3();
    public GameObject tailLight = null;

    
    private void Start()
    {
        cameraFirstPos = Camera.main.transform.position;
        target = transform.position;
    }


    private void Update()
    {
        if (!isActive) return;
        //Select target by mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            if (useRayCast) //use raycast for 3d scene
            {
                Ray ray = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.point;
                }
            }
            else
            {
                mouse.z = -Camera.main.transform.position.z;
                target = Camera.main.ScreenToWorldPoint(mouse);
            }
        }

        ControlCamera();


        
        PathFinding.Move(transform, target, speed, lookNextPosIndex, 10);
        //Use move function in PathFinding for move object to target
        //or use PathFinding.Find(transform.position, target) to return list positition between transform.position, target and write your movement script.

        if (PathFinding.IsMove(transform)) tailLight.SetActive(false); //tail light off when car move
        else tailLight.SetActive(true); //tail light on when car stop

    }

    Vector3 cameraFirstPos = new Vector3();
    float camera_size = 10;
    private void ControlCamera()
    {
        //Move Camera
        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * 2;
        }
        else if (!transform.GetChild(0).gameObject.activeSelf) //When tail light off car is move => Camera position = transform position
            Camera.main.transform.position = transform.position + cameraFirstPos;

        //Zoom Camera
        camera_size = Mathf.Clamp(camera_size - Input.mouseScrollDelta.y * 3, 5, 50);
        Camera.main.orthographicSize = camera_size;
    }
}

