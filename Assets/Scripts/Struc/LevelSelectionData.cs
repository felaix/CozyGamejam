using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionData : Singleton<LevelSelectionData>
{
    public bool HasSeenTutorial = false;
    public LevelData CurLevelData => LevelDatas[CurLevelIndex];

    public bool HasNextTierInRange => NextTierIndex < AvailableTiers.Count;
    public List<ScriptableTier> AvailableTiers => _availableTiers;
    [SerializeField]
    private List<ScriptableTier> _availableTiers = new();
    public List<ScriptableTier> UnlockedTiers { get; private set; } = new();
    public List<LevelData> ActiveLevelDatas { get; private set; } = new();

    public int CurTierIndex;
    public int NextTierIndex => CurTierIndex + 1;

    public Dictionary<int, LevelData> LevelDatas = new();
    public int CurLevelIndex = 0; // set on level load
    public float TotalScore;

    //  --- CONDITIONS ---
    private bool PlayedLevelBefore(LevelData data) => data.AvailablePoints.Count > 0;
    
    protected override void Awake()
    {
        if (LevelSelectionData.Instance != null && LevelSelectionData.Instance != this) Destroy(LevelSelectionData.Instance.gameObject);
        DontDestroyOnLoad(this);
        CurTierIndex = -1;

        CallbackManager callbacks = CallbackManager.Instance;
        callbacks.OnLoadLevel += SetActiveLevel;
        callbacks.OnExitLevel += UpdateLevelData;
        callbacks.OnLevelReset += ResetLevel;
    }

    private void SetActiveLevel(LevelData data) => CurLevelIndex = data.Index;
    public void AddToLevelList(LevelData data) => ActiveLevelDatas.Add(data);

    private void UpdateLevelData(List<Vector2> totalPoints, float accuracy)
    {
        if (!PlayedLevelBefore(CurLevelData)) CurLevelData.AddAvailablePoints(totalPoints);
        CurLevelData.UpdateReachedPoints(accuracy);
    }

    private void ResetLevel(LevelData data)
    {
        Debug.Log("RESET LEVEL");
        data.Score = 0;
    }
}

public class LevelData
{
    public Sprite ImageToCode { get; set; }
    public Sprite ImageToDisplay { get; set; }
    public int Index { get; set; }
    public List<Vector2> AvailablePoints { get; set; } = new();
    public float Score;

    public void AddImageToCode(Sprite img) => ImageToCode = img;
    public void AddImageToDisplay(Sprite img) => ImageToDisplay = img;
    public void AddIndex(int index) => Index = index;

    //  call, when return from first levelStart
    public void AddAvailablePoints(List<Vector2> points) => AvailablePoints = points;

    //  call each time, exiting level
    public void UpdateReachedPoints(float score)
    {
        Score = score;
    }
}