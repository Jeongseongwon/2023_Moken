using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_change_3_2 : MonoBehaviour
{
    [Header("Only for 3-2 scene change")]
    // Start is called before the first frame update

    private float temp_timer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        temp_timer += Time.deltaTime;

        if (temp_timer > 9.9f)
        {
            SceneManager.LoadScene("3-2.Bright");
        }
    }
}
