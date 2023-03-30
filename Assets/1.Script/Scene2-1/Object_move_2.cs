using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Object_move_2 : MonoBehaviour
{
    [Header("ONLY FOR TEST")]

    public List<GameObject> Targetposition_obj_list = new List<GameObject>();   //Ÿ�� ������Ʈ transform Ÿ�� ����


    /*=================================================
     * Main_object -> �̵���ų ������Ʈ
     * Speed_obj-> ������Ʈ �̵��ӵ�
     * 
     * �̵��� ����Ʈ�� ������Ʈ ������� ��ġ�ϰ�
     * ������Ʈ�� �θ� ������Ʈ�� ����
     * ���� �θ� ������Ʈ�� Position_seq1,2,3�� ���� ����
     * 
     * =================================================
     */

    [Header("======== System variation  ======== ")]
    public GameObject Main_object;
    public GameObject First_screen;

    public int Number_waypoints;

    public float Speed_obj = 1000f;

    public Sprite image_1;
    public Sprite image_2;
    public Sprite image_3;
    //=================================================
    private bool check = false;
    private bool Flag_1 = true;


    private RectTransform Test_obj_transform;
    private Transform Only_access_obj;

    private int Status_chapter;

    private bool Check_for_movement;
    private int Check_seq_target;

    private float stop_timer = 0f;
    private bool Check_for_stop = false;
    private float Check_timer = 0f;
    private bool Check_for_stop_2 = false;
    private bool tem_check = false;

    //============== target position object =======================
    public GameObject Position_seq0;
    public GameObject Position_seq1;
    public GameObject Position_seq2;
    private GameObject Game_manger;


    //================ only for eye image =========================
    public GameObject eye_open_image_1;
    public GameObject eye_open_image_2;
    public GameObject eye_open_image_3;
    public GameObject eye_open_image_4;

    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp = 0;
    private float Temp_timer = 0;

    //================ only for fade =========================
    private float fadeTime = 1f;

    private GameObject Audio_bgm;
    public GameObject nextscene;
    public GameObject nowscene;

    public GameObject Fadeout;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
        Audio_bgm = GameObject.FindGameObjectWithTag("BGM");
        Status_chapter = 0;
        Check_for_movement = false;
        Check_seq_target = 0;
        Check_for_eyeblink = false;
        Test_obj_transform = Main_object.GetComponent<RectTransform>();
        Scene_setting();
        Scene_start();
        First_screen.GetComponent<AudioSource>().Play();
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("check_active");

        if (check == true)
        {
            Object_move(Number_waypoints);
        }
        else if (check == false)
        {

            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 8f)
            {
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
                First_screen.SetActive(false);
                check = true;
                //Debug.Log("First screen off");
            }

        }


        if (Check_for_eyeblink == true)
        {
            Blink_eye();
        }

        if (Check_for_stop == true)
        {
            Only_for_timer_stop(2f);
        }

    }
    void Scene_start()
    {
        check = false;
        //Debug.Log("Scene start");
        Check_for_movement = true;
    }

    void Scene_End()
    {
        //Debug.Log("Scene End");
        //SceneManager.LoadScene("2-2.Image");
        nowscene.SetActive(false);
        nextscene.SetActive(true);
    }

    void Scene_setting()
    {
        //Debug.Log("Chapter End");
        Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
        //Set targetposition before start movement
        if (Game_manger != null)
        {
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
            //Debug.Log("check status chapter : " + Status_chapter);
        }
        if (Status_chapter == 0)
        {
            Position_seq0.SetActive(true);
            Position_seq1.SetActive(false);
            Position_seq2.SetActive(false);
            Get_child(Position_seq0);
            Debug.Log("Chapter_0 START");
            Main_object.GetComponent<Image>().sprite = image_1;
            Speed_obj = 750f;
            Main_object.GetComponent<RectTransform>().position = Targetposition_obj_list[0].GetComponent<RectTransform>().position;
        }
        else if (Status_chapter == 1)
        {
            
            Position_seq0.SetActive(false);
            Position_seq1.SetActive(true);
            Position_seq2.SetActive(false);
            Get_child(Position_seq1);
            Debug.Log("Chapter_1 START");
            Main_object.GetComponent<Image>().sprite = image_2;
            Speed_obj = 750f;
            Main_object.GetComponent<RectTransform>().position = Targetposition_obj_list[0].GetComponent<RectTransform>().position;
        }
        else if (Status_chapter == 2)
        {
            
            Position_seq0.SetActive(false);
            Position_seq1.SetActive(false);
            Position_seq2.SetActive(true);
            Get_child(Position_seq2);
            Debug.Log("Chapter_2 START");
            Main_object.GetComponent<Image>().sprite = image_3;
            Speed_obj = 750f;
            Main_object.GetComponent<RectTransform>().position = Targetposition_obj_list[0].GetComponent<RectTransform>().position;
        }
        else if (Status_chapter == 3)
        {
            //Scene_End();
            eye_open_image_4.SetActive(true);
            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            Check_for_eyeblink = true;
            check = false;
        }
        Check_seq_target = 0;
        Game_manger.GetComponent<Game_manger>().Go_next_chapter();
        tem_check = false;
    }
    void Get_child(GameObject Parent_obj)
    {
        Targetposition_obj_list.Clear();
        //Save all target gameobject from parent object in the list
        Only_access_obj = Parent_obj.GetComponent<Transform>();
        //Debug.Log("number of child : " + Only_access_obj.childCount);

        for (int i = 0; i < Only_access_obj.childCount; i++)
        {
            Targetposition_obj_list.Add(Only_access_obj.GetChild(i).gameObject);    //Save the each child object in the list
            Only_access_obj.GetChild(i).gameObject.SetActive(false);                //Image,text disable
        }
        Number_waypoints = Only_access_obj.childCount;
    }
    void Setting_start_position()
    {
        if (Check_seq_target % 2 == 0)
        {
            //Debug.Log(Check_seq_target);
            Test_obj_transform.position = Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position;
            Check_seq_target++;
        }
        Flag_1 = false;
    }

    void Only_for_timer_stop(float time)
    {
        //fade in, timer
        stop_timer += Time.deltaTime;
        
        if (stop_timer > time)
        {
            //first screen timer
            Check_for_stop = false;
            //Debug.Log("timer 3f");
            stop_timer = 0;
            Color color = Main_object.GetComponent<Image>().color;
            color.a = 1;
            Main_object.GetComponent<Image>().color = color;
            if (Flag_1 == true)
            {
                Setting_start_position();
            }
        }
    }
    void Object_move(int Numberofchild = 0)
    {
        //Debug.Log("object move");
        if (Check_for_movement == true && Numberofchild != 0)
        {
            //Move toward target in the list
            if (Check_for_stop == false)
            {
                Main_object.GetComponent<RectTransform>().position = Vector3.MoveTowards(Test_obj_transform.position,
                    Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position, Speed_obj * Time.deltaTime);
            }
            else if (Check_for_stop == true)
            {

            }

            //Check if it arrive to target position or not
            if (Test_obj_transform.position == Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position)
            {
                Check_for_movement = false;
            }
        }
        else if (Check_for_movement == false && Numberofchild != 0)
        {
            if (Check_seq_target < Numberofchild - 1)
            {
                Check_for_stop = true;
                Check_seq_target++;
                Check_for_movement = true;
                //Debug.Log("Change to next target / toward " + Check_seq_target);
                Flag_1 = true;


            }
            else if(Check_seq_target == Numberofchild - 1)
            {
                
                if (Status_chapter >= 3)
                {

                }
                else
                {
                    if (tem_check == false)
                    {
                        Invoke("Scene_setting", 1.5f);
                        Check_for_movement = true; 
                        tem_check = true;
                    }
                }
            }
        }


    }
    private void Only_for_fade()
    {
        StartCoroutine(Fade(1, 0));
    }
    private IEnumerator Fade(float start, float end)
    {
        //yield return StartCoroutine(Fade(1, 0));
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = Main_object.GetComponent<Image>().color;
            color.a = Mathf.Lerp(start, end, percent);
            Main_object.GetComponent<Image>().color = color;
            yield return null;
        }
    }
    void Blink_eye()
    {
        check = false;
        Temp_timer += Time.deltaTime;
        if (Temp_timer > 4.0f)
        {
            if (Check_for_eyeblink_temp==1)
            {   //눈 뜬 이미지로 변경
                //Debug.Log("1111");
                eye_open_image_2.SetActive(true);
                eye_open_image_3.SetActive(false);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;
            }
            else if(Check_for_eyeblink_temp == 0)
            {   //눈 감은 이미지로 변경
                //Debug.Log("0000");
                eye_open_image_4.SetActive(false);
                eye_open_image_3.SetActive(true);
                Temp_timer = 0f;
                Check_for_eyeblink_temp++;

            }
            else if (Check_for_eyeblink_temp == 2)
            {
                //Scene_End();
                Fadeout.SetActive(true);
            }
        }

    }
}

