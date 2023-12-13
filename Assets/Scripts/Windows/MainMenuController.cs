using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : SingletonWindow<MainMenuController>
{
    [SerializeField]
    private GameObject _eventSystemRef;
    private EventSystem _curSystem => EventSystem.current;
    //  --- REFERENCES ---
    private GameManager GameManager => GameManager.Instance;
    private CallbackManager Callbacks => CallbackManager.Instance;


    //  --- VARIABLES ---
    [SerializeField]
    private Button _startBtn;
    [SerializeField]
    private Button _quitBtn;


    //  --- SCRIPT METHODS ---

    protected override void Awake()
    {
        base.Awake();

        if (_curSystem == null) Instantiate(_eventSystemRef);

        //  NullCheck();

        base.ShowWindow();
        SetButtons();

        //  Callbacks.OnLoadMainMenu += ShowWindow;
    }

    public override void ShowWindow()
    {
        if (_curSystem == null) Instantiate(_eventSystemRef);

        //  NullCheck();

        base.ShowWindow();
        SetButtons();
    }

    private void NullCheck()
    {
        if (_startBtn == null || _quitBtn == null) Debug.LogError("ERROR : MainMenu Buttons not set!"); return;
        
    }

    private void SetButtons()
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() =>
        {
            GameManager.LoadScene(1);
        });

        _quitBtn.onClick.RemoveAllListeners();
        _quitBtn.onClick.AddListener(() =>
        {
            GameManager.QuitGame();
        });
    }
}