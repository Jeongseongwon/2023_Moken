using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenechangebytime : MonoBehaviour
{
    public GameObject nowimage;
    public GameObject nextimage;
    public float time;

    private GameObject Audio_bgm;
    // Start is called before the first frame update
    void Start()
    {

        //Application.targetFrameRate = 120;
        Audio_bgm = GameObject.FindGameObjectWithTag("BGM");
        if (this.gameObject.GetComponent<AudioSource>() != null)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
        }
        StartCoroutine(check());
    }
    IEnumerator check()
    {
        yield return new WaitForSeconds(time);
        next();

    }
    public void next()
    {
        Audio_bgm.GetComponent<AudioSource>().volume = 0.1f;
        nowimage.SetActive(false);
        nextimage.SetActive(true);

    }


    // Update is called once per frame
}
