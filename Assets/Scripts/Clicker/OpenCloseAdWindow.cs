using UnityEngine;

public class OpenCloseAdWindow : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _adButton;
    [SerializeField] private GameObject _adCanvas;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private GameObject _settingCanvas;
    [HideInInspector] public bool isSettingsOpen = false; // ���� ��� ������������ ��������� ����

    [Space]
    [SerializeField] private OpenCloseSettings _openCloseSettings;
    [SerializeField] private CookieManager _�ookieManager;

    private void Start()
    {
        _animator = _adCanvas.GetComponent<Animator>();
        _adCanvas.SetActive(false);
    }

    public void OnUpgradeButtonPress()
    {
        if (!isSettingsOpen)
        {
            isSettingsOpen = true; // ������������� ����
            _adCanvas.SetActive(true);
            //_adButton.SetActive(false);
            _animator.SetTrigger("Open");

            if (_openCloseSettings.isSettingsOpen)
            {
                _openCloseSettings.OnResumeButtonPress();
            }
            if (_�ookieManager.isSettingsOpen)
            {
                _�ookieManager.OnResumeButtonPress();
            }
        }
        else
        {
            isSettingsOpen = false; // ���������� ����
            _adButton.SetActive(true);
            _animator.SetTrigger("Close");
        }

        FindAnyObjectByType<AudioManager>().Play("Open");
    }

    public void OnResumeButtonPress()
    {
        if (isSettingsOpen)
        {
            isSettingsOpen = false; // ���������� ����
            _adButton.SetActive(true);
            _animator.SetTrigger("Close");
        }
        FindAnyObjectByType<AudioManager>().Play("Open");
    }

    public void OnCloseAnimationEnd()
    {
        _adCanvas.SetActive(false); // ��������� ����
    }
}
