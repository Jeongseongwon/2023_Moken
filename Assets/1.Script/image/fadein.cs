using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadein : MonoBehaviour
{
   [SerializeField]
[Range(0.1f,10f)]
private float fadeTime;
private Image image;
public GameObject nowimage;
public GameObject nextimage;
[SerializeField]
private AnimationCurve fadeCurve;




private void Awake()
{
    

    image =GetComponent<Image>();
   
   StartCoroutine(FadeInOut());
         
    
    
   

   


   
}

private IEnumerator FadeInOut()
{
    while(true)
    {
        
        yield return StartCoroutine(Fade(0,1));
       
       next();
       break;
        
    }
}
private IEnumerator Fade(float start,float end)
{
    float currentTime =0.0f;
    float percent =0.0f;
    while (percent<1)
    {
        currentTime +=Time.deltaTime;
        percent=currentTime/fadeTime;

        Color color =image.color;
        color.a =Mathf.Lerp(start,end,percent);
        image.color=color;
        yield return null;
        
    }
}
public void next()
{
    nowimage.SetActive(false);
     nextimage.SetActive(true);

}
}