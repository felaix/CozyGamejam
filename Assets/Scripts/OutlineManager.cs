using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager Instance { get; private set; }

    //[SerializeField] private float multiplyAmount;

    [SerializeField] private List<ScriptableImage> outlines;

    [SerializeField] private TMP_Text pointsTxt;
    [SerializeField] private int sampleRate;

    [SerializeField] private List<Vector2> footpoints;
    [SerializeField] private List<Vector2> points;

    private PolygonCollider2D outlineCollider;

    private bool compared = false;
    
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

        if (compared) return;

        if (footpoints.Count >= points.Count) 
        {
            Debug.Log($"Footprints: {footpoints.Count} reached point amount: {points.Count}");
            CompareAccuracy(footpoints, points);
        }
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
        compared = false;
        CompareAccuracy(footpoints, points);
    }

    private void CompareAccuracy(List<Vector2> playerFootsteps, List<Vector2> points)
    {
        // a total value 
        float totalDistance = 0f;
        compared = true;
        
        for (int i = 0; i < playerFootsteps.Count-1; i++)
        {
            // get the distance of each playerfootstep and the vertice/point of the outline

            if (i > points.Count-1) break;
            if (i > footpoints.Count-1) break;

            float distance = Vector2.Distance(playerFootsteps[i], points[i]);

            // add the distance to the total distance
            totalDistance += distance;
            Debug.Log(distance);
        }

        // get the average distance by dividing the total distance by the count of footprints
        float averageDistance = totalDistance / footpoints.Count;
        //float score = averageDistance * multiplyAmount;

        Debug.Log("Average Distance: " +  averageDistance);

        LevelController.Instance.FinishLevel(points, footpoints);

    }

}
