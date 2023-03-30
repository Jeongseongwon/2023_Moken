using System.Collections.Generic;
using UnityEngine;

public class LineGenerate : MonoBehaviour
{
    [SerializeField] float distancePerPosition = 0f;
    public CreateLine.LineType lineType = CreateLine.LineType.smooth;
    public enum MoveType { two_side, move_0_to_n, move_n_to_0 }
    public MoveType moveType = MoveType.two_side;

    List<Vector3> positions = new List<Vector3>();



    /// <summary>
    /// Get the list position in line
    /// </summary>
    public List<Vector3> GetPositions()
    {
        CheckPositions();
        return positions;
    }

    /// <summary>
    /// Create line
    /// </summary>
    /// <returns></returns>
    public void Create()
    {
        if (transform.parent != null) if (transform.parent.gameObject.GetComponent<PathFinding>() != null) distancePerPosition = PathFinding.instance.maxDistanceGetOtherLine * 1.5f;
        List<Vector3> child_pos = new List<Vector3>();
        foreach (Transform child in transform)
        {
            child_pos.Add(child.position);
        }
        positions = CreateLine.GetListPosition(child_pos, distancePerPosition, lineType);
        if (PathFinding.instance != null)
        {
            int i = 0;
            int points_length = PathFinding.instance.listPoint.Count;
            if (points_length > 0) i = PathFinding.instance.listPoint[points_length - 1].line_index + 1;
            foreach (Vector3 pos in positions)
            {
                Point point = new Point()
                {
                    get = pos,
                    line_index = i,
                    move_type = moveType,
                    line_circle = (lineType == CreateLine.LineType.circle || lineType == CreateLine.LineType.circuit) ? true : false
                };
                PathFinding.instance.listPoint.Add(point);
            }
        }
        if (gameObject.GetComponent<LineRenderer>())
        {
            LineRenderer line = gameObject.GetComponent<LineRenderer>();
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());
            if (lineType == CreateLine.LineType.circle || lineType == CreateLine.LineType.circuit) line.loop = true;
        }
    }

    private void CheckPositions()
    {
        if (positions.Count > 0) return;
        Create();
    }

    List<ListPosInObject> list_object_move = new List<ListPosInObject>();

    /// <summary>
    /// Moving transformObject on the line
    /// </summary>
    /// <returns></returns>
    public void Move(Transform transformObject, float moveSpeed, MoveAlignment aligiment = MoveAlignment.transformZ
        ,int lookNextPosIndex = 0, float rotationSpeed = 1000)
    {
        CheckPositions();
        ListPosInObject object_move = GetObjectMoveByID(transformObject.GetInstanceID());
        if (object_move == null)
        {
            object_move = new ListPosInObject()
            {
                id = transformObject.GetInstanceID(),
                list_pos = new List<Vector3>(),
                flip = (moveType == MoveType.move_n_to_0) ? true : false
            };
            list_object_move.Add(object_move);
            object_move.list_pos.AddRange(positions);
            if (object_move.flip) object_move.list_pos.Reverse();
        }

        if (object_move.list_pos.Count == 0)
        {
            if (moveType == MoveType.two_side)
            {
                object_move.list_pos.AddRange(positions);
                if (object_move.flip) object_move.list_pos.Reverse();
                object_move.flip = !object_move.flip;
            }
            else
            {
                if (lineType == CreateLine.LineType.circle || lineType == CreateLine.LineType.circuit)
                {
                    object_move.list_pos.AddRange(positions);
                    if (object_move.flip) object_move.list_pos.Reverse();
                }
                else return;
            }
            //return;
        }
        transformObject.position = Vector3.MoveTowards(transformObject.position, object_move.list_pos[0], Time.deltaTime *1000* moveSpeed);
        if (Vector3.Distance(transformObject.position, object_move.list_pos[0]) < 0.1f) object_move.list_pos.RemoveAt(0);

        if (lookNextPosIndex <= 0) return;
        if (object_move.list_pos.Count > lookNextPosIndex)
        {

            if (aligiment == MoveAlignment.transformZ)
            {
                Vector3 dir = object_move.list_pos[lookNextPosIndex] - transformObject.position;
                float angel = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transformObject.rotation = Quaternion.Lerp(transformObject.rotation, Quaternion.AngleAxis(angel, Vector3.forward),
                    Time.deltaTime * rotationSpeed);
            }
            else
            {
                Vector3 dir = object_move.list_pos[lookNextPosIndex] - transformObject.position;
                float angel = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                transformObject.rotation = Quaternion.Lerp(transformObject.rotation, Quaternion.AngleAxis(angel, Vector3.up),
                    Time.deltaTime * rotationSpeed);
            }
        }
    }


    /// <summary>
    /// Check transformObject is move
    /// </summary>
    /// <returns></returns>
    public bool IsMove(Transform transformObject)
    {
        foreach (ListPosInObject object_move in list_object_move)
        {
            if (object_move.id == transformObject.GetInstanceID())
            {
                if (object_move.list_pos.Count > 0) return true;
                else return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Get list position for moving transformObject
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetListPos(Transform transformObject)
    {
        foreach (ListPosInObject object_move in list_object_move)
        {
            if (object_move.id == transformObject.GetInstanceID())
            {
                if (object_move.list_pos.Count > 0) return object_move.list_pos;
                else return new List<Vector3>();
            }
        }
        return new List<Vector3>();
    }

    /// <summary>
    /// Create new movement for transformObject
    /// </summary>
    /// <returns></returns>
    public void NewMovement(Transform transformObject)
    {
        CheckPositions();
        foreach (ListPosInObject object_move in list_object_move)
        {
            if (object_move.id == transformObject.GetInstanceID())
            {
                object_move.list_pos.AddRange(positions);
                if (object_move.flip) object_move.list_pos.Reverse();
                transformObject.position = object_move.list_pos[0];
                return;
            }
        }

        ListPosInObject new_object_move = new ListPosInObject()
        {
            id = transformObject.GetInstanceID(),
            list_pos = new List<Vector3>(),
            flip = (moveType == MoveType.move_n_to_0) ? true : false
        };
        list_object_move.Add(new_object_move);
        new_object_move.list_pos.AddRange(positions);
        if (new_object_move.flip) new_object_move.list_pos.Reverse();
        transformObject.position = new_object_move.list_pos[0];
    }

    private ListPosInObject GetObjectMoveByID(int id)
    {
        foreach (ListPosInObject object_move in list_object_move)
        {
            if (object_move.id == id) return object_move;
        }
        return null;
    }





#if UNITY_EDITOR
    public void SetDistancePerPos(float value)
    {
        distancePerPosition = value * 1.5f;
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }

    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }

    private void DrawGizmos(bool is_view)
    {
           
        if (distancePerPosition < 0.1f) return;
        if (transform.parent != null)
        {
            if (transform.parent.gameObject.GetComponent<PathFinding>() != null)
            {
                if (transform.parent.gameObject.GetComponent<PathFinding>().maxDistanceGetOtherLine < 0.1f) return;
                distancePerPosition = transform.parent.gameObject.GetComponent<PathFinding>().maxDistanceGetOtherLine * 1.5f;
            }
        }
        if (transform.childCount < 2) return;
        List<Vector3> child_pos = new List<Vector3>();
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        foreach (Transform child in transform)
        {
            Gizmos.DrawSphere(child.position, distancePerPosition / 1.5f);
            child_pos.Add(child.position);
        }
        Gizmos.color = Color.gray;
        for (int i = 0; i < child_pos.Count - 1; i++)
        {
            Gizmos.DrawLine(child_pos[i], child_pos[i + 1]);
        }

        List<Vector3> list_pos = CreateLine.GetListPosition(child_pos, distancePerPosition, lineType);
        Gizmos.color = is_view ? Color.cyan : Color.blue;
        for (int i = 0; i < list_pos.Count - 1; i++)
        {
            Gizmos.DrawLine(list_pos[i], list_pos[i + 1]);
        }
        if (lineType == CreateLine.LineType.circle || lineType == CreateLine.LineType.circuit) Gizmos.DrawLine(list_pos[list_pos.Count - 1], list_pos[0]);
    }
#endif
}