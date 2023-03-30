using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    public GameObject mowScene;
    public GameObject nextScene;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mowScene.SetActive(false);
        if (nextScene != null)
        {
            nextScene.SetActive(true);
        }
    }
    // Start is called before the first frame update
    
}
