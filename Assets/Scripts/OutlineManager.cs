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

    //[SerializeField] private TMP_Text pointsTxt;
    [SerializeField] private int sampleRate;

    [SerializeField] private List<Vector2> footpoints;
    [SerializeField] private List<Vector2> _totalPoints;

    [SerializeField] private GameObject scoreUI;
    [SerializeField] private TMP_Text scoreTMP;

    private Dictionary<Collider2D, float> _pointDistances = new();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic("StandardMusic");


        //LevelController.Instance.
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

        // ? lucy code
        //if (AllPointsReached) ReturnToLevelSelection();
        // ? ---

        // ! Show score and return to level selection after pressing button
        if (AllPointsReached) ShowScore();
    } 

    public float GetScore()
    {
        //  differenz zwischen _pointDistances.count und points.count
        //  => negativ berücksichtigen, dafür sollte ne schlechte akkuratheit genutzt werden
        //  int notReachedPoints * ;

        int notReachedPoints = (_totalPoints.Count - _pointDistances.Count) * 2;
        int penaltyForTooManyFootsteps = (_totalPoints.Count - footpoints.Count) * 2;


        if (notReachedPoints > _totalPoints.Count / 2) { notReachedPoints = notReachedPoints * 3; }

        int penalty = Mathf.Abs(notReachedPoints + penaltyForTooManyFootsteps);
        if (penalty > 30) penalty = 30;

        float maxScore = 100f;
        float maxAccuracy = 2f;

        float score = Mathf.Clamp01(1f - TotalAccuracy / maxAccuracy) * maxScore - penalty;
        Debug.Log($"maxscore: {maxScore}, totalAccuracy: {TotalAccuracy}, not reached points: {notReachedPoints}, penalty for footsteps: {penaltyForTooManyFootsteps}, total penalty: {penalty}");
        Debug.Log("Score: " + score);

        if (score < 0f) score = 0f;
        if (score > 100f) score = 100f;

        return score;
    }

    public void ShowScore()
    {
        scoreUI.gameObject.SetActive(true);
        scoreTMP.text = $"Score: {(int)GetScore()} / 100";
    }

    public void AddFootStep(Vector2 point)
    {
        footpoints.Add(point);
    }

    public void ResetPoints() => _totalPoints.Clear();
    public void ResetFootSteps() => footpoints.Clear();
    public void AddPoint(Vector2 point) => _totalPoints.Add(point);

    private void AddPointAsEntry(Collider2D col, float distance) => _pointDistances.Add(col, distance);


    public bool AllPointsReached => _pointDistances.Count >= _totalPoints.Count;
    public bool IsFirstVisit(Collider2D col) => !_pointDistances.ContainsKey(col);


    public float TotalDistance => _pointDistances.Values.Sum();
    public float TotalAccuracy => TotalDistance / _totalPoints.Count;

    private float GetDistance(Collider2D col, Vector2 playerPos) => Vector2.Distance(col.transform.position, playerPos);
    private float GetLastDistance(Collider2D col) => _pointDistances.GetValueOrDefault(col);
    private float GetShortestDistance(float lastDis, float newDis) => Mathf.Min(lastDis, newDis);

    public void ReturnToLevelSelection()
    {
        LevelController.Instance.FinishLevel(_totalPoints, GetScore());
    }
}
