using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockmove : MonoBehaviour
{

    private float count = 0;
    private bool flag = true;

    [Header("회전속도조절")]
    [SerializeField] [Range(1f, 100f)] float rotateSpeed = 50f;


    void Update()
    {
        if (flag == true)
        {
            count += Time.deltaTime;
        }
        if (count == 3)
            StartCoroutine(check());
        else
            move();
    }
    IEnumerator check()
    {
        yield return new WaitForSeconds(3);
    }
    void move()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.Self);

    }

}
