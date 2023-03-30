using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Eyeblink_1 : MonoBehaviour
{
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;
    public GameObject Fadeout;



    private int num;
    private int Check_for_eyeblink_temp = 0;
    private float Temp_timer = 0;
    private bool Check_for_eyeblink = false;

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
        //´«±ôºı±ôºı ÇÑ´ÙÀ½¿¡
        //ÀÌ¹ÌÁö ¾Æ¿¹ ¾ø¾Ö°í ´Ù½Ã ½ÇÇàÇÏ°Ô²û ¹Ù²Ş
        Temp_timer += Time.deltaTime;


        if (Temp_timer > 4.0f)
        {
            if (Check_for_eyeblink_temp == 1)
            {   //´« ¶á ÀÌ¹ÌÁö·Î º¯°æ
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);

                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if (Check_for_eyeblink_temp == 0)
            {   //´« °¨Àº ÀÌ¹ÌÁö·Î º¯°æ
                eye_open_image_3.SetActive(true);
                eye_open_image_4.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 2)
            {
                Fadeout.SetActive(true);
            }
        }
    }

}
