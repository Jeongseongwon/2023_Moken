using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fruit_game : MonoBehaviour
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

    public float Speed_obj = 5f;

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
    private int Check_fruit;
    private int score;

    private float Distance;

    //============== target position object =======================
    public GameObject[] Position_seq;
    public GameObject Game_manger;

    //============== effect object =======================
    public GameObject[] Effect_position;
    private bool Check_for_endsound = false;
    public GameObject Audio_end;
    public GameObject Audio_bgm;
    public GameObject Audio_narr;

    private float button_timer = 0f;
    public GameObject Fadein;
    public GameObject Fadeout_1;
    public GameObject Fadeout_2;

    // Start is called before the first frame update
    void Start()
    {
        
        if (Sprite_image != null)
        {
            Sprite_image = Resources.LoadAll<Sprite>("6. Fruit");
        }

        Status_chapter = 0;
        Check_for_movement = false;
        Check_seq_target = 0;
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
        button_timer += Time.deltaTime;

        //첫씬 시작하는거 화면에 보여주기 (5초)

        if (check == true)
        {
            Object_move(Number_waypoints);
        }
        else if(check ==false)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 8f)
            {
                Fadeout_1.SetActive(true);
                
            }
            if (temp_timer > 10f)
            {
                First_screen.SetActive(false);
                check = true;
                Fadein.SetActive(true);
            }
        }

        //if (Input.GetKeyDown(KeyCode.JoystickButton0)&& button_timer>1f)
        if(Input.GetKeyDown(KeyCode.Space)&& button_timer > 1f)
        {
           
            Distance =Main_object.GetComponent<RectTransform>().position.y - Target_object.GetComponent<RectTransform>().position.y;
            if (Distance < 0.7f)
            {
                Show_effect();
            
                Check_for_answer = true;
                //Debug.Log("HIT!!");
                
            }
            else
            {
               // Debug.Log(Distance);
            }
            button_timer = 0f;
        }

        if (Check_for_endsound == false && Game_manger.GetComponent<Timer>().Get_time() > 50f)
        {
            //Debug.Log("check_sound_effect");

            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            Audio_narr.GetComponent<AudioSource>().Play();
            Audio_end.GetComponent<AudioSource>().Play();
            Check_for_endsound = true;
        }

    }

    void Show_effect()
    {
        if (Check_fruit == 0)
        {
            Effect_position[0].SetActive(true);
        }
        if (Check_fruit == 1)
        {
            Effect_position[1].SetActive(true);
        }
        if (Check_fruit == 2)
        {
            Effect_position[2].SetActive(true);
        }
        if (Check_fruit == 3)
        {
            Effect_position[3].SetActive(true);
        }
        if (Check_fruit == 4)
        {
            Effect_position[4].SetActive(true);
        }
        if (Check_fruit == 5)
        {
            Effect_position[5].SetActive(true);
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
        Debug.Log("Scene start");
        Check_for_movement = true;
    }

    void Scene_End()
    {
       //Debug.Log("Scene End");
        SceneManager.LoadScene("Chapter_4");
    }

    void Scene_setting()
    {
        //Set targetposition before start movement
        if (Game_manger != null)
        {
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
           // Debug.Log("check status chapter : " + Status_chapter);
        }

        if(Check_for_answer==true)
        {
            //Correct case
            Fruit_array.GetChild(Check_fruit).gameObject.SetActive(true);
            Check_fruit= Random.Range(0, 6);    //2단계 넘어가면 숫자 바꿔야할 필요 있음
            Check_for_answer = false;
            score++;
            Scoreboard.GetComponent<Text>().text = score.ToString();

        }
        else if (Check_for_answer==false)
        {
            //Incorrect case
        }
        Fruit_array.GetChild(Check_fruit).gameObject.SetActive(false);
        Main_object.GetComponent<Image>().sprite = Sprite_image[Check_fruit];

        //Setting only for fruit path
        Setting_fruit_path(Check_fruit);
        Check_seq_target = 0;
    }
    void Setting_fruit_path(int temp_num)
    {
        if (temp_num == 0)
        {
            //딸기
            Position_seq[0].SetActive(true);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(false);
            Position_seq[3].SetActive(false);
            Position_seq[4].SetActive(false);
            Position_seq[5].SetActive(false);
            Get_child(Position_seq[0]);
            Debug.Log("Route_0 START");

        }
        else if (temp_num == 1)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(true);
            Position_seq[2].SetActive(false);
            Position_seq[3].SetActive(false);
            Position_seq[4].SetActive(false);
            Position_seq[5].SetActive(false);
            Get_child(Position_seq[1]);
            Debug.Log("Route_1 START");
        }
        else if (temp_num == 2)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(true);
            Position_seq[3].SetActive(false);
            Position_seq[4].SetActive(false);
            Position_seq[5].SetActive(false);
            Get_child(Position_seq[2]);
            Debug.Log("Route_2 START");
        }
        else if (temp_num == 3)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(false);
            Position_seq[3].SetActive(true);
            Position_seq[4].SetActive(false);
            Position_seq[5].SetActive(false);
            Get_child(Position_seq[3]);
            Debug.Log("Route_3 START");
        }
        else if (temp_num == 4)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(false);
            Position_seq[3].SetActive(false);
            Position_seq[4].SetActive(true);
            Position_seq[5].SetActive(false);
            Get_child(Position_seq[4]);
            Debug.Log("Route_4 START");
        }
        else if (temp_num == 5)
        {
            Position_seq[0].SetActive(false);
            Position_seq[1].SetActive(false);
            Position_seq[2].SetActive(false);
            Position_seq[3].SetActive(false);
            Position_seq[4].SetActive(false);
            Position_seq[5].SetActive(true);
            Get_child(Position_seq[5]);
            Debug.Log("Route_5 START");
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

                if (Game_manger.GetComponent<Timer>().Get_time() > 60f)
                {
                    //check if the timer is over or not
                    Fadeout_2.SetActive(true);
                    Invoke("Scene_End", 2f);
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


