using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Image_change : MonoBehaviour
{
    [Header("ONLY FOR TEST")]
    public GameObject temp_timer;
    public GameObject First_screen;


    [Header("======== Source image ======== ")]
    public Sprite[] Sprite_image;


    [Header("======== System variation ======== ")]


    private GameObject Image_object;
    private GameObject fader;
    private GameObject Game_manger;
    public GameObject eye_open_image_1;
    public GameObject eye_open_image_2;
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;
    public GameObject Basecanvas;
    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp=0;
  


    private float Temp_timer;
    private bool check;
    private int Status_chapter;
    private bool Check_for_fade;

    private GameObject Audio_bgm;
    public GameObject nextscene;
    public GameObject nowscene;
    public GameObject Fadeout;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        
        Sprite_image = Resources.LoadAll<Sprite>("2. Background");
        Audio_bgm = GameObject.FindGameObjectWithTag("BGM");
        Image_object = GameObject.Find("Main_image");
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
        fader = GameObject.Find("Fade");

        Temp_timer = 9f;
        Status_chapter = 0;
        Check_for_fade = true;
        check = false;
        Scene_start();

        //눈깜빡임
        
        Check_for_eyeblink = false;

        First_screen.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        //temp_timer.GetComponent<Text>().text = string.Format("Timer :"+"{0:N1}", Temp_timer);
        
        if (check == true)
        {
            Temp_timer -= Time.deltaTime;
               
               
            if (Temp_timer < 0.1)
            {
                if (Status_chapter == Sprite_image.Length-1)
                {
                    //End of image sequence
                    Check_for_eyeblink=true;
                }
                Reset_timer();

                Check_for_fade = true;
            }

            if (Check_for_fade == true)
            {

                Fade_out();
                Change_image();
                Invoke("Fade_in", 2.5f);

                Check_for_fade = false;
            }
        }
        else if (check == false)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 8f)
            {
                First_screen.SetActive(false);
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
                check = true;
            }
        }
        if (Check_for_eyeblink == true)
        {
            Basecanvas.SetActive(false);
            Blink_eye();
        }

    }
    void Scene_start()
    {
        check = false;
        //Debug.Log("Scene start");
    }

    void Scene_End()
    {
        //Debug.Log("Scene End");
        check = false;
        Check_for_eyeblink=true;
        //SceneManager.LoadScene("1-3.Landolt");
        nextscene.SetActive(true);
        nowscene.SetActive(false);


    }
    void Reset_timer()
    {
        Temp_timer = 9f;
    }

    void Change_image()
    { 
        //for fade in, out at change image
        if (Game_manger != null)
        {
            Game_manger.GetComponent<Game_manger>().Go_next_chapter();
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
            //Debug.Log("check status chapter : " + Status_chapter);
        }
        
        if (Status_chapter != Sprite_image.Length)
        {
            //Debug.Log("image change called and Number is" + Status_chapter);
            Image_object.GetComponent<Image>().sprite = Sprite_image[Status_chapter];
        }
        

    }
    void Fade_in()
    {
        //Debug.Log("fade in");
        fader.GetComponent<Fader>().FadeIn(2.5f);
    }

    void Fade_out()
    {
        //Debug.Log("fade out");
        fader.GetComponent<Fader>().JustBlack();
        //fader.GetComponent<Fader>().FadeOut(3.0f);
    }
    void Blink_eye()
    {
        check = false;
        Temp_timer += Time.deltaTime;
        
        if (Temp_timer > 4.0f)
        {
            if (Check_for_eyeblink_temp==0)
            {   //눈 뜬 이미지로 변경
                
                eye_open_image_4.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if(Check_for_eyeblink_temp == 1)
            {   //눈 감은 이미지로 변경
                
                eye_open_image_4.SetActive(false);
                eye_open_image_3.SetActive(true);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }else if (Check_for_eyeblink_temp == 2)
            {
                
                eye_open_image_2.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 3)
            {
                Fadeout.SetActive(true);
            }
        }
    }
}
