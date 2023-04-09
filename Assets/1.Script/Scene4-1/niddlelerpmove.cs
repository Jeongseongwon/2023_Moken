using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class niddlelerpmove : MonoBehaviour
{
    public GameObject spin;
    [Header("회전속도조절")]
    [SerializeField]
    [Range(1f, 100f)]
    float rotateSpeed = 30f;
    float timer = 0.0f;
    int waitingTime = 3;
    bool flag = true;
    float[] niddlerdgreestart = { -120, 60 };
    float[] niddledgreeend = { 60, -120 };
    float nowdegree = 0;
    int count = 0;
     public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;
    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp=0;

    public GameObject Fadein;
    public GameObject Fadeout;
    public GameObject Fadeout_2;

    private float Temp_timer;

    private int check_seq = 0;
    private GameObject Game_manger;

    public GameObject nowscene;
    public GameObject nextscene;

    // Start is called before the first frame update
    void Start()
    {
        Check_for_eyeblink = false;
        spin.GetComponent<RectTransform>().Rotate(0, 0, -120, Space.Self);
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if (Check_for_eyeblink == true)
        {
            Fadein.SetActive(true);
            Blink_eye();
            //Fadeout.SetActive(true);
            //Invoke("Blink_eye", 2f); 
        }

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
                spin.GetComponent<RectTransform>().Rotate(0, 0, 60, Space.Self);
            }
            else if (check_seq == 2)
            {
                spin.GetComponent<RectTransform>().Rotate(0, 0, 120, Space.Self);
            }
            else if (check_seq == 3)
            {
                spin.GetComponent<RectTransform>().Rotate(0, 0, 60, Space.Self);
            }
            else if (check_seq == 4)
            {
                Check_for_eyeblink = true;
            }


            flag = true;
            timer = 0;

        }
        

    }
    void Scene_End()
    {
        nextscene.SetActive(true);
        nowscene.SetActive(false);
    }
    void rotate()
    {
        if(check_seq == 0|| check_seq == 1)
        {
            spin.GetComponent<RectTransform>().Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);

        }
        else if(check_seq == 2 || check_seq == 3)
        {
            spin.GetComponent<RectTransform>().Rotate(0, 0, +Time.deltaTime * rotateSpeed, Space.Self);
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
               // Debug.Log("Check_1");
                check_seq++;
            }

        }
        else
        {
            if ((niddledgreeend[count] + nowdegree) >= niddlerdgreestart[count])
            {
                
                flag = false;
                nowdegree = 0;
               // Debug.Log("Check_2");
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
    void Blink_eye()
    {
        if (Check_for_eyeblink_temp == 0)
        {   //눈 뜬 이미지로 변경
           // Debug.Log("0000");
            eye_open_image_4.SetActive(true);
            eye_open_image_3.SetActive(false);
            Temp_timer = 0f;
            Check_for_eyeblink_temp++;
        }
        Temp_timer += Time.deltaTime;
        
        if (Temp_timer > 4.0f)
        {
            if(Check_for_eyeblink_temp == 1)
            {   //눈 감은 이미지로 변경
               // Debug.Log("1111");
                eye_open_image_4.SetActive(false);
                eye_open_image_3.SetActive(true);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }else if (Check_for_eyeblink_temp == 2)
            {
                
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 3)
            {
                
                Fadeout_2.SetActive(true);
                Invoke("Scene_End", 2f);
               // Debug.Log("2222");
            }
        }
    }
}
