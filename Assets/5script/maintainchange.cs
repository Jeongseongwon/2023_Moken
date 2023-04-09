using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintainchange : MonoBehaviour
{
    public GameObject now;
      public GameObject image;
    public float time;
    public GameObject Fadeout;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(check());
    }
    IEnumerator check()
    {
        
        if (Fadeout != null)
        {
            yield return new WaitForSeconds(time - 1f);
            Fadeout.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        image.SetActive(true);
        now.SetActive(false);
    }

    // Update is called once per frame
    
}
