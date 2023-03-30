using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Football_game : MonoBehaviour
{
    [Header("ONLY FOR TEST")]

    public List<GameObject> Targetposition_obj_list = new List<GameObject>();   //Ÿ�� ������Ʈ transform Ÿ�� ����


    /*=================================================
     * Main_object -> �̵���ų ������Ʈ
     * Speed_obj-> ������Ʈ �̵��ӵ�
     * 
     * Football_object -> �౸�� ������Ʈ
     * Football_target_object -> �౸�� ������Ʈ�� �̵��� Ÿ�� ������Ʈ
     * 
     * 
     * =================================================
     */

    [Header("======== System variation  ======== ")]
    public GameObject Main_object;
    public GameObject Football_object;
    public GameObject Football_target_object;
    public GameObject Goalpost_object;
    public GameObject First_screen;
    public GameObject Goal_text;
     public GameObject Audio_bgm;

    public int Number_waypoints;

    public float Speed_obj;
    public float Goal_time = 1f;

    //=================================================
    private bool check = false;


    private RectTransform Test_obj_transform;
    private RectTransform Football_obj_transform;
    private Transform Only_access_obj;

    private int Status_chapter;

    private bool Check_for_movement;
    private int Check_seq_target;

    private bool check_shot = false;
    private bool check_goal = false;

    private bool check_firstscreen = true;

    public GameObject score_board;
    private int Check_score = 0;


    //============== target position object =======================
    public GameObject Position_seq0;
    private GameObject Game_manger;

    //============== effect object =======================
    private GameObject[] Effect_position;

    private GameObject sound_object;
    private bool Check_for_endsound = false;
    private float button_timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
        Effect_position = GameObject.FindGameObjectsWithTag("Effect");
        sound_object = GameObject.FindGameObjectWithTag("End_sound");

        Status_chapter = 0;
        Check_for_movement = false;
        Check_seq_target = 0;

        Test_obj_transform = Main_object.GetComponent<RectTransform>();
        Football_obj_transform = Football_object.GetComponent<RectTransform>();
        Debug.Log(Football_obj_transform.position);
        Scene_setting();
        Scene_start();
    }

    // Update is called once per frame
    void Update()
    {
        button_timer += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.JoystickButton0) && button_timer > 1f)
        if (Input.GetKeyDown(KeyCode.Space)&& button_timer >1f)
        {
            check_shot = true;
            button_timer = 0f;
        }

        if (check_shot == true)
        {
            Shot_ball();
        }
        else if (check_shot == false)
        {

        }

        if (check_firstscreen == true)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 8f)
            {
                First_screen.SetActive(false);
                check = true;
                check_firstscreen = false;
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
            }
        }

        if (check == true)
        {
            Object_move(Number_waypoints);

        }
        else if (check == false)
        {

        }

        if (check_goal == true)
        {
            Show_goal();

        }
        else
        {

        }

        if (Check_for_endsound == false && Game_manger.GetComponent<Timer>().Get_time() > 50f)
        {
            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            sound_object.GetComponent<AudioSource>().Play();
            Check_for_endsound = true;
        }
    }
    void Scene_start()
    {
        Check_for_movement = true;
    }

    void Scene_End()
    {
        SceneManager.LoadScene("Chapter_5");
    }

    void Scene_setting()
    {
        //Set targetposition before start movement
        if (Game_manger != null)
        {
            Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();
        }
        if (Status_chapter == 0)
        {
            Position_seq0.SetActive(true);
            Get_child(Position_seq0);

            Speed_obj = 3f;
        }
        Check_seq_target = 0;
    }
    void Get_child(GameObject Parent_obj)
    {
        Targetposition_obj_list.Clear();
        //Save all target gameobject from parent object in the list
        Only_access_obj = Parent_obj.GetComponent<Transform>();

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
            if (Check_seq_target == Numberofchild - 1)
            {
                //Debug.Log("Chapter End");
                Game_manger.GetComponent<Game_manger>().Go_next_chapter();
                Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();

                if (Game_manger.GetComponent<Timer>().Get_time() > 60f)
                {
                    Scene_End();
                    check = false;
                }
                else
                {
                    //Next chapter
                    Scene_setting();
                    Check_for_movement = true;
                }
            }
            //Next target position
            else
            {
                if (Check_seq_target == 0)
                    Check_seq_target = 1;
                else if (Check_seq_target == 1)
                    Check_seq_target = 0;

                Check_for_movement = true;
                //Debug.Log("Change to next target / toward " + Check_seq_target);
            }
        }

    }

    void Shot_ball()
    {
        Football_object.GetComponent<RectTransform>().position = Vector3.MoveTowards(Football_object.GetComponent<RectTransform>().position,
               Football_target_object.GetComponent<RectTransform>().position, 6f * Time.deltaTime);   

        Check_goal();

        if (Football_object.GetComponent<RectTransform>().position == Football_target_object.GetComponent<RectTransform>().position)
        {
            
            Football_object.GetComponent<RectTransform>().position = new Vector3(-0.1f, -2.3f, 89.8f);
            check_shot = false;
        }
    }

    void Check_goal()
    {
        if (Football_object.GetComponent<RectTransform>().position.x >= (Goalpost_object.GetComponent<RectTransform>().position.x - 3) &&
            Football_object.GetComponent<RectTransform>().position.x <= (Goalpost_object.GetComponent<RectTransform>().position.x + 3))
        {
            if (Football_object.GetComponent<RectTransform>().position.y >= Goalpost_object.GetComponent<RectTransform>().position.y &&
            Football_object.GetComponent<RectTransform>().position.y <= (Goalpost_object.GetComponent<RectTransform>().position.y + 10))
            {
                Check_score++;
                score_board.GetComponent<Text>().text = string.Format("{0:N0}", Check_score);
                Show_effect();
                check_shot = false;
                check_goal = true;
                Goal_text.SetActive(true);
                check = false;
            }
        }

    }

    void Show_goal()
    {
        Goal_time -= Time.deltaTime;
        if (Goal_time < 0f)
        {
            //Debug.Log("position change_show goal");
            Football_object.GetComponent<RectTransform>().position = new Vector3(-0.1f, -2.3f, 89.8f);

            Goal_text.SetActive(false);
            check = true;
            check_goal = false;
            Goal_time = 1f;
        }
    }

    void Show_effect()
    {
        Effect_position[0].SetActive(true);
        Game_manger.GetComponent<AudioSource>().Play();
    }
}
