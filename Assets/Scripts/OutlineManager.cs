using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager Instance { get; private set; }

    [SerializeField] private int sampleRate;

    [SerializeField] private List<ScriptableImage> outlines;

    [SerializeField] private List<Vector2> footpoints;
    public List<Vector2> TotalPoints => _totalPoints;
    [SerializeField] private List<Vector2> _totalPoints;

    [SerializeField] private GameObject scoreUI;
    [SerializeField] private TMP_Text scoreTMP;

    private Dictionary<Collider2D, float> _pointDistances = new();

    [SerializeField]
    private float _minRange = 0.025f;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic("StandardMusic");
    }

    private float _visitedPoints = 0;

    public void CheckDistance(Collider2D col, Vector2 playerPos)
    {
        float dis = GetPlayerPointDistance(col, playerPos);

        //  first time triggering point
        if (IsFirstVisit(col))
        {
            Debug.Log($"visited points: {_visitedPoints}");
            AddPointAsEntry(col, dis);
        }
        else
        {
            float lastDis = GetLastDistance(col);
            float shortestDis = GetShortestDistance(GetLastDistance(col), dis);

            if (shortestDis != lastDis)
            {
                _pointDistances.Remove(col);
                AddPointAsEntry(col, shortestDis);
            }
        }

        // ! Show score and return to level selection after pressing button
        if (AllPointsReached) ShowScore();
    }

    private float GetPlayerPointDistance(Collider2D col, Vector2 playerPos)
    {
        float newDis = GetDistance(col, playerPos);
        //  return distance of 0, when minRequiredRange is reached
        if (IsInMinRange(newDis)) newDis = BestDistance;

        return newDis;
    }

    public float GetScore()
    {
        //  differenz zwischen _pointDistances.count und points.count
        //  => negativ berücksichtigen, dafür sollte ne schlechte akkuratheit genutzt werden
        //  int notReachedPoints * ;

        int missingPoints = NotReachedPoints;
        int missingPointsPenalty = (_totalPoints.Count - footpoints.Count) * 2;

        if (missingPoints > _totalPoints.Count / 2)
        {
            missingPoints = missingPoints * 3;
        }

        int penalty = Mathf.Abs(missingPoints + missingPointsPenalty);
        if (penalty > 30) penalty = 30;

        float maxScore = 100f;
        float maxAccuracy = 2f;

        float score = Mathf.Clamp01(1f - TotalAccuracy / maxAccuracy) * maxScore - penalty;
        Debug.Log($"maxscore: {maxScore}, totalAccuracy: {TotalAccuracy}, not reached points: {missingPoints}, penalty for footsteps: {missingPointsPenalty}, total penalty: {penalty}");
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
    public bool IsInMinRange(float dis) => dis < _minRange;

    public float TotalDistance => _pointDistances.Values.Sum();
    public float TotalAccuracy => TotalDistance / _totalPoints.Count;
    public float BestDistance => 0;

    private float GetDistance(Collider2D col, Vector2 playerPos) => Vector2.Distance(col.transform.position, playerPos);
    private float GetLastDistance(Collider2D col) => _pointDistances.GetValueOrDefault(col);
    private float GetShortestDistance(float lastDis, float newDis) => Mathf.Min(lastDis, newDis);

    public int NotReachedPoints => (_totalPoints.Count - _pointDistances.Count) * 2;
}
