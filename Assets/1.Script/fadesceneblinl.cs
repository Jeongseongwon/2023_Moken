using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class fadesceneblinl : MonoBehaviour
{
    //eye
    public GameObject eye_open_image_2;
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;

    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp = 0;
    private float Temp_timer = 0;

    private GameObject Audio_bgm;
    public GameObject nowscene;
    public GameObject nextscene;
    public GameObject Fadeout;

    // Start is called before the first frame update
    void Start()
    {

        Check_for_eyeblink = true;
        Check_for_eyeblink_temp = 0;
        Blink_eye();
    }

    // Update is called once per frame
    void Update()
    {
        if (Check_for_eyeblink == true)
        {
            Blink_eye();
        }
    }
    void Blink_eye()
    {

        Temp_timer += Time.deltaTime;
        if (Temp_timer > 5.0f)
        {
            if (Check_for_eyeblink_temp == 0)
            {   //눈 감은 이미지로 변경
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if (Check_for_eyeblink_temp == 1)
            {   //눈 뜬 이미지로 변경
                eye_open_image_4.SetActive(false);
                eye_open_image_2.SetActive(true);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 2)
            {
                Fadeout.SetActive(true);
               // Invoke("Scene_End", 2f);
            }
        }
        void Scene_End()
        {
            nowscene.SetActive(false);
            nextscene.SetActive(true);
        }

    }
}
