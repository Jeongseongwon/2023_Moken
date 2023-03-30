using UnityEngine;

public class MoveNode_Example : MonoBehaviour
{
    public LineGenerate line = null;
    public float speed;

    private void Update()
    {
        line.Move(transform, 200, MoveAlignment.transformZ, 1, 10);
    }
}
