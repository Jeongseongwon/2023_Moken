using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    [Header("ONLY FOR TEST")]
    
    public bool timer_setting;
    public float Limit_time;

    private float Temp_timer;
    // Start is called before the first frame update
    void Start()
    {
        Temp_timer = Limit_time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer_setting==true)
        {
            Temp_timer += Time.deltaTime;
        }
        else if(timer_setting == false)
        {
            Temp_timer -= Time.deltaTime;
            if (Temp_timer < 0.1)
            {
                Temp_timer = Limit_time;
            }
        }
        
    }

    public float Get_time()
    {
        return Temp_timer;
    }

}
