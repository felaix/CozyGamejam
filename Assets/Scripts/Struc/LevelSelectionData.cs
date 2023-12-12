using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionData : Singleton<LevelSelectionData>
{
    public LevelData CurLevelData => LevelDatas[CurLevelIndex];

    public List<ScriptableTier> AvailableTiers => _availableTiers;
    [SerializeField]
    private List<ScriptableTier> _availableTiers = new();
    public List<ScriptableTier> UnlockedTiers { get; private set; } = new();

    public int CurTierIndex;

    public Dictionary<int, LevelData> LevelDatas = new();
    public int CurLevelIndex = 0; // set on level load
    public int TotalScore;

    //  --- CONDITIONS ---
    private bool PlayedLevelBefore(LevelData data) => data.AvailablePoints.Count > 0;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        CurTierIndex = -1;

        CallbackManager callbacks = CallbackManager.Instance;
        callbacks.OnLoadLevel += SetActiveLevel;
        callbacks.OnExitLevel += UpdateLevelData;
    }

    private void SetActiveLevel(LevelData data) => CurLevelIndex = data.Index;

    private void UpdateLevelData(List<Vector2> totalPoints, float accuracy)
    {
        if (!PlayedLevelBefore(CurLevelData)) CurLevelData.AddAvailablePoints(totalPoints);
        CurLevelData.UpdateReachedPoints(accuracy);
    }
}

public class LevelData
{
    public Sprite Image { get; set; }
    public int Index { get; set; }
    public List<Vector2> AvailablePoints { get; set; } = new();
    public float Accuracy;

    public List<Vector2> ReachedPoints { get; set; } = new();

    //  CurScore aus Accuracy und maxScore berechnen
    public int CurScore;
    private int _maxScore = 100;

    public void AddImage(Sprite img) => Image = img;
    public void AddIndex(int index) => Index = index;

    //  call, when return from first levelStart
    public void AddAvailablePoints(List<Vector2> points) => AvailablePoints = points;

    //  call each time, exiting level
    public void UpdateReachedPoints(float accuracy)
    {
        Accuracy = accuracy;
    }
}