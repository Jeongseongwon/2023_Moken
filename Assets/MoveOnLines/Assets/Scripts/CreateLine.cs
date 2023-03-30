using System.Collections.Generic;
using UnityEngine;

public class CreateLine
{
    public enum LineType
    {
        smooth, beizer, circle, circuit, flat
    }

    public static List<Vector3> GetListPosition(List<Vector3> list_pos, float distance_per_point, LineType type)
    {
        if (type != LineType.circle && list_pos.Count < 3)
        {
            return Create2Point(list_pos[0], list_pos[1], (list_pos[1] - list_pos[0]).normalized, Vector3.Distance(list_pos[0], list_pos[1]), distance_per_point);
        }
        List<Vector3> postitions = new List<Vector3>();
        switch (type)
        {
            case LineType.smooth:
                int length = list_pos.Count;
                float[] _distances = new float[length];
                float total_distance = 0.0f;
                
                for (int i = 0; i < length - 1; i++)
                {
                    _distances[i] = total_distance;
                    total_distance += (list_pos[i + 1] - list_pos[i]).magnitude;
                }
                _distances[length - 1] = total_distance;
                float dist = 0.0f;
                while (dist < total_distance)
                {
                    int index = 1;
                    while (_distances[index] < dist)
                    {
                        index++;
                    }

                    float _interpolation = Mathf.InverseLerp(_distances[index - 1], _distances[index], dist);

                    index = index % length;
                    Vector3 point = CatmullRom(new Vector3[]
                    {
                        list_pos[Mathf.Clamp(index - 2, 0, length-1)],
                        list_pos[(index - 1 + length)%length],
                        list_pos[index % length],
                        list_pos[Mathf.Clamp(index + 1, 0, length-1)]
                    }, _interpolation);
                    postitions.Add(point);
                    dist = Mathf.Clamp(dist + distance_per_point, 0, total_distance);
                }
                break;
            case LineType.beizer:
                float length_pos = 0;
                for (int i = 0; i < list_pos.Count - 1; i++)
                {
                    float _length = Vector3.Distance(list_pos[i], list_pos[i + 1]) / (distance_per_point * 1.8f);
                    length_pos += _length;
                }
                for (int i = 0; i < length_pos; i++)
                {
                    postitions.Add(CalculateBezierPoint((i) / (float)length_pos, list_pos));
                }
                postitions.Add(list_pos[list_pos.Count - 1]);
                break;
            case LineType.circle:
                float distance = (list_pos[0] - list_pos[1]).magnitude;
                float circle_length = (2 * Mathf.PI * distance) / distance_per_point;
                for (int i = 0; i < (int)circle_length; i++)
                {
                    postitions.Add(CirclePoint(list_pos[0], list_pos[1], distance, i * (2f * Mathf.PI) / circle_length));
                }
                break;
            case LineType.circuit:
                length = list_pos.Count;
                _distances = new float[length + 1];
                total_distance = 0.0f;
                
                for (int i = 0; i < length - 1; i++)
                {
                    _distances[i] = total_distance;
                    total_distance += (list_pos[i + 1] - list_pos[i]).magnitude;
                }
                _distances[length - 1] = total_distance;
                total_distance += (list_pos[length - 1] - list_pos[0]).magnitude;

                _distances[_distances.Length - 1] = total_distance;
                dist = 0.0f;
                while (dist < total_distance)
                {
                    int index = 1;
                    while (_distances[index] < dist)
                    {
                        index++;
                    }

                    float _interpolation = Mathf.InverseLerp(_distances[index - 1], _distances[index], dist);

                    index = index % length;
                    Vector3 point = CatmullRom(new Vector3[]
                    {
                        list_pos[(index - 2 + length) % length],
                        list_pos[(index - 1 + length) % length],
                        list_pos[index % length],
                        list_pos[(index + 1) % length]
                    }, _interpolation);
                    postitions.Add(point);
                    
                    dist = Mathf.Clamp(dist + distance_per_point, 0, total_distance);
                }
                break;
            case LineType.flat:
                length = list_pos.Count;
                _distances = new float[list_pos.Count];
                total_distance = 0.0f;
                
                for (int i = 0; i < length - 1; i++)
                {
                    _distances[i] = total_distance;
                    total_distance += (list_pos[i + 1] - list_pos[i]).magnitude;
                }
                _distances[length - 1] = total_distance;
                dist = 0.0f;
                while (dist < total_distance)
                {
                    int index = 1;
                    while (_distances[index] < dist)
                    {
                        index++;
                    }

                    float _interpolation = Mathf.InverseLerp(_distances[index - 1], _distances[index], dist);

                    index = index % length;
                    Vector3 point = Vector3.Lerp(
                    list_pos[(index - 1 + length) % length],
                    list_pos[index % length],
                    _interpolation);
                    postitions.Add(point);
                    dist = Mathf.Clamp(dist + distance_per_point, 0, total_distance);
                }
                break;
        }
        return postitions;
    }

    private static List<Vector3> Create2Point(Vector3 first_pos, Vector3 finish_pos, Vector3 direction, float length, float size)
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(first_pos);
        float _i = 0;
        while (_i < (finish_pos - first_pos).magnitude - size)
        {
            _i += size;
            Vector3 pos = first_pos + direction * _i;
            positions.Add(pos);
        }
        positions.Add(finish_pos);
        return positions;
    }

    private static Vector3 CalculateBezierPoint(float t, List<Vector3> points)
    {
        int n = points.Count - 1;
        Vector3 result = Vector3.zero;
        for (int i = 0; i < points.Count; i++)
        {
            result += BinomialCoefficient(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i) * points[i];
        }
        return result;
    }

    private static int BinomialCoefficient(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    private static int Factorial(int value)
    {
        int i = 1;
        int result = 1;
        while (i < value)
        {
            i++;
            result *= i;
        }
        return result;
    }

    private static Vector3 CatmullRom(Vector3[] p, float i)
    {
        return 0.5f * ((2f * p[1]) + (-p[0] + p[2]) * i + (2f * p[0] - 5f * p[1] + 4f * p[2] - p[3]) * i * i + (-p[0] + 3f * p[1] - 3f * p[2] + p[3]) * i * i * i);
    }

    private static Vector3 CirclePoint(Vector3 p0, Vector3 p1, float d, float i)
    {
        return new Vector3(p0.x + d * Mathf.Cos(i), (Mathf.Abs(p0.z - p1.z) < 0.01f) ? p0.y + d * Mathf.Sin(i) : p0.y, (Mathf.Abs(p0.y - p1.y) < 0.01f) ? p0.z + d * Mathf.Sin(i) : p0.z);
    }
}
