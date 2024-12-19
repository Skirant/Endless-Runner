using UnityEngine;

public class OpenCloseAdWindow : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _adButton;
    [SerializeField] private GameObject _adCanvas;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private GameObject _settingCanvas;
    [HideInInspector] public bool isSettingsOpen = false; // Флаг для отслеживания состояния окна

    [Space]
    [SerializeField] private OpenCloseSettings _openCloseSettings;
    [SerializeField] private CookieManager _сookieManager;

    private void Start()
    {
        _animator = _adCanvas.GetComponent<Animator>();
        _adCanvas.SetActive(false);
    }

    public void OnUpgradeButtonPress()
    {
        if (!isSettingsOpen)
        {
            isSettingsOpen = true; // Устанавливаем флаг
            _adCanvas.SetActive(true);
            //_adButton.SetActive(false);
            _animator.SetTrigger("Open");

            if (_openCloseSettings.isSettingsOpen)
            {
                _openCloseSettings.OnResumeButtonPress();
            }
            if (_сookieManager.isSettingsOpen)
            {
                _сookieManager.OnResumeButtonPress();
            }
        }
        else
        {
            isSettingsOpen = false; // Сбрасываем флаг
            _adButton.SetActive(true);
            _animator.SetTrigger("Close");
        }

        FindAnyObjectByType<AudioManager>().Play("Open");
    }

    public void OnResumeButtonPress()
    {
        if (isSettingsOpen)
        {
            isSettingsOpen = false; // Сбрасываем флаг
            _adButton.SetActive(true);
            _animator.SetTrigger("Close");
        }
        FindAnyObjectByType<AudioManager>().Play("Open");
    }

    public void OnCloseAnimationEnd()
    {
        _adCanvas.SetActive(false); // Выключаем окно
    }
}
