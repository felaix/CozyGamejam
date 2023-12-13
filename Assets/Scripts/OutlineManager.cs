using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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


    private Dictionary<Collider2D, float> _pointDistances = new();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CheckDistance(Collider2D col, Vector2 playerPos)
    {
        float newDis = GetDistance(col, playerPos);

        //  first time triggering point
        if (IsFirstVisit(col)) 
        {
            AddPointAsEntry(col, newDis);
        }
        else
        {
            float lastDis = GetLastDistance(col);
            float shortestDis = GetShortestDistance(GetLastDistance(col), newDis);

            if (shortestDis != lastDis)
            {
                _pointDistances.Remove(col);
                AddPointAsEntry(col, shortestDis);
            }
        }

        if (AllPointsReached) ReturnToLevelSelection();
    }

    public float GetScore()
    {
        Debug.Log($"Total Distance: {TotalDistance}, Total Accuracy: {TotalAccuracy}");
        //  differenz zwischen _pointDistances.count und points.count
        //  => negativ berücksichtigen, dafür sollte ne schlechte akkuratheit genutzt werden
        //  int notReachedPoints * ;

        int notReachedPoints = points.Count - _pointDistances.Count;

        if (notReachedPoints > points.Count / 2) { notReachedPoints = notReachedPoints * 2; }

        float maxScore = 100f;
        float maxAccuracy = 2f;

        float score = Mathf.Clamp01(1f - TotalAccuracy / maxAccuracy) * maxScore - notReachedPoints;
        if (score <= 0f) { score = 0f; }
        Debug.Log($"maxscore: {maxScore}, totalAccuracy: {TotalAccuracy}, not reached points: {notReachedPoints}, Score. {score}");


        return score;
    }

    public void AddFootStep(Vector2 point)
    {
        footpoints.Add(point);

        //if (compared) return;

        //if (footpoints.Count >= points.Count) 
        //{
        //    CompareAccuracy(footpoints, points);
        //}
    }

    public void ResetPoints() => points.Clear();
    public void ResetFootSteps() => footpoints.Clear();
    public void AddPoint(Vector2 point) => points.Add(point);

    private void AddPointAsEntry(Collider2D col, float distance) => _pointDistances.Add(col, distance);


    public bool AllPointsReached => _pointDistances.Count >= points.Count;
    public bool IsFirstVisit(Collider2D col) => !_pointDistances.ContainsKey(col);


    public float TotalDistance => _pointDistances.Values.Sum();
    public float TotalAccuracy => TotalDistance / points.Count;

    private float GetDistance(Collider2D col, Vector2 playerPos) => Vector2.Distance(col.transform.position, playerPos);
    private float GetLastDistance(Collider2D col) => _pointDistances.GetValueOrDefault(col);
    private float GetShortestDistance(float lastDis, float newDis) => Mathf.Min(lastDis, newDis);

    public void ReturnToLevelSelection()
    {
        LevelController.Instance.FinishLevel(points, GetScore());
    }
}
