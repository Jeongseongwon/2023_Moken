using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class niddlelerpmove_2 : MonoBehaviour
{
    public GameObject spin;
    [Header("회전속도조절")]
    [SerializeField]
    [Range(1f, 100f)]
    float rotateSpeed = 30f;
    float timer = 0.0f;
    int waitingTime = 3;
    bool flag = true;
    float[] niddlerdgreestart = { -180, 180 };
    float[] niddledgreeend = { 180, -180 };
    float nowdegree = 0;
    int count = 0;

    public GameObject nowobject;
    public GameObject nextobject;
    public GameObject nextobject_blink;
    private int check_seq = 0;

    // Start is called before the first frame update
    void Start()
    {
        spin.GetComponent<RectTransform>().Rotate(0, 0, -90, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        move();

    }
    void wait()
    {

        timer += Time.deltaTime;
        if (timer > waitingTime)
        {

            Debug.Log(count);
            Debug.Log(niddlerdgreestart[count]);
            if (check_seq == 1)
            {
                //spin.GetComponent<RectTransform>().Rotate(0, 0, 180, Space.Self);
            }
            if (check_seq == 2)
            {
                Scene_End();
            }

            flag = true;
            timer = 0;

        }

    }
    void Scene_End()
    {
        Debug.Log("Scene End");
        nextobject_blink.SetActive(true);
        //nextobject.SetActive(true);
        nowobject.SetActive(false);
    }
    void rotate()
    {
        if (check_seq == 0 )
        {
            spin.GetComponent<RectTransform>().Rotate(0, 0, +Time.deltaTime * rotateSpeed, Space.Self);
        }
        else if (check_seq == 1)
        {
            spin.GetComponent<RectTransform>().Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);
        }

        degreecheck();
    }
    void degreecheck()
    {
        nowdegree += Time.deltaTime * rotateSpeed;

        if (niddlerdgreestart[count] < niddledgreeend[count])
        {
            if ((niddlerdgreestart[count] + nowdegree) >= niddledgreeend[count])
            {
                flag = false;
                nowdegree = 0;
                Debug.Log("Check_1");
                check_seq++;
            }

        }
        else
        {

            if ((niddledgreeend[count] + nowdegree) >= niddlerdgreestart[count])
            {

                flag = false;
                nowdegree = 0;
                Debug.Log("Check_2");
                check_seq++;
            }

        }

    }
    void move()
    {
        if (flag == false)
        {
            wait();
        }
        else
        {
            rotate();
        }
    }
}
