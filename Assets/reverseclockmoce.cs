using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverseclockmoce : MonoBehaviour
{
    public GameObject spin;
  private float count=0;
  private bool flag=true;
  public GameObject target;
  float timer=0.0f;
  int waitingTime=3;
  
  [Header("회전속도조절")]
  [SerializeField] [Range(1f,100f)]  
  float  rotateSpeed=50f;

   

   void Update()
   {
    
    
    

    move();
  
   }
     

     private void OnCollisionEnter2D(Collision2D other)
    {
        flag=false;
        
    }
    void wait()
    {
       timer +=Time.deltaTime;
       if(timer>waitingTime)
       {
        flag=true;
        timer=0;
       }

    }
 
    


    void move()
    {
       
       if(flag==false)
       {
        wait();
       }
       
        
        else
        {
        
        spin.GetComponent<RectTransform>().Rotate(0,0,-Time.deltaTime*rotateSpeed,Space.Self);
       
        }
        
   
     
        
 
   

    }
}
