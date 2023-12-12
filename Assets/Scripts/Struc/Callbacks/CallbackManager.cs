using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackManager : Singleton<CallbackManager>
{
    public Action OnLoadMainMenu;
    public Action OnLoadLevelSelection;
    //  for every script, that is required for preparing/starting level
    public Action<LevelData> OnLoadLevel;
    public Action<List<Vector2>, List<Vector2>> OnExitLevel;

    public Action<Dictionary<int, LevelData>> OnShowLevelSelection;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}