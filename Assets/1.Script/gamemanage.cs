using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gamemanage : MonoBehaviour
{
     public GameObject targetposition1;
    public GameObject targetposition2;
    public GameObject targetposition3;
    public GameObject targetposition4;
    public GameObject targetposition5;
    public GameObject mainobject;
   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
       
        
        mainobject.GetComponent<RectTransform>().position =Vector3.MoveTowards(mainobject.GetComponent<RectTransform>().position,
        targetposition2.GetComponent<RectTransform>().position,2);
       
        
        
       
      
    
         
    }
}
