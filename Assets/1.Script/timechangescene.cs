    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timechangescene : MonoBehaviour
{
    public float waitTime;
    public float musicPlaytime;
    float timer;
    float musictimer;
   public AudioSource soundposition;
   public AudioClip playsound;

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>waitTime)
        {
            musictimer+=Time.deltaTime;
            if(musictimer<=musicPlaytime)
            {
                soundposition.PlayOneShot(playsound);

            }
            else
            SceneManager.LoadScene("2-1.Eye_exercise");
        }

        
    }
}
