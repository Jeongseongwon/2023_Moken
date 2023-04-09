using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class eyeblick : MonoBehaviour
{

    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;

    public GameObject nextscene;
    public GameObject nowScene;



    private int num;
    private int Check_for_eyeblink_temp = 0;
    private float Temp_timer = 0;
    private bool Check_for_eyeblink = false;

    public GameObject Fadeout;

    // Start is called before the first frame update
    void Start()
    {
        Check_for_eyeblink = true;

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
        //눈깜빡깜빡 한다음에
        //이미지 아예 없애고 다시 실행하게끔 바꿈
        Temp_timer += Time.deltaTime;


        if (Temp_timer > 4.0f)
        {
            if (Check_for_eyeblink_temp == 1)
            {   //눈 뜬 이미지로 변경
               // Debug.Log("0000");
                eye_open_image_3.SetActive(true);
                eye_open_image_4.SetActive(false);

                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if (Check_for_eyeblink_temp == 0)
            {   //눈 감은 이미지로 변경
                //Debug.Log("1111");
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 2)
            {
                Fadeout.SetActive(true);
                Invoke("Scene_End", 2f);
                //Debug.Log("2222");

            }
        }
    }

    void Scene_End()
    {

        //SceneManager.LoadScene("3-2. Bright");
        nextscene.SetActive(true);
        nowScene.SetActive(false);
    }

}
