using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imagestart : MonoBehaviour
{
    public GameObject image;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(check());
    }
    IEnumerator check()
    {
        yield return new WaitForSeconds(time);
        image.SetActive(true);
    }

    // Update is called once per frame
    
}
