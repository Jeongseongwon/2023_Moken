using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move5 : MonoBehaviour
{
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
      
          move();

      
        
    }
    public void move()
    {
      
        mainobject.GetComponent<RectTransform>().position=Vector3.MoveTowards(mainobject.GetComponent<RectTransform>().position,targets[num].GetComponent<RectTransform>().position,speed*Time.deltaTime);
        if(mainobject.GetComponent<RectTransform>().position==targets[num].GetComponent<RectTransform>().position)
        {
            
           flag=false;
           num++;
        }
        
        

    }
}
