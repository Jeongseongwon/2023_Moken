using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Object_game : MonoBehaviour
{
    /*=================================================
     * Main_object0 -> 이동시킬 오브젝트 1
     * Main_object1 -> 이동시킬 오브젝트 2
     * 
     * 이동할 포인트에 오브젝트 순서대로 배치하고
     * 오브젝트를 부모 오브젝트로 묶음
     * 묶은 부모 오브젝트는 Position_seq1,2,3에 각각 저장
     * 
     * =================================================
     */

    public GameObject Main_object_0;
    public GameObject Main_object_1;
    public GameObject First_screen;

    public GameObject Path_0;
    public GameObject Path_1;
    public GameObject Path_2;
    public GameObject Path_3;
    public GameObject Path_4;

    public GameObject score_board;
    public GameObject prefab;
    public GameObject Fadeout;


    private float Distance;

    private GameObject Game_manger;
    private int Check_chapter;

    private bool check = false;
    private bool Check_for_answer = false;
    private float temp_timer = 0;
    private int Check_score = 0;

    private GameObject sound_object;
    private bool Check_for_endsound = false;
    public GameObject Audio_bgm;

    private float button_timer = 0f;
    // Start is called before the first frame update
    void Start()
    {

        Application.targetFrameRate = 120;
        Game_manger = GameObject.FindGameObjectWithTag("GameController");
        sound_object = GameObject.FindGameObjectWithTag("End_sound");

        Check_chapter = 0;


        Check_for_answer = false;

    }

    // Update is called once per frame
    void Update()
    {
        button_timer += Time.deltaTime;
        if (check == true)
        {
            Game();
            //시간 타이머를 하나 설정을 해주자
        }
        else if (check == false)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 6f)
            {
                //Debug.Log("First screen");
                First_screen.SetActive(false);
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
                Path_0.SetActive(true);
                check = true;
            }
        }

        if (Check_for_endsound == false && Game_manger.GetComponent<Timer>().Get_time() > 46f)
        {
            //Debug.Log("check_sound_effect");
            Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
            sound_object.GetComponent<AudioSource>().Play();
            Check_for_endsound = true;
        }

    }

    void Game()
    {

        temp_timer += Time.deltaTime;

        if (temp_timer > 15f)
        {
            Change_path();
            temp_timer = 0;
        }
        //if (Input.GetKeyDown(KeyCode.JoystickButton0) && button_timer > 1f)
        if (Input.GetKeyDown(KeyCode.Space)&& button_timer >1f)
        {
            Distance = Main_object_0.GetComponent<Transform>().position.x - Main_object_1.GetComponent<Transform>().position.x;//이 값을 조금 바꿔야겟다
            if (Distance < 10 && Distance > -10)
            {
                Check_for_answer = true;
                Check_score++;
                score_board.GetComponent<Text>().text = string.Format("{0:N0}", Check_score);
                Show_effect();
                button_timer = 0f;
            }
            else
            {

            }
        }
    }
    void Show_effect()
    {

        Transform tem_transform = Main_object_0.GetComponent<Transform>();

        prefab.GetComponent<Transform>().localPosition = new Vector3(tem_transform.localPosition.x + 30, tem_transform.localPosition.y, tem_transform.localPosition.z - 30);
        prefab.SetActive(true);
        this.gameObject.GetComponent<AudioSource>().Play();
    }
    void Change_path()
    {
        Game_manger.GetComponent<Game_manger>().Go_next_chapter();
        Check_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();

        //이전 오브젝트들 비활성화 및 이번에 오브젝트들 활성화
        if (Check_chapter == 1)
        {
            Main_object_0 = Path_1.GetComponent<Transform>().GetChild(0).gameObject;
            Main_object_1 = Path_1.GetComponent<Transform>().GetChild(1).gameObject;
            Path_0.SetActive(false);
            Path_1.SetActive(true);
            Path_2.SetActive(false);
        }
        else if (Check_chapter == 2)
        {
            Main_object_0 = Path_2.GetComponent<Transform>().GetChild(0).gameObject;
            Main_object_1 = Path_2.GetComponent<Transform>().GetChild(1).gameObject;
            Path_0.SetActive(false);
            Path_1.SetActive(false);
            Path_2.SetActive(true);

        }
        else if (Check_chapter == 3)
        {
            Main_object_0 = Path_3.GetComponent<Transform>().GetChild(0).gameObject;
            Main_object_1 = Path_3.GetComponent<Transform>().GetChild(1).gameObject;
            Path_0.SetActive(false);
            Path_3.SetActive(true);
            Path_2.SetActive(false);
        }
        else if (Check_chapter == 4)
        {
            Fadeout.SetActive(true);
            Invoke("Scene_End", 2f);
            //Main_object_0 = Path_4.GetComponent<Transform>().GetChild(0).gameObject;
            //Main_object_1 = Path_4.GetComponent<Transform>().GetChild(1).gameObject;
            //Path_4.SetActive(true);
            //Path_3.SetActive(false);
            //Path_2.SetActive(false);

        }
        else if (Check_chapter == 5)
        {
            Fadeout.SetActive(true);
            Invoke("Scene_End", 2f);
        }


    }

    void Scene_End()
    {
        SceneManager.LoadScene("Chapter_3");
    }
}
