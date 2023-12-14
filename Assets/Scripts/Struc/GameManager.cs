using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int _frameRate = 60;

    private CallbackManager _callbacks => CallbackManager.Instance;

    public bool StartWithMenu = true;

    public bool IsMainMenu(int index) => index == 0;
    public bool IsLevelSelection(int index) => index == 1;
    public bool IsLevel(int index) => index != 0 && index != 1;

    protected override void Awake()
    {
        if (GameManager.Instance != null && GameManager.Instance != this)
        {
            DestroyImmediate(GameManager.Instance.gameObject);
            //  return;
        }

        DontDestroyOnLoad(gameObject);
        SetFrameRate(_frameRate);

        _callbacks.OnExitLevel += ExitLevel;
    }

    private void ExitLevel(List<Transform> totalPoints, float score)
    {
        LoadScene(1);
    }

    private void Start()
    {
        if (StartWithMenu) LoadScene(0);
    }

    private void SetFrameRate(int frameRate)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }

    public void LoadScene(int sceneID)
    {
        UnityEngine.SceneManagement.Scene curScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (curScene.buildIndex != sceneID)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID); //  call on MainMenu Press Play
        }
        if (IsMainMenu(sceneID)) _callbacks.OnLoadMainMenu?.Invoke();
        else if (IsLevelSelection(sceneID)) _callbacks.OnLoadLevelSelection?.Invoke();
    }

    public void LoadLevel(LevelData data)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        CallbackManager.Instance.OnLoadLevel?.Invoke(data);
    }

    public void QuitGame() { Application.Quit(); }
    public void PauseGame(bool pause)   { if (pause) Time.timeScale = 0f; else Time.timeScale = 1f; }


    private void OnApplicationPause(bool pause) { } // wenn der spieler pausiert ist
    public void OnApplicationQuit() { } //  message an objekte, bevor game beendet wird
}