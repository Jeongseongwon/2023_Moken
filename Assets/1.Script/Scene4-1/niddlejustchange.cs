using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class niddlejustchange : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10f)]
    private float fadeTime;
    private Image image;
    private bool flag = true;
    [SerializeField]
    private AnimationCurve fadeCurve;
    public GameObject spin;
    private float count = 0;
    public GameObject nowscene;
    public GameObject nextscene;
    public GameObject nextscene_blink;


    float timer = 0.0f;
    float waitingTime = 1.5f;
    int scenumber = 4;

    void Start()
    {

        image = GetComponent<Image>();

        StartCoroutine(FadeInOut());

    }
    void Update()
    {
        if (scenumber == 0)
        {
            nowscene.SetActive(false);
            nextscene.SetActive(true);
            nextscene_blink.SetActive(true);
        }
    }



    private IEnumerator FadeInOut()
    {
        while (true)
        {
            for (int count = 0; count <= 4; count++)
            {
                yield return StartCoroutine(Fade(0, 1));
                yield return new WaitForSeconds(1.5f);
                yield return StartCoroutine(Fade(1, 0));
                spin.GetComponent<RectTransform>().Rotate(0, 0, -90, Space.Self);
                if (count == 4)
                {
                    for (; count >= 0; count--)
                    {
                        yield return StartCoroutine(Fade(0, 1));
                        yield return new WaitForSeconds(1.5f);
                        yield return StartCoroutine(Fade(1, 0));
                        spin.GetComponent<RectTransform>().Rotate(0, 0, 90, Space.Self);
                        scenumber--;
                    }
                }


            }

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

}
