using UnityEngine;

public class OpenCloseSettings : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _settingCanvas;
    [SerializeField] private GameObject _adWindows;
    [SerializeField] private GameObject _upgradeCanvas;
    [HideInInspector] public bool isSettingsOpen = false; // Флаг для отслеживания состояния окна

    [Space]
    [SerializeField] private CookieManager _cookieManager;
    [SerializeField] private OpenCloseAdWindow _openCloseAdWindow;

    private void Start()
    {
        _animator = _settingCanvas.GetComponent<Animator>();
        _settingCanvas.SetActive(false);
    }

    public void OnUpgradeButtonPress()
    {
        if (!isSettingsOpen)
        {
            // Открываем настройки
            isSettingsOpen = true;
            _settingCanvas.SetActive(true);
            //_settingsButton.SetActive(false);
            _animator.SetTrigger("Open");

            if (_cookieManager.isSettingsOpen)
            {
                _cookieManager.OnResumeButtonPress();
            }

            if (_openCloseAdWindow.isSettingsOpen)
            {
                _openCloseAdWindow.OnResumeButtonPress();
            }
        }
        else
        {
            // Закрываем настройки
            isSettingsOpen = false;
            _animator.SetTrigger("Close");
            _settingsButton.SetActive(true);
        }
    }

    public void OnResumeButtonPress()
    {
        if (isSettingsOpen)
        {
            isSettingsOpen = false; // Сбрасываем флаг
            _settingsButton.SetActive(true);
            _animator.SetTrigger("Close");
        }
    }

    public void OnCloseAnimationEnd()
    {
        _settingCanvas.SetActive(false); // Выключаем окно
    }
}
