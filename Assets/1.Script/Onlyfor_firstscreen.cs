using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Onlyfor_firstscreen : MonoBehaviour
{
    [Header("ONLY FOR TEST")]
    public GameObject temp_timer;
    public float time = 6f;

    [Header("First screen")]
    public GameObject First_screen;

    [Header("After first screen")]
    public GameObject Game_obj;


    public bool timer_setting;
    public float Limit_time;

    private float Temp_timer;
    // Start is called before the first frame update
    void Start()
    {
        Temp_timer = Limit_time;
        if (timer_setting == true)
        {
            Debug.Log("Timer increase");
        }
        else if (timer_setting == false)
        {
            Debug.Log("Timer decrease");
        }
    }

    // Update is called once per frame
    void Update()
    {
        temp_timer.GetComponent<Text>().text = string.Format("{0:N1}", Temp_timer);
        // temp_timer.GetComponent<Text>().text = Temp_timer.ToString();
        //Timer
        if (timer_setting = true)
        {
            Temp_timer += Time.deltaTime;
            if (Temp_timer > time)
            {
                First_screen.SetActive(false);
                Game_obj.SetActive(true);
            }
        }
        else if (timer_setting = false)
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
