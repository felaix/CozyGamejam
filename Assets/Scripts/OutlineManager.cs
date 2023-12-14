using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    [SerializeField]
    private CollisionCheck _player;
    public static OutlineManager Instance { get; private set; }

    [SerializeField] private int sampleRate;

    [SerializeField] private List<ScriptableImage> outlines;

    [SerializeField] private List<Vector2> _steps = new();
    [SerializeField] private List<Vector3> _noPenaltySteps = new();

    public List<Transform> TotalPoints => _totalPoints;
    [SerializeField] private List<Transform> _totalPoints;
    private List<Transform> _reachedPoints = new();

    [SerializeField] private GameObject scoreUI;
    [SerializeField] private TMP_Text scoreTMP;

    private Dictionary<Collider2D, float> _pointDistances = new();

    [SerializeField]
    private float _minRange;
    private float _notReachedDistance = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic("StandardMusic");
    }

    public void CheckDistance(Collider2D col, Vector2 playerPos)
    {
        float dis = GetPlayerPointDistance(col, playerPos);

        //  first time triggering point
        if (IsFirstVisit(col)) AddPointAsEntry(col, dis);
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
        int penalty = _steps.Count - _noPenaltySteps.Count;

        if (ReachedMinHalfOfAllPoints(MissedPoints)) penalty *= 3;

        if (penalty > 30) penalty = 30;

        float maxScore = 100f;
        float maxAccuracy = 1f;

        AddMissedPointsDistances();

        float score = Mathf.Clamp01(1f - TotalAccuracy / maxAccuracy) * maxScore - penalty;

        if (score < 0f) score = 0f;
        if (score > 100f) score = 100f;

        return score;
    }

    private void AddMissedPointsDistances()
    {
        //  for jeden missedPoint, adde distance von 0,5 zu _distances
        //  brauch dafür aber auch alle nicht erreichten punkte

        //  check all totalPoints
        for (int i = 0; i < _totalPoints.Count; i++)
        {
            Collider2D curCol = _totalPoints[i].gameObject.GetComponent<Collider2D>();

            //  point has not been visited
            if (!_pointDistances.ContainsKey(curCol))
            {
                //  add point with distance for not reached points
                _pointDistances.Add(curCol, _notReachedDistance);
            }
        }
    }

    public void ShowScore()
    {
        scoreUI.gameObject.SetActive(true);
        scoreTMP.text = $"Score: {(int)GetScore()} / 100";
    }

    public void AddFootStep(Vector2 point) { _steps.Add(point); }
    public void AddOutsideOutlineStep(Vector2 point)
    {
        if (!_noPenaltySteps.Contains(point)) _noPenaltySteps.Add(point);
    }
    public void ResetPoints() => _totalPoints.Clear();
    public void ResetFootSteps() => _steps.Clear();
    public void AddPoint(Transform point) => _totalPoints.Add(point);

    private void AddPointAsEntry(Collider2D point, float distance)
    {
        _pointDistances.Add(point, distance);
        _reachedPoints.Add(point.transform);
    }

    public bool IsCollidingWithPlayer(Collider2D collider) => _player.IsCurrentCollider(collider);
    public bool ReachedMinHalfOfAllPoints(int missedPoints) => missedPoints > (_totalPoints.Count / 2);
    public bool IsPointRegistered(Vector2 point) => _steps.Contains(point);
    public bool AllPointsReached => _pointDistances.Count >= _totalPoints.Count;
    public bool IsFirstVisit(Collider2D col) => !_pointDistances.ContainsKey(col);
    public bool IsInMinRange(float dis) => dis < _minRange;

    public float TotalDistance => _pointDistances.Values.Sum();
    public float TotalAccuracy => TotalDistance / _totalPoints.Count;
    public float BestDistance => 0;

    private float GetDistance(Collider2D col, Vector2 playerPos) => Vector2.Distance(col.transform.position, playerPos);
    private float GetLastDistance(Collider2D col) => _pointDistances.GetValueOrDefault(col);
    private float GetShortestDistance(float lastDis, float newDis) => Mathf.Min(lastDis, newDis);

    public int MissedPoints => (_totalPoints.Count - _pointDistances.Count) * 2;

    //  used by returnButton
    public void ReturnToLevelSelection()
    {
        LevelController.Instance.FinishLevel(_totalPoints, GetScore());
    }
}
