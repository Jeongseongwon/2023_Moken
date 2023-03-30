using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListPosInObject
{
    public int id { get; set; }
    public List<Vector3> list_pos { get; set; }
    public Vector3 target { get; set; }
    public bool isFinding { get; set; }
    public bool flip { get; set; }
}
