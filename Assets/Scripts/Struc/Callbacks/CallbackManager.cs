using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CallbackManager : Singleton<CallbackManager>
{
    public Action OnLoadMainMenu;
    public Action OnLoadLevelSelection;
    //  for every script, that is required for preparing/starting level
    public Action<LevelData> OnLoadLevel;
    public Action<LevelData> OnLevelReset;
    public Action<List<Vector2>, float> OnExitLevel;

    public Action<List<LevelData>> OnShowLevelSelection;

    protected override void Awake()
    {
        if (CallbackManager.Instance != null && CallbackManager.Instance != this)
        {
            DestroyImmediate(CallbackManager.Instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}