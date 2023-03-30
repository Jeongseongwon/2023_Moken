using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Numbergame : MonoBehaviour
{
    /*=================================================
     * Main_object -> 이동시킬 오브젝트
     * Speed_obj-> 오브젝트 이동속도
     * Target_object -> 버튼 눌렀을 때 도착하는 지점 검사하는 오브젝트
     * Fruit_array -> 과일 묶어놓은 부모 오브젝트
     * 
     * 이동할 포인트에 오브젝트 순서대로 배치하고
     * 오브젝트를 부모 오브젝트로 묶음
     * 묶은 부모 오브젝트는 "Path"태그로 찾아서 Position_seq로 저장
     * 
     * =================================================
     */

    [Header("======== System variation  ======== ")]
    public GameObject Main_object;
    public GameObject Target_object;
    public GameObject Scoreboard;
    public GameObject First_screen;

    public Transform Fruit_array;
    public Sprite[] Sprite_image;
    public int Number_waypoints;
    public GameObject Audio_bgm;

    public float Speed_obj = 100f;

    //=================================================
    private bool check = false;
    private bool Flag_1 = true;


    private RectTransform Test_obj_transform;
    private Transform Only_access_obj;
    private List<GameObject> Targetposition_obj_list = new List<GameObject>();   //타겟 오브젝트 transform 타입 저장

    private int Status_chapter;

    private bool Check_for_movement;
    private bool Check_for_answer;
    private int Check_seq_target;
    private int Check_fruit_path;
    private int Check_fruit;
    private int score;

    private float Distance;

    //============== target position object =======================
    private GameObject[] Position_seq;
    private GameObject Game_manger;

    //============== effect object =======================
    
    private GameObject sound_object;
    private bool Check_for_endsound = false;

    public GameObject nextscene;

    public GameObject effect_1;
    public GameObject effect_2;
    public GameObject effect_3;
    private float button_timer =0f;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
        Position_seq = GameObject.FindGameObjectsWithTag("Path");
        
        sound_object = GameObject.FindGameObjectWithTag("End_sound");

        if (Sprite_image != null)
        {
            Sprite_image = Resources.LoadAll<Sprite>("7.Number");
        }

        Status_chapter = 0;
        Check_for_movement = false;
        Check_seq_target = 0;
        Check_fruit_path = 1;
        Check_fruit = 0;
        score = 0;
        Check_for_answer = false;

        Test_obj_transform = Main_object.GetComponent<RectTransform>();
        Scene_setting();
        Scene_start();

        First_screen.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {


        //첫씬 시작하는거 화면에 보여주기 (5초)
        button_timer += Time.deltaTime;
        if (check == true)
        {
            Object_move(Number_waypoints);
        }
        else if (check == false)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 10f)
            {
                First_screen.SetActive(false);
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
                check = true;
            }
        }

        //if (Input.GetKeyDown(KeyCode.JoystickButton0) && button_timer > 0.7f)
        if (Input.GetKeyDown(KeyCode.Space)&&button_timer>0.7f)
        {
            Distance = Main_object.GetComponent<RectTransform>().position.y - Target_object.GetComponent<RectTransform>().position.y;
            if (Distance < 1f)
            {
                Show_effect();
                Check_for_answer = true;
                Debug.Log("HIT!!");
            }
            else
            {
                Debug.Log(Distance);
            }
            button_timer = 0f;
        }

        if (Check_for_endsound == false && Game_manger.GetComponent<Timer>().Get_time() > 50f)
        {
            Debug.Log("check_sound_effect");
            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            sound_object.GetComponent<AudioSource>().Play();
            Check_for_endsound = true;
        }else if(Check_for_endsound == true && Game_manger.GetComponent<Timer>().Get_time() > 60f)
        {
            Scene_End();
            check = false;
        }

    }
    void Show_effect()
    {
        if (Check_fruit_path == 0)
        {
            effect_1.SetActive(true);
        }
        if (Check_fruit_path == 1)
        {
            effect_2.SetActive(true);
        }
        if (Check_fruit_path == 2)
        {
            effect_3.SetActive(true);
        }
        Game_manger.GetComponent<AudioSource>().Play();
    }
    void Change_animal_mode()
    {
        //sprite 이미지 바꿔줘야하고
        //원래 있던 오브젝트 비활성화 해줘야하고
    }

    void Scene_start()
    {
        //check = true;
        //Debug.Log("Scene start");
        Check_for_movement = true;
    }

    void Scene_End()
    {
        //Debug.Log("Scene End");
        //nextscene.SetActive(true);
        //this.gameObject.SetActive(false);
        SceneManager.LoadScene("Chapter_1");
    }

    void Scene_setting()
    {
        //Set targetposition before start movement
        if (Game_manger != null)
        {
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
            Debug.Log("check status chapter : " + Status_chapter);
        }

        if (Check_for_answer == true)
        {
            //Correct case
            Fruit_array.GetChild(Check_fruit).gameObject.SetActive(true);
            Check_fruit_path = Random.Range(0, 3);
            Check_fruit = Random.Range(0, 6);    //2단계 넘어가면 숫자 바꿔야할 필요 있음
            Check_for_answer = false;
            score++;
            Scoreboard.GetComponent<Text>().text = score.ToString();

        }
        else if (Check_for_answer == false)
        {
            //Incorrect case
        }
        Fruit_array.GetChild(Check_fruit).gameObject.SetActive(false);
        Main_object.GetComponent<Image>().sprite = Sprite_image[Check_fruit];

        //Setting only for fruit path
        Setting_fruit_path(Check_fruit_path);
        Check_seq_target = 0;
    }
    void Setting_fruit_path(int temp_num)
    {
        if (temp_num == 0)
        {
            Position_seq[0].SetActive(true);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(false);
            Get_child(Position_seq[0]);
            Debug.Log("Route_0 START");

        }
        else if (temp_num == 1)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(true);
            Position_seq[2].SetActive(false);
            Get_child(Position_seq[1]);
            Debug.Log("Route_1 START");
        }
        else if (temp_num == 2)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(true);
            Get_child(Position_seq[2]);
            Debug.Log("Route_2 START");
        }
    }
    void Get_child(GameObject Parent_obj)
    {
        Targetposition_obj_list.Clear();
        //Save all target gameobject from parent object in the list
        Only_access_obj = Parent_obj.GetComponent<Transform>();
        Debug.Log("number of child : " + Only_access_obj.childCount);

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
            Debug.Log(Check_seq_target);
            Test_obj_transform.position = Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position;
        }
        Flag_1 = false;
    }

    void Object_move(int Numberofchild = 0)
    {
        if (Flag_1 == true)
        {
            Setting_start_position();
        }
        if (Check_for_movement == true && Numberofchild != 0)
        {
            //그리고 페이드인

            //Move toward target in the list
            Main_object.GetComponent<RectTransform>().position = Vector3.MoveTowards(Test_obj_transform.position,
                Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position, Speed_obj * Time.deltaTime);

            //Check if it arrive to target position or not
            if (Test_obj_transform.position == Targetposition_obj_list[Check_seq_target].GetComponent<RectTransform>().position)
            {
                Check_for_movement = false;
            }
        }
        else if (Check_for_movement == false && Numberofchild != 0)
        {
            //End of target, change to next chapter
            if (Check_seq_target == Numberofchild - 1)
            {
                Debug.Log("Chapter End");
                Game_manger.GetComponent<Game_manger>().Go_next_chapter();
                Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();

                if (Status_chapter == 15)    //2단계로 바뀔 때 뭔가 설정이 더 필요함
                {
                    Scene_End();
                    check = false;
                }
                else
                {
                    //Next chapter
                    Scene_setting();
                    Flag_1 = true;
                    Check_for_movement = true;
                }
            }
            //Next target position
            else
            {
                Check_seq_target++;
                Check_for_movement = true;
                Debug.Log("Change to next target / toward " + Check_seq_target);
                Flag_1 = true;
            }
        }

    }
}

//추가되어야 하는 기능

// 눈깜박임 표시 집어넣기

//연속 이동하려면 어떻게 해야하나?
//이런 기능도 있으면 좋겠는데

