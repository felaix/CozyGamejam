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
    //[SerializeField] private List<float> smallestDistances;
    //[SerializeField] private List<Vector2> reachedPoints;

    //private PolygonCollider2D outlineCollider;

    //private bool compared = false;
    //private bool calculated = false;

    [SerializeField] private List<(Collider2D col, float smallestDistance)> tuples = new();

    //private (Collider2D pointCol, float smallestDistance) pointTuple = (null, 100);
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    //public bool CheckCollision(Vector2 pos)
    //{
    //    return outlineCollider.OverlapPoint(pos);
    //}

    public void DoSomething(Collider2D col, Vector2 playerPos)
    {
        // ! Get the index
        // ! If the col doesn't exist, the index will be -1
        // ! When the index is -1, create a new tuple

        // ! If the index is higher then 1, it means that you already collided with the collisison
        // ! if thats the case, check if the distance is smaller than the stored distance
        // ! if the distance is smaller: overwrite the distance

        int index = tuples.FindIndex(tuple => tuple.col == col);

        //Debug.Log("col alr exist" + index);

        if (index >= 0)
        {
            float distance = Vector2.Distance(col.transform.position, playerPos);
            float oldDistance = tuples[index].smallestDistance;

            //Debug.Log($"old dist: {oldDistance}, new dist: {distance}");

            if (distance < tuples[index].smallestDistance) { tuples[index] = (col, distance); }
        }else
        {
            (Collider2D newCol, float dist) newTuple = (col, Vector2.Distance(col.transform.position, playerPos));
            tuples.Add(newTuple);
        }

        if (tuples.Count >= points.Count)
        {
            // ! All points reached -> level completed
            Debug.Log("All points reached");

            CalculateAccuracy();
        }
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
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

    public void ResetPoints()
    {
        points.Clear();
    }

    public void ResetFootSteps()
    {
        footpoints.Clear();
    }

    //public void Compare()
    //{
    //    compared = false;
    //    CompareAccuracy(footpoints, points);
    //}

    public float CalculateAccuracy()
    {
        //calculated = true;
        float totalDistance = 0;

        for (int i = 0; i < tuples.Count; i++)
        {
            totalDistance += tuples[i].smallestDistance;
        }

        Debug.Log("Total Distance: " + totalDistance);

        float averageDistance = totalDistance / tuples.Count;

        Debug.Log("Average Distance: " + averageDistance);

        LevelController.Instance.FinishLevel(points, footpoints);

        return averageDistance;
    }

    //private void CompareAccuracy(List<Vector2> playerFootsteps, List<Vector2> points)
    //{
    //    // a total value 
    //    float totalDistance = 0f;
    //    compared = true;
        
    //    for (int i = 0; i < playerFootsteps.Count-1; i++)
    //    {
    //        // get the distance of each playerfootstep and the vertice/point of the outline

    //        if (i > points.Count-1) break;
    //        if (i > footpoints.Count-1) break;

    //        float distance = Vector2.Distance(playerFootsteps[i], points[i]);

    //        // add the distance to the total distance
    //        totalDistance += distance;
    //        //Debug.Log(distance);
    //    }

    //    // get the average distance by dividing the total distance by the count of footprints
    //    float averageDistance = totalDistance / footpoints.Count;
    //    //float score = averageDistance * multiplyAmount;

    //    Debug.Log("Average Distance: " +  averageDistance);

    //    LevelController.Instance.FinishLevel(points, footpoints);

    //}

}
