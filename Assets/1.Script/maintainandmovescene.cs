using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class maintainandmovescene : MonoBehaviour
{
    public float time;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float fadeTime;
    private Image image;
    public GameObject nowimage;
    public GameObject nextimage;
    [SerializeField]
    private AnimationCurve fadeCurve;

    public bool Fadein = false;
    public bool Fadeout = false;


    private void Awake()
    {
        image = GetComponent<Image>();

        StartCoroutine(FadeInOut());


    }

    private IEnumerator FadeInOut()
    {

        while (true)
        {
            if (Fadeout)
            {
                yield return StartCoroutine(Fade(0, 1));
            }
            else if (Fadein)
            {
                yield return StartCoroutine(Fade(1, 0));
            }
            yield return new WaitForSeconds(time);

            next();
            break;

        }
    }
    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;
            yield return null;

        }
    }
    public void next()
    {
        if (nowimage != null && nextimage!=null)
        {
            nowimage.SetActive(false);
            nextimage.SetActive(true);
        }
    }

}
