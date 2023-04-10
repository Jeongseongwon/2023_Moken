using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Object_movement : MonoBehaviour
{
    [Header("ONLY FOR TEST")]

    public List<GameObject> Targetposition_obj_list = new List<GameObject>();   //타겟 오브젝트 transform 타입 저장


    /*=================================================
     * Main_object -> 이동시킬 오브젝트
     * Speed_obj-> 오브젝트 이동속도
     * 
     * 이동할 포인트에 오브젝트 순서대로 배치하고
     * 오브젝트를 부모 오브젝트로 묶음
     * 묶은 부모 오브젝트는 Position_seq1,2,3에 각각 저장
     * 
     * =================================================
     */

    [Header("======== System variation  ======== ")]
    public GameObject Main_object;
    public GameObject First_screen;

    public int Number_waypoints;

    public float Speed_obj=1000f;

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

    //============== target position object =======================
    public GameObject Position_seq0;
    public GameObject Position_seq1;
    public GameObject Position_seq2;
    public GameObject Game_manger;

  
    //================ only for eye image =========================
   
    private bool Check_for_eyeblink;
    private int Check_for_eyeblink_temp=0;
    private float Temp_timer = 0;

    public GameObject Audio_bgm;
    public GameObject nextonject;
    public GameObject Fadeout;

    public GameObject Fadeout_first;
    public GameObject Fadein_first;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        //여기에서 오디오 소스 실행
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
        //if (Input.GetKeyDown(KeyCode.A))
        //{
            
        //}

        if (check == true)
        {
            Object_move(Number_waypoints);
        }
        else if (check == false)
        {
            float temp_timer = Game_manger.GetComponent<Timer>().Get_time();
            if (temp_timer > 6f)
            {
                Fadeout_first.SetActive(true);
                
            }
            if (temp_timer > 8f)
            {
                Fadeout_first.SetActive(false);
                Fadein_first.SetActive(true);
                First_screen.SetActive(false);
                Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
                check = true;
            }
        }

        if (Check_for_eyeblink == true)
        {
            //Blink_eye();
            //다음 오브젝트로 활성화
            Invoke("Scene_End", 2f);
            Fadeout.SetActive(true);
        }


    }
    void Scene_start()
    {
        check = false;
        
        Check_for_movement = true;
    }

    void Scene_End()
    {
        nextonject.SetActive(true);
        this.gameObject.SetActive(false);
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
            Position_seq1.SetActive(false);
            Position_seq2.SetActive(false);
            Get_child(Position_seq0);
            //Debug.Log("Chapter_0 START");
            Main_object.GetComponent<Image>().sprite = image_1;
            Speed_obj = 550f;
        }
        else if(Status_chapter == 1)
        {
            //여기에 잠깐 스탑하고 화면 보여주는 함수 호출 하면 좋을 것 같음.
            //Check_for_eyeblink = true;
            //eye_open_image_4.SetActive(true);

            Position_seq0.SetActive(false);
            Position_seq1.SetActive(true);
            Position_seq2.SetActive(false);
            Get_child(Position_seq1);
            //Debug.Log("Chapter_1 START");
            Main_object.GetComponent<Image>().sprite = image_2;
            Speed_obj = 550f;
        }
        else if (Status_chapter == 2)
        {
            //Check_for_eyeblink = true;
           // eye_open_image_4.SetActive(true);

            Position_seq0.SetActive(false);
            Position_seq1.SetActive(false);
            Position_seq2.SetActive(true);
            Get_child(Position_seq2);
            //Debug.Log("Chapter_2 START");
            Main_object.GetComponent<Image>().sprite = image_3;
            Speed_obj = 650f;
        }

        Check_seq_target = 0;
    }
    void Get_child(GameObject Parent_obj)
    {
        Targetposition_obj_list.Clear();
        //Save all target gameobject from parent object in the list
        Only_access_obj = Parent_obj.GetComponent<Transform>();
       

        for (int i = 0; i< Only_access_obj.childCount; i++)
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
                Test_obj_transform.position = Targetposition_obj_list[Check_seq_target+1].GetComponent<RectTransform>().position;
            }
        Flag_1 = false;
    }

    void Object_move(int Numberofchild=0)
    {
        if(Flag_1==true)
        {
            Setting_start_position();
        }
        if(Check_for_movement==true && Numberofchild!=0)
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
        else if(Check_for_movement == false && Numberofchild !=0)
        {
            //End of target, change to next chapter
            if(Check_seq_target == Numberofchild - 1)
            {
                
                Game_manger.GetComponent<Game_manger>().Go_next_chapter();
                Status_chapter = Game_manger.GetComponent<Game_manger>().Get_nchapter();

                if (Status_chapter==3)
                {
                    
                   
                    Audio_bgm.GetComponent<AudioSource>().volume = 0.03f;
                    Check_for_eyeblink = true;
                    check = false; //눈 감은 이미지로 변경
                    
                    Temp_timer = 0f;
                   
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
                
                Flag_1 = true;
            }
        }
            
    }
}
