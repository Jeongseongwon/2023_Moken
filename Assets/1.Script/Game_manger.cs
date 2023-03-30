using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manger : MonoBehaviour
{
    // Start is called before the first frame update
    public static int Number_chapter;

    
    void Start()
    {
        Number_chapter = 0;
    }

    public int Get_nchapter()
    {
        return Number_chapter;
    }
    public void Go_next_chapter()
    {
        Number_chapter++;
    }
    public void Go_prev_chapter()
    {
        Number_chapter--;
    }

    public void Playing_endeffect()
    {
        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Play();
        }
        
    }
}
