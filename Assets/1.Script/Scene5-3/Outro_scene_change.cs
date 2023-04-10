using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro_scene_change : MonoBehaviour
{
    public float time;
    public GameObject Fadeout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = this.GetComponent<Timer>().Get_time();

        if(time > 98f)
        {
            Fadeout.SetActive(true);
        }
        if (time > 100f)
        {
            SceneManager.LoadScene("Chapter_1");
        }
    }
}
