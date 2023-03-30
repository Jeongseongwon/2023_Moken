using System.Collections;
using UnityEngine;

public class MoveNode_Fade_Example : MonoBehaviour
{
    public LineGenerate line = null;
    public SpriteRenderer car_render = null;
    public float speed = 5;
    public bool loop = true;


    private void Start()
    {
        StartCoroutine(OnMove());
    }

    IEnumerator OnMove()
    {
        line.Move(transform, speed, MoveAlignment.transformZ, 2, 10);
        while (line.IsMove(transform))
        {
            line.Move(transform, speed, MoveAlignment.transformZ, 2, 10);
            yield return null;
        }
        StartCoroutine(OnFade());
    }

    IEnumerator OnFade()
    {
        Color color = Color.white;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            car_render.color = color;
            yield return null;
        }
        if (loop)
        {
            line.NewMovement(transform);
            car_render.color = Color.white;
            StartCoroutine(OnMove());
        }
        else Destroy(gameObject);
    }
}
