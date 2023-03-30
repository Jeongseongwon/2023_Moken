using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coliisionanwait : MonoBehaviour
{
     float timer=0.0f;
     float waitingTime=1.5f;
     bool flag=true;
     
       public GameObject mowScene;
    public GameObject nextScene;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        move();
        
        
    }
    void move()
    {
        while(true)
        {
            if(flag==true)
             wait();
             else
             change();
                   

             
       
     
        }
              
         
        

    }
    void change()
    {
                  mowScene.SetActive(false);
        nextScene.SetActive(true);

    }
       
    
       void wait()
    {
        
         timer +=Time.deltaTime;
       if(timer>waitingTime)
       {
        flag=false;
        
       }
   
        

        
      

    }
 
}
