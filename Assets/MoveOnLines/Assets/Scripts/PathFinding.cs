using System.Collections.Generic;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
#endif

public enum MoveAlignment
{
    transformY, transformZ
}
public class PathFinding : MonoBehaviour
{
    public static PathFinding instance { get; private set; }
    private void Awake()
    {
        instance = this;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<LineGenerate>().Create();
        }
    }

    public List<Point> listPoint = new List<Point>();
    //public enum Alignment { transformZ, transformY }
    public MoveAlignment aligiment = MoveAlignment.transformZ;



    /// <summary>
    /// Return the list position between current and target position
    /// </summary>
    /// <returns></returns>
    public static List<Vector3> Find(Vector3 current, Vector3 target)
    {
        return instance.Find_(current, target);
    }

    

    public List<Vector3> Find_(Vector3 current, Vector3 target)
    {
#if UNITY_EDITOR
        if (debug_content != null)
        {
            foreach(Transform child in debug_content)
            {
                Destroy(child.gameObject);
            }
        }
#endif
        OpenAllPoints();
        Point finish_point = GetNearestPoint(target);
        Point current_point = GetNearestPoint(current);

        int i = 0;
        if (current_point != finish_point)
        {
            current_point.Add_List_Pos(new List<Vector3>());
            current_point.Add_Line_Indeces(new List<int>()); //add line index
            List<Point> next_points = GetNextPoint(current_point);
            while (!IsFinishCheck(next_points, current_point, finish_point))
            {
                i++;
                List<Point> list_next_point = new List<Point>();
                foreach (Point next_point in next_points)
                {
                    list_next_point.AddRange(GetNextPoint(next_point));
#if UNITY_EDITOR
                    if (debug_point) SpawnDebugPoint(next_point.get);
#endif
                }
                

                if (next_points.Count == 0)
                {
                    Debug.Log("Can't Move to this positon. Re check all lines");
                    OpenAllPoints();
                    float min_distance = 100f;
                    Point nearest_point = null;
                    foreach (Point next_point in next_points)
                    {
                        float distance = (next_point.get - finish_point.get).magnitude;
                        if (distance < min_distance)
                        {
                            min_distance = distance;
                            nearest_point = next_point;
                        }
                    }
                    if (nearest_point == null) return new List<Vector3>() { current_point.get };
                    else return nearest_point.list_pos;
                    //break;
                }

                next_points = list_next_point;
            }
        }
        if (finish_point.list_pos == null) return new List<Vector3>();
        if (gameObject.GetComponent<LineRenderer>() != null)
        {
            LineRenderer line = gameObject.GetComponent<LineRenderer>();
            line.positionCount = finish_point.list_pos.Count;
            for (int j = 0; j < finish_point.list_pos.Count; j++)
            {
                line.SetPosition(j, finish_point.list_pos[j] + new Vector3(0, 0, -0.5f));
            }
        }
        if (finish_point.list_pos.Count > 2) finish_point.list_pos.RemoveAt(0);
        return finish_point.list_pos;
    }

    private bool IsFinishCheck(List<Point> next_points, Point current_point, Point finish_point)
    {
        foreach (Point next_point in next_points)
        {
            if (next_point == finish_point)
            {
                return true;
            }
        }
        return false;
    }

    private Point GetNearestPoint(Vector3 pos)
    {
        float min_distance = 100f;
        Point point_select = null;
        foreach (Point point in listPoint)
        {
            float distance = Vector3.Distance(pos, point.get);
            if (distance < min_distance)
            {
                min_distance = distance;
                point_select = point;
            }
        }
        return point_select;
    }

    private void OpenAllPoints()
    {
        foreach (Point point in listPoint)
        {
            point.is_close = false;
        }
    }

    private List<Point> GetNextPoint(Point point)
    {
        int index = listPoint.IndexOf(point);
        List<Point> list_nearpoints = new List<Point>();
        if (index < listPoint.Count - 1 && (point.move_type == LineGenerate.MoveType.two_side || point.move_type == LineGenerate.MoveType.move_0_to_n))
        {
            if (point.line_circle && listPoint[index + 1].line_index != point.line_index) //move circle
            {
                int i = index;
                while (listPoint[i].line_index == point.line_index && i > 0)
                {
                    i--;
                }
                i++;
                if (!listPoint[i].is_close)
                {
                    listPoint[i].is_close = true;
                    listPoint[i].Add_List_Pos(point.list_pos);
                    listPoint[i].Add_Line_Indeces(point.line_indeces); // add line index
                    list_nearpoints.Add(listPoint[i]);

                }
            }
            else
            if (!listPoint[index + 1].is_close && listPoint[index + 1].line_index == point.line_index)
            {
                listPoint[index + 1].is_close = true;
                listPoint[index + 1].Add_List_Pos(point.list_pos);
                listPoint[index + 1].Add_Line_Indeces(point.line_indeces); // add line index
                list_nearpoints.Add(listPoint[index + 1]);

            }

            if (point.move_type == LineGenerate.MoveType.move_0_to_n) //only right
            {
                list_nearpoints.AddRange(GetNextPointOtherLine(point));
                return list_nearpoints;
            }
        }
        else
        {
            foreach (Point _point in listPoint)
            {
                if (!_point.is_close &&
                    Vector3.Distance(point.get, _point.get) < maxDistanceGetOtherLine)
                {
                    _point.is_close = true;
                    _point.Add_List_Pos(point.list_pos);
                    _point.Add_Line_Indeces(point.line_indeces); // add line index
                    list_nearpoints.Add(_point);

                }
            }
        }
        if (index > 0 && (point.move_type == LineGenerate.MoveType.two_side || point.move_type == LineGenerate.MoveType.move_n_to_0))
        {
            if (point.line_circle && listPoint[index - 1].line_index != point.line_index) //move circle
            {
                int i = index;
                while (listPoint[i].line_index == point.line_index && i < listPoint.Count - 1)
                {
                    i++;
                }
                i--;
                if (!listPoint[i].is_close)
                {
                    listPoint[i].is_close = true;
                    listPoint[i].Add_List_Pos(point.list_pos);
                    listPoint[i].Add_Line_Indeces(point.line_indeces); // add line index
                    list_nearpoints.Add(listPoint[i]);

                }
            }
            else
            if (!listPoint[index - 1].is_close && listPoint[index - 1].line_index == point.line_index)
            {
                listPoint[index - 1].is_close = true;
                listPoint[index - 1].Add_List_Pos(point.list_pos);
                listPoint[index - 1].Add_Line_Indeces(point.line_indeces); // add line index
                list_nearpoints.Add(listPoint[index - 1]);

            }

            if (point.move_type == LineGenerate.MoveType.move_n_to_0) //only right
            {
                list_nearpoints.AddRange(GetNextPointOtherLine(point));
                return list_nearpoints;
            }
        }
        list_nearpoints.AddRange(GetNextPointOtherLine(point));
        return list_nearpoints;
    }
    [Range(0.1f, 1f)]
    public float maxDistanceGetOtherLine = 0.1f;
    private List<Point> GetNextPointOtherLine(Point point)
    {
        List<Point> list_nearpoints = new List<Point>();

        foreach (Point _point in listPoint)
        {
            float distance = Vector3.Distance(point.get, _point.get);
            if (point.line_index != _point.line_index && distance < maxDistanceGetOtherLine)
            {
                list_nearpoints.Add(_point);
            }
        }
        int i = 0;
        while (i < list_nearpoints.Count)
        {
            i++;
            float min_distance = maxDistanceGetOtherLine;
            Point point_select = null;
            foreach (Point near_point in list_nearpoints)
            {
                float distance = Vector3.Distance(point.get, near_point.get);
                if (!near_point.is_close && distance < min_distance)
                {
                    min_distance = distance;
                    point_select = near_point;
                }
            }
            int length_index_per_point = 0;
            if (point_select == null) return new List<Point>();
            Point point_nearest = null;
            min_distance = maxDistanceGetOtherLine;
            foreach (Point near_point in list_nearpoints)
            {
                if (point_select.line_index == near_point.line_index)
                {
                    length_index_per_point++;
                    float distance = Vector3.Distance(point.get, near_point.get);
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        point_nearest = near_point;
                    }
                }
            }
            if (length_index_per_point < 3)
            {
                point_select.is_close = true;
                if (point.list_pos != null) point.list_pos[point.list_pos.Count - 1] = GetPosEdgeCross(point, point_nearest);
                point_select.Add_List_Pos(point.list_pos);
                point_select.Add_Line_Indeces(point.line_indeces); // add line index
            }
            else list_nearpoints.Remove(point_select);
        }
        return list_nearpoints;
    }

    private Vector3 GetPosEdgeCross(Point point1, Point point2)
    {
        int i_1 = listPoint.IndexOf(point1);
        int i_2 = listPoint.IndexOf(point2);
        int i_1_n = i_1, i_2_n = i_2;
        if (i_1 + 1 > listPoint.Count - 1) i_1_n = i_1 - 1;
        else if (i_1 == 0) i_1_n = i_1 + 1;
        else if (listPoint[i_1].line_index != listPoint[i_1 + 1].line_index) i_1_n = i_1 - 1;
        else i_1_n = i_1 + 1;
        if (i_2 + 1 > listPoint.Count - 1) i_2_n = i_2 - 1;
        else if (i_2 == 0) i_2_n = i_2 + 1;
        else if (listPoint[i_2].line_index != listPoint[i_2 + 1].line_index) i_2_n = i_2 - 1;
        else i_2_n = i_2 + 1;
        float a_1 = 0, a_2 = 0, b_1 = 0, b_2 = 0, x = 0;
        if (Mathf.Abs(listPoint[i_1_n].get.x - listPoint[i_1].get.x) < 0.01f || Mathf.Abs(listPoint[i_2_n].get.x - listPoint[i_2].get.x) < 0.01f)
        {
            x = listPoint[i_1].get.x;
            b_1 = listPoint[i_2].get.z + listPoint[i_2].get.y;
        }
        else if (((Mathf.Abs(listPoint[i_1_n].get.y - listPoint[i_1].get.y) < 0.01f || Mathf.Abs(listPoint[i_2_n].get.y - listPoint[i_2].get.y) < 0.01f)
            && aligiment == MoveAlignment.transformZ) ||
            ((Mathf.Abs(listPoint[i_1_n].get.z - listPoint[i_1].get.z) < 0.01f || Mathf.Abs(listPoint[i_2_n].get.z - listPoint[i_2].get.z) < 0.01f)
            && aligiment == MoveAlignment.transformY))
        {
            x = listPoint[i_1].get.x;
            b_1 = listPoint[i_2].get.z + listPoint[i_2].get.y;
        }
        else
        {
            a_1 = ((listPoint[i_1_n].get.z + listPoint[i_1_n].get.y) -
                (listPoint[i_1].get.z + listPoint[i_1].get.y)) / (listPoint[i_1_n].get.x - listPoint[i_1].get.x);
            a_2 = ((listPoint[i_2_n].get.z + listPoint[i_2_n].get.y) -
                (listPoint[i_2].get.z + listPoint[i_2].get.y)) / (listPoint[i_2_n].get.x - listPoint[i_2].get.x);
            b_1 = (listPoint[i_1_n].get.z + listPoint[i_1_n].get.y) - a_1 * listPoint[i_1_n].get.x;
            b_2 = (listPoint[i_2_n].get.z + listPoint[i_2_n].get.y) - a_2 * listPoint[i_2_n].get.x;
            x = (b_2 - b_1) / (a_1 - a_2);
        }

        if (aligiment == MoveAlignment.transformY) return new Vector3(x, 0, a_1 * x + b_1);
        else return new Vector3(x, a_1 * x + b_1, 0);
    }



