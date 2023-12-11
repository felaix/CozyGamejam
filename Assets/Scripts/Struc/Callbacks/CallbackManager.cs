using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackManager : Singleton<CallbackManager>
{
    public Action OnLoadMainMenu;
    public Action OnLoadLevel;

    public Action<LevelController> OnStartLevel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}