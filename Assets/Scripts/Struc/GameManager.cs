using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int _frameRate = 60;

    private CallbackManager _callbacks => CallbackManager.Instance;

    public bool StartWithMenu = true;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        SetFrameRate(_frameRate);
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

        if (sceneID == 0) _callbacks.OnLoadMainMenu?.Invoke();
        else if (sceneID == 1) _callbacks.OnLoadLevel?.Invoke();
    }

    public void QuitGame() { Application.Quit(); }
    public void PauseGame(bool pause)   { if (pause) Time.timeScale = 0f; else Time.timeScale = 1f; }


    private void OnApplicationPause(bool pause) { } // wenn der spieler pausiert ist
    public void OnApplicationQuit() { } //  message an objekte, bevor game beendet wird
}