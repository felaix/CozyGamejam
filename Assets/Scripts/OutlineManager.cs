using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager Instance { get; private set; }

    [SerializeField] private TMP_Text pointsTxt;
    [SerializeField] private int sampleRate;

    [SerializeField] private List<Vector2> footpoints;
    [SerializeField] private List<Vector2> points;

    private PolygonCollider2D outlineCollider;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public bool CheckCollision(Vector2 pos)
    {
        return outlineCollider.OverlapPoint(pos);
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
    }

    public void AddFootStep(Vector2 point)
    {
        footpoints.Add(point);
    }

    public void ResetPoints()
    {
        points.Clear();
    }

    public void ResetFootSteps()
    {
        footpoints.Clear();
    }

    public void Compare()
    {
        CompareAccuracy(footpoints, points);
    }

    private void CompareAccuracy(List<Vector2> playerFootsteps, List<Vector2> points)
    {
        float totalDistance = 0f;

        for (int i = 0; i < playerFootsteps.Count; i++)
        {
            float distance = Vector2.Distance(playerFootsteps[i], points[i]);
            totalDistance += distance;
            Debug.Log(distance);
        }

        float averageDistance = totalDistance / footpoints.Count;

        Debug.Log("Average Distance: " +  averageDistance);
    }

}
