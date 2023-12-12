using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LUCYOutlineManager : MonoBehaviour
{
    public static LUCYOutlineManager Instance { get; private set; }

    //[SerializeField] private float multiplyAmount;

    [SerializeField] private List<ScriptableImage> outlines;

    [SerializeField] private TMP_Text pointsTxt;
    [SerializeField] private int sampleRate;

    [SerializeField] private List<Vector2> footpoints;
    [SerializeField] private List<Vector2> points;

    private PolygonCollider2D outlineCollider;

    private bool compared = false;

    //  --- LUCY VARIABLES ---
    //  reachedPoints ist List<Vector2>, damit man bei neuem level-besuch diese bereits erledigt hat
    [SerializeField] private List<Vector2> _reachedPoints = new();
    private List<float> _distances = new();
    
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

    public void LUCYAddFootStep(Collider stepArea)
    {
        //  player bekommt triggerCollider
        //  points bekommen tag
        //  während playerTriggerCollider mit pointTag-Objekt collidiert,
        //      prüfe distanz zwischen player und pointTag-Objekt in if-loop(?)
        //      speichere kürzeste distanz zwischen
        //  wenn der playerTriggerCollider nicht mehr mit einem pointTag-Objekt collidiert
        //      ODER
        //  wenn der playerTriggerCollider mit einem NEUEN pointTag-Objekt collidert
        //      speichere den besuchten Punkt als lastPoint-Variable
        //      füge lastPoint der Liste reachedPoints hinzu
        //      füge kürzeste Distanz zu _distances hinzu;

        //  wenn reachedpoints.count >= points.count
        //      berechne TotalAccuracy, beende anschließend das level
        
        //  berechne TotalAccuracy NUR bei oben genannter Bedingung ODER, wenn der Button zum beenden geklickt wird
        //  => IMPORTANT: statt reachedPoints, gebe reachedAccuracy zurück

        //  TotalAccuracy Berechnung:
        //      methoden-interne variable "curAccuracy"
        //      berechne curAccuracy
        //      => if (curAccuracy > reachedAccuracy) reachedAccuracy = curAccuracy;
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
