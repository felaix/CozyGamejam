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
    private Button _settingsBtn;
    [SerializeField]
    private Button _quitBtn;
    [SerializeField]
    private Button _closeSettingsBtn;
    [SerializeField]
    private GameObject _mainPanel;
    [SerializeField]
    private GameObject _settingsPanel;

    //  --- SCRIPT METHODS ---

    protected override void Awake()
    {
        base.Awake();

        if (_curSystem == null) Instantiate(_eventSystemRef);
        base.ShowWindow();
        SetButtons();
    }

    public override void ShowWindow()
    {
        if (_curSystem == null) Instantiate(_eventSystemRef);
        base.ShowWindow();
        SetButtons();
    }

    private void SetButtons()
    {
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(() =>
        {
            GameManager.LoadScene(1);
        });

        _settingsBtn.onClick.RemoveAllListeners();
        _settingsBtn.onClick.AddListener(() =>
        {
            _mainPanel.SetActive(false);
            _settingsPanel.SetActive(true);
        });

        _quitBtn.onClick.RemoveAllListeners();
        _quitBtn.onClick.AddListener(() =>
        {
            GameManager.QuitGame();
        });

        _closeSettingsBtn.onClick.RemoveAllListeners();
        _closeSettingsBtn.onClick.AddListener(() =>
        {
            _mainPanel.SetActive(true);
            _settingsPanel.SetActive(false);
        });
    }
}