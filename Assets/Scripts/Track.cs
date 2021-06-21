using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track
{
    private List<Vector3> _trackPoint;
    public Track()
    {
        _trackPoint = new List<Vector3>();
    }

    public void AddPoints(Vector3[] points)
    {
        foreach (var point in points)
            AddPoint(point);
    }

    public void AddPoint(Vector3 point)
    {
        _trackPoint.Add(point);
        if(_trackPoint.Count > 200)
        {

        }
    }

    public Vector3 GetTrackPosition(Vector3 position, float distance)
    {
        for(int i = _trackPoint.Count - 1; i > 0; i--)
        {
            if(IsPointLiesOnTheLine(_trackPoint[i], _trackPoint[i - 1], position))
            {
                if (Vector3.Distance(position, _trackPoint[i]) > distance)
                {
                    float t = distance / Vector3.Distance(position, _trackPoint[i]);
                    return Vector3.Lerp(position, _trackPoint[i], t);
                }
                else if (Vector3.Distance(position, _trackPoint[i]) == distance)
                {
                    return _trackPoint[i];
                }
                else if (Vector3.Distance(position, _trackPoint[i]) < distance)
                {
                    return GetTrackPosition(_trackPoint[i], distance - Vector3.Distance(position, _trackPoint[i]));
                }
            }
        }
        Debug.LogError("Dot stay outside track");
        return Vector3.zero;
    }

    private bool IsPointLiesOnTheLine(Vector3 vector1, Vector3 vector2, Vector3 dot)
    {
        return dot == vector1 || dot == vector2 || ScalarProduct(vector1,vector2, dot) < 0;
    }

    private float ScalarProduct(Vector3 vector1, Vector3 vector2, Vector3 dot)
    {
        Vector3 vectorA = vector1 - dot;
        Vector3 vectorB = vector2 - dot;

        float scalarProduct = vectorA.x * vectorB.x + vectorA.y * vectorB.y;
        float modulA = VectorToModul(vectorA);
        float modulB = VectorToModul(vectorB);

        float cos = scalarProduct / (modulA * modulB);
        float result = modulA * modulB * cos;

        return result;
    }

    private float VectorToModul(Vector3 vector) => Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));


}

