using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clockmovefull : MonoBehaviour
{
  public GameObject spin;
  public GameObject[] targets;
  private float count=0;
  private bool flag=true;
  public GameObject target;
  float timer=0.0f;
  int waitingTime=3;
  float speed_niddeld;
  [Header("회전속도조절")]
  [SerializeField] [Range(1f,100f)]  
  float  rotateSpeed=30f;
  
  float niddledgree=-120;
  float niddlespeed;
  int num=0;
   


void Start()
{
    
    
    spin.GetComponent<RectTransform>().Rotate(0,0,niddledgree,Space.Self);

    targets[num].SetActive(true);
}
   void Update()
   {
    
    
    

    move();
  
   }
     

  
    void wait()
    {
        
       timer +=Time.deltaTime;
       if(timer>waitingTime)
       {
          Debug.Log(niddledgree);
         spin.GetComponent<RectTransform>().Rotate(0,0,niddledgree,Space.Self); 
             
         
        flag=true;
        timer=0;
        
       }

    }
    void spining()
    {
       
      

     
             spin.GetComponent<RectTransform>().Rotate(0,0,-Time.deltaTime*rotateSpeed,Space.Self);


        
    
        
    }
        private void OnCollisionEnter2D(Collision2D other)
    {
        
        flag=false;
        
        targets[num].SetActive(false);
        num++;
        
        targets[num].SetActive(true);
        
    }

 
    


    void move()
    {
       
       if(flag==false)
       {
        wait();
       }
       
        
        else
        {
            
            spining();

        
       
      
        }
        
   
     
        
 
   

    }
}
