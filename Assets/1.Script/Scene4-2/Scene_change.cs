using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_change : MonoBehaviour
{
    [Header("Only for 4-2 scene change")]
    // Start is called before the first frame update
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;
    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp = 0;

    public GameObject Fadeout;
    public GameObject Fadein;

    private float Temp_timer;

    private float temp_timer = 0f;

    public GameObject nowscene;
    public GameObject nextscene;
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
            Fadein.SetActive(true);
        }


    }
    void Scene_End()
    {
        nextscene.SetActive(true);
        nowscene.SetActive(false);
    }
    void Blink_eye()
    {

        Temp_timer += Time.deltaTime;

        if (Temp_timer > 4.0f)
        {
            if (Check_for_eyeblink_temp == 0)
            {   //눈 뜬 이미지로 변경
                Debug.Log("0000");
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if (Check_for_eyeblink_temp == 1)
            {   //눈 감은 이미지로 변경
                Debug.Log("1111");
                eye_open_image_4.SetActive(false);
                eye_open_image_3.SetActive(true);
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
}
