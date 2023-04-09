using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class Change_next_2 : MonoBehaviour
{
    [Header("======== Main and target object  ======== ")]
    public GameObject Main_object;
    public GameObject Target_object;

    [Header("======== Now and next movement  ======== ")]
    public GameObject Now_mov;
    public GameObject Next_mov;

    [Header("======== Mode X = true, Y= false  ======== ")]
    //mode x,y ����?
    public bool mode;
    public bool mode_for_blink = false;

    private float Distance;
    private float Distance_x;
    private float Distance_y;

    public GameObject Audio_narr;
    //================ only for eye image =========================
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;

    private int Check_for_eyeblink_temp = 0;
    private float Temp_timer = 0;

    private bool Check_for_end = false;
    public GameObject plane;
    public GameObject Fadein_2;
    public GameObject Fadeout;
    public GameObject Fadeout_2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (mode == true)
        {
            Distance_x = Main_object.GetComponent<RectTransform>().position.x - Target_object.GetComponent<Transform>().position.x;
            Distance_y = Main_object.GetComponent<RectTransform>().position.y - Target_object.GetComponent<Transform>().position.y;
            Distance = Distance_x+Distance_y;
            Debug.Log(Distance);
        }
        else if (mode == false)
        {
            Distance_y = Main_object.GetComponent<RectTransform>().position.y - Target_object.GetComponent<Transform>().position.y;
        }

        if (Distance_y > -0.5 && Distance_y < 0.5)
        {
            Check_for_end = true;
        }

        if (Check_for_end == true)
        {

            if (mode_for_blink == true)
            {
                Blink_eye();
            }
            else if (mode_for_blink == false)
            {
                Now_mov.SetActive(false);
                Next_mov.SetActive(true);
            }
        }
        else
        {

        }
    }
    void Scene_End()
    {
        //Debug.Log("Scene End");
        // SceneManager.LoadScene("5-2.Earth");
        Now_mov.SetActive(false);
        Next_mov.SetActive(true);
    }
    void Blink_eye()
    {
        if(Temp_timer == 0 && Check_for_eyeblink_temp == 0)
        {
            Fadeout.SetActive(true);
            Main_object.SetActive(false);
        }
        if (Temp_timer > 2f && Check_for_eyeblink_temp == 0)
        {
            Fadeout.SetActive(false);
            Audio_narr.GetComponent<AudioSource>().Play();
            eye_open_image_4.SetActive(true);
            eye_open_image_3.SetActive(false);
            Fadein_2.SetActive(true);
            Temp_timer = 0f;
            Check_for_eyeblink_temp++;
        }
        Temp_timer += Time.deltaTime;
        if (Temp_timer > 4.0f)
        {

            if (Check_for_eyeblink_temp == 2)
            {   //눈 뜬 이미지로 변경
                //Debug.Log("0000");
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if (Check_for_eyeblink_temp == 1)
            {   //눈 감은 이미지로 변경
                //Debug.Log("1111");
                eye_open_image_4.SetActive(false);
                eye_open_image_3.SetActive(true);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 3)
            {
                Fadeout_2.SetActive(true);
                Invoke("Scene_End", 2f);
            }
        }

    }
}
