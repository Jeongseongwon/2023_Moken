using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene5controll : MonoBehaviour
{
    public GameObject nextscene;
    public GameObject nowScene;
     public GameObject[] targets;
    public int TargetIndex;
    public GameObject mainobject;
    private bool flag=false;
    int num=0;
    public  int speed;
    public int count;//카운트 해서 노드 갯수를 적을 것
    // Start is called before the first frame update
    void Start()
    {
        flag=true;
    }


    // Update is called once per frame
    void Update()
    {
      if(flag==true)
      {
          move();

      }
      else
      {
      
       next();
      }
      
        
    }
    public void move()
    {
      
        mainobject.GetComponent<RectTransform>().position=Vector3.MoveTowards(mainobject.GetComponent<RectTransform>().position,targets[num].GetComponent<RectTransform>().position,speed*Time.deltaTime);
        if(mainobject.GetComponent<RectTransform>().position==targets[num].GetComponent<RectTransform>().position)
        {
        
           if(num<4)
           num++;
           else
           flag=false;

        }
        
        

    }
  
     public void next()
    {
        if(num==count)
        {
              nowScene.SetActive(false);
            nextscene.SetActive(true);
          

        }
    }
    
}
