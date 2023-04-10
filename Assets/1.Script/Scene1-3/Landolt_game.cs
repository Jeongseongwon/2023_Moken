using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Landolt_game : MonoBehaviour
{
    [Header("ONLY FOR TEST")]

    public GameObject Main_object;
    public GameObject First_screen;

    public List<GameObject> Targetposition_obj_list = new List<GameObject>();   //타겟 오브젝트 transform 타입 저장

    [Header("======== Source image ======== ")]
    public Sprite[] Sprite_image;
    public GameObject Game_manger;

    [Header("======== System variation ======== ")]
    public float Limit_time = 4;

    public float Speed_obj = 350f;
    public GameObject Position_seq0;
    public GameObject prefab;

    public GameObject score_board;
    public GameObject Fadeout;


    private int Check_score = 0;

    public GameObject Audio_End;
    private RectTransform Test_obj_transform;
    private Transform Only_access_obj;
    private int Number_rand_formove;

    private bool check;
    private int Status_chapter;
    private float Temp_timer;
    private bool Check_for_movement;
    private int Check_seq_target;
    private bool Check_for_endsound=false;

    private int Key_input;
    private int Number_rand;
    private int Number_waypoints;

    private float button_timer=0f;

    public GameObject Audio_bgm;
    public GameObject Audio_narr;
    // Start is called before the first frame update
    void Start()
    {

        Sprite_image = Resources.LoadAll<Sprite>("3.Landolt");

        Test_obj_transform = Main_object.GetComponent<RectTransform>();

        Get_child(Position_seq0);

        Check_for_movement = false;
        Temp_timer = Limit_time;
        Key_input = -1;
        Number_rand_formove = -1;
        Scene_start();

        First_screen.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        Temp_timer -= Time.deltaTime;
        button_timer += Time.deltaTime;
        
       
        if (check == true)
        {
            Check_input();
            Object_move(Number_waypoints);
            Check_time_for_active();
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

        if(Check_for_endsound == false && Game_manger.GetComponent<Timer>().Get_time() > 45f)
        {
            //Debug.Log("check_sound_effect");
            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            Audio_narr.GetComponent<AudioSource>().Play();
            Audio_End.GetComponent<AudioSource>().Play();
            Check_for_endsound = true;
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
        SceneManager.LoadScene("Chapter_2");
    }

    void Change_image()
    {
        if (Game_manger != null)
        {
            Game_manger.GetComponent<Game_manger>().Go_next_chapter();
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
            //Debug.Log("check status chapter : " + Status_chapter);
        }
        // Debug.Log("image chang called and Number is"+ Status_chapter);
        //Debug.Log("Image changedd");

        Number_rand = Random.Range(0, 4); //난수 생성
        Main_object.GetComponent<Image>().sprite = Sprite_image[Number_rand];
    }

    void Check_input()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Key_input = 2;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Key_input = 3;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Key_input = 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Key_input = 0;
        }
        if (Key_input == Number_rand && button_timer > 1f)
        {
            button_timer = 0f;
            //정답처리
            Check_score++;
            score_board.GetComponent<Text>().text = string.Format("{0:N0}", Check_score);
            Main_object.GetComponent<Image>().enabled = false;
            Show_answer_effect();
            Key_input = -1;
            Number_rand = -2;
            //그 이후로 키입력이랑 한동안 못 받게 해야하는거 같은데?
            //Debug.Log("ANSWER IS RIGHT");
        }
    }

    void Show_answer_effect()
    {
        prefab.SetActive(true);
        this.gameObject.GetComponent<AudioSource>().Play();
    }
    void Check_time_for_active()
    {
        if (Temp_timer < 0.1)
        {
            //Time of each image = 4s
            //if it is inactive, make it active
            
            Main_object.GetComponent<Image>().enabled = true;
            Change_image();
            //여기다가 경로 변경
            Check_for_movement = false;

            Reset_timer();
        }
    }

    void Reset_timer()
    {
        Temp_timer = Limit_time;
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

    void Object_move(int Numberofchild = 0)
    {
        if (Check_for_movement == true && Numberofchild != 0)
        {
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
            //Debug.Log("Chapter End");

            if (Game_manger.GetComponent<Timer>().Get_time() > 60f)
            {
                //check if the timer is over or not
                Fadeout.SetActive(true);
                Invoke("Scene_End", 2f);
                check = false;
            }
            else
            {
                //Next target position
                Check_seq_target = Random.Range(0, Only_access_obj.childCount);
                Check_for_movement = true;
                //Debug.Log("Change to next target / toward " + Check_seq_target);
            }

        }

    }
}
