using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image image;
    public bool DoFadeIn = true;
    private void Awake()
    {
        image = GetComponent<Image>();

        //ȭ���� ���� �����
        if(DoFadeIn==true)
        StartCoroutine(Fade(1,0));
    }
    public void FadeIn(float fTime)
    {
        fadeTime = fTime;
        StartCoroutine(Fade(1, 0));
    }
    public void FadeOut(float fTime)
    {
        fadeTime = 0.1f;
        StartCoroutine(Fade(0, 1));
    }

    public void JustBlack()
    {
        Color color = image.color;
        color.a = 1.0f;
        image.color = color;
    }


    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0f;
        float percent = 0f;

        while(percent<1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;
            yield return null;
        }
    }

}