#if UNITY_EDITOR
    [SerializeField] bool debug_point = false;
    private Transform debug_content = null;
    private void SpawnDebugPoint(Vector3 pos)
    {
        if (AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd") == null) return;
        if (debug_content == null) debug_content = new GameObject("[debug]").transform;
        debug_content.parent = null;
        GameObject debug = new GameObject(string.Format("line ({0})", debug_content.childCount), typeof(SpriteRenderer));
        debug.GetComponent<SpriteRenderer>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        debug.GetComponent<SpriteRenderer>().color = Color.green;
        debug.GetComponent<SpriteRenderer>().sortingOrder = 1;
        debug.transform.parent = debug_content;
        debug.transform.position = pos;
        if (aligiment == MoveAlignment.transformY) debug.transform.rotation *= Quaternion.AngleAxis(90, Vector3.right);
    }

    public void SpawnLineInEditor()
    {
        GameObject line = new GameObject(string.Format("line ({0})", transform.childCount), typeof(LineGenerate));
        Vector3 spawn_pos = Vector3.zero;
        if (transform.childCount > 0) spawn_pos = transform.GetChild(transform.childCount - 1).
                GetChild(transform.GetChild(transform.childCount - 1).childCount - 1).position + Vector3.up;
        spawn_pos.z = 0;
        line.transform.position = spawn_pos;
        for (int i = 0; i < 2; i++)
        {
            GameObject point = new GameObject(string.Format("point ({0})", line.transform.childCount));
            point.transform.parent = line.transform;
            point.transform.localPosition = Vector3.right * i * 2;
            Texture2D tex = EditorGUIUtility.IconContent("sv_label_1").image as Texture2D;
            Type editorGUIUtilityType = typeof(EditorGUIUtility);
            BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            object[] args = { point, tex };
            editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
        }
        line.transform.parent = transform;
        line.GetComponent<LineGenerate>().SetDistancePerPos(maxDistanceGetOtherLine);
    }

    [MenuItem("Tools/Move On Lines/New Path")]
    public static void CreateNewPathFinding() =>
            new GameObject("[PathFinding]", typeof(PathFinding));

    [MenuItem("Tools/Move On Lines/Single Line")]
    public static void CreateSingleLine()
    {
        GameObject line = new GameObject(string.Format("single line (clone)"), typeof(LineGenerate));
        for (int i = 0; i < 2; i++)
        {
            GameObject point = new GameObject(string.Format("point ({0})", line.transform.childCount));
            point.transform.parent = line.transform;
            point.transform.localPosition = Vector3.right * i * 2;
            Texture2D tex = EditorGUIUtility.IconContent("sv_label_1").image as Texture2D;
            Type editorGUIUtilityType = typeof(EditorGUIUtility);
            BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            object[] args = { point, tex };
            editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
        }
    }

#endif


    //Action<ListPosInObject> _move_object_callback;

    public IEnumerator LoadListPos(Vector3 current, Vector3 target, int id, int speed)
    {
        ListPosInObject object_move = GetObjectMoveByID(id);
        object_move.list_pos.Clear();
        object_move.isFinding = true;
#if UNITY_EDITOR
        if (debug_content != null)
        {
            foreach (Transform child in debug_content)
            {
                Destroy(child.gameObject);
            }
        }
#endif
        OpenAllPoints();
        Point finish_point = GetNearestPoint(target);
        Point current_point = GetNearestPoint(current);
        Point nearest_point = null;

        int s = 0;
        int i = 0;
        if (current_point != finish_point)
        {
            current_point.Add_List_Pos(new List<Vector3>());
            current_point.Add_Line_Indeces(new List<int>()); //add line index
            List<Point> next_points = GetNextPoint(current_point);
            while (!IsFinishCheck(next_points, current_point, finish_point))
            {
                i++;
                List<Point> list_next_point = new List<Point>();
                foreach (Point next_point in next_points)
                {
                    list_next_point.AddRange(GetNextPoint(next_point));
#if UNITY_EDITOR
                    if (debug_point) SpawnDebugPoint(next_point.get);
#endif
                }


                if (next_points.Count == 0)
                {
                    Debug.Log("Can't Move to this positon. Re check all line");
                    OpenAllPoints();
                    float min_distance = 100f;
                    
                    foreach (Point next_point in next_points)
                    {
                        float distance = (next_point.get - finish_point.get).magnitude;
                        if (distance < min_distance)
                        {
                            min_distance = distance;
                            nearest_point = next_point;
                        }
                    }
                    break;
                }

                next_points = list_next_point;
                if (s > speed)
                {
                    yield return null;
                    s = 0;
                } s++;
            }
        }
        if (finish_point.list_pos == null)
        {
            if (nearest_point == null) object_move.list_pos = new List<Vector3> { current_point.get };
            else object_move.list_pos = nearest_point.list_pos;
        }
        else
        {
            if (gameObject.GetComponent<LineRenderer>() != null)
            {
                LineRenderer line = gameObject.GetComponent<LineRenderer>();
                line.positionCount = finish_point.list_pos.Count;
                for (int j = 0; j < finish_point.list_pos.Count; j++)
                {
                    line.SetPosition(j, finish_point.list_pos[j] + new Vector3(0, 0, -0.5f));
                }
            }
            if (finish_point.list_pos.Count > 2) finish_point.list_pos.RemoveAt(0);
            object_move.list_pos = finish_point.list_pos;
        }
        object_move.isFinding = false;
        object_move.target = target;
    }


    static List<ListPosInObject> list_object_move = new List<ListPosInObject>();
    /// <summary>
    /// Moving transformObject to target position in path
    /// </summary>
    /// <returns></returns>
    public static void Move(Transform transformObject, Vector3 target, float moveSpeed, int lookNextPosIndex = 0, float rotationSpeed = 10)
    {
        ListPosInObject object_move = GetObjectMoveByID(transformObject.GetInstanceID());
        if (object_move == null)
        {
            object_move = new ListPosInObject()
            {
                id = transformObject.GetInstanceID(),
                list_pos = new List<Vector3>(),
                target = (instance.aligiment == MoveAlignment.transformZ) ? Vector3.forward : Vector3.up
            };
            list_object_move.Add(object_move);
        }

        if (!object_move.isFinding && (object_move.target - target).magnitude > 0.1)
        {
            //instance.StartCoroutine(instance.LoadListPos(transformObject.position, target, object_move.id, 1));
            //or
            object_move.list_pos = Find(transformObject.position, target); object_move.target = target;
        }

        if (object_move.list_pos.Count == 0) return;
        transformObject.position = Vector3.MoveTowards(transformObject.position, object_move.list_pos[0], Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transformObject.position, object_move.list_pos[0]) < 0.1f) object_move.list_pos.RemoveAt(0);

        if (lookNextPosIndex <= 0) return;
        if (object_move.list_pos.Count > lookNextPosIndex)
        {
            
            if (instance.aligiment == MoveAlignment.transformZ)
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
    /// Clear move node transformObject
    /// </summary>
    /// <returns></returns>
    public static void Clear(Transform transformObject)
    {
        GetObjectMoveByID(transformObject.GetInstanceID()).list_pos.Clear();
    }

    /// <summary>
    /// Check transformObject is move
    /// </summary>
    /// <returns></returns>
    public static bool IsMove(Transform transformObject)
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
    public static List<Vector3> GetListPos(Transform transformObject)
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

    private static ListPosInObject GetObjectMoveByID(int id)
    {
        foreach(ListPosInObject object_move in list_object_move)
        {
            if (object_move.id == id) return object_move;
        }
        return null;
    }
}

public class Point
{
    public Vector3 get { get; set; }
    public int line_index { get; set; }
    public bool is_close { get; set; }
    public List<Vector3> list_pos { get; set; }
    public LineGenerate.MoveType move_type { get; set; }
    public bool line_circle { get; set; }

    public void Add_List_Pos(List<Vector3> previous_list)
    {
        if (previous_list == null) return;
        list_pos = new List<Vector3>();
        list_pos.AddRange(previous_list);
        list_pos.Add(get);
    }

    public List<int> line_indeces { get; set; }
    public void Add_Line_Indeces(List<int> previous_indeces)
    {
        if (previous_indeces == null) return;
        line_indeces = new List<int>();
        line_indeces.AddRange(previous_indeces);
        if (line_indeces.Contains(line_index)) return;
        line_indeces.Add(line_index);
    }
}


public class CoroutineWithData<T>
{
    private IEnumerator _target;
    public T result;
    public Coroutine Coroutine { get; private set; }

    public CoroutineWithData(MonoBehaviour owner_, IEnumerator target_)
    {
        _target = target_;
        Coroutine = owner_.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (_target.MoveNext())
        {
            result = (T)_target.Current;
            yield return result;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PathFinding))]
[CanEditMultipleObjects]
public class PathBehaviour_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnityEngine.Object[] path_scripts = targets;
        if (GUILayout.Button("Create The Line"))
        {
            PathFinding path;
            for (int i = 0; i < path_scripts.Length; i++)
            {
                path = (PathFinding)path_scripts[i];
                path.SpawnLineInEditor();
            }
        }
    }
}
#endif
