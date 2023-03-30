using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenechange : MonoBehaviour
{
    public GameObject[] Sceen;
    public GameObject[] mainObject;
    public GameObject[] ChangeSpot;
    private int chaptercount=0;
    private RectTransform positionnode;
    private

    // Start is called before the first frame update
    void Start()
    {
       
     

    }
    void chapter1()
    {
       

        Sceen[0].SetActive(true);
         Sceen[1].SetActive(false);
          Sceen[2].SetActive(false);
          

          }

        

        
        
        
            
        

    
    void chapter2()
    {
          Sceen[0].SetActive(false);
         Sceen[1].SetActive(true);
          Sceen[2].SetActive(false);

       
    }
    void chapter3()
    {
          Sceen[0].SetActive(false);
         Sceen[1].SetActive(false);
          Sceen[2].SetActive(true);
    }



    
}
