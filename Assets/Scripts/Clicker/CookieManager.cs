using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEditor.PackageManager.UI;

public class CookieManager : MonoBehaviour
{
    public static CookieManager instance;

    public GameObject MainGameCanvas;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private TextMeshProUGUI _cookieCountText;
    [SerializeField] private TextMeshProUGUI _cookiesPerSecondText;
    [SerializeField] private GameObject _cookieObj;
    public GameObject CookieTextPopup;
    [SerializeField] private GameObject _backgroundObj;
    [HideInInspector] public bool isSettingsOpen = false; // ���� ��� ������������ ��������� ����


    [Space]
    [SerializeField] private OpenCloseSettings _openCloseSettings;
    [SerializeField] private OpenCloseAdWindow _openCloseAdWindow;

    private Animator animator;

    [Space]
    public CookieUpgrade[] CookieUpgrades;
    [SerializeField] private GameObject _upgradeUIToSpawn;
    [SerializeField] private Transform _upgradeUIParent;
    public GameObject CookiesPerSecondObjToSpawn;

    public double CurrentCookieCount { get; set; }
    public double CurrentCookiesPerSecond { get; set; }

    // upgrades
    public double CookiesPerClickUpgrade { get; set; }

    private InitializeUpgrades _initializeUpgrades;
    private CookieDisplay _cookieDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _cookieDisplay = GetComponent<CookieDisplay>();

        UpdateCookieUI();
        UpdateCookiesPerSecondUI();

        _upgradeCanvas.SetActive(false);
        MainGameCanvas.SetActive(true);

        _initializeUpgrades = GetComponent<InitializeUpgrades>();
        _initializeUpgrades.Initialize(CookieUpgrades, _upgradeUIToSpawn, _upgradeUIParent);
    }

    private void Start()
    {
        animator = _upgradeCanvas.GetComponent<Animator>();
    }

    #region On Clicker

    public void OnCookieClicker()
    {
        IncreaseCookie();

        // �������� �������
        _cookieObj.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(CookiesScaleBack);
        _backgroundObj.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(BackgroundScaleBack);

        // �������� RectTransform �������, �� �������� �������
        RectTransform rectTransform = _cookieObj.GetComponent<RectTransform>();

        // ��������� ��������� ����� ������ RectTransform
        Vector2 randomPoint = new Vector2(
            Random.Range(rectTransform.rect.xMin, rectTransform.rect.xMax), // ��������� ����� �� ������
            Random.Range(rectTransform.rect.yMin, rectTransform.rect.yMax)  // ��������� ����� �� ������
        );

        // ����������� ��������� ����� � ������� ����������
        Vector3 worldPoint = rectTransform.TransformPoint(randomPoint);

        // ������� ����������� �����
        PopupText.Create(1 + CookiesPerClickUpgrade, worldPoint);
    }

    private void CookiesScaleBack()
    {
        _cookieObj.transform.DOBlendableScaleBy(new Vector3(-0.05f, -0.05f, -0.05f), 0.05f);
    }

    private void BackgroundScaleBack()
    {
        _backgroundObj.transform.DOBlendableScaleBy(new Vector3(-0.05f, -0.05f, -0.05f), 0.05f);
    }

    public void IncreaseCookie()
    {
        CurrentCookieCount += 1 + CookiesPerClickUpgrade;
        UpdateCookieUI();
    }

    #endregion

    #region UI Updates

    private void UpdateCookieUI()
    {
        //_cookieCountText.text = CurrentCookieCount.ToString();
        _cookieDisplay.UpdateCookieText(CurrentCookieCount, _cookieCountText);
    }

    private void UpdateCookiesPerSecondUI()
    {
        //_cookieCountText.text = CurrentCookiesPerSecond.ToString() + " �/�";
        _cookieDisplay.UpdateCookieText(CurrentCookiesPerSecond, _cookiesPerSecondText, " M/C");
    }

    #endregion

    #region Button Presses

    public void OnUpgradeButtonPress()
    {
        //MainGameCanvas.SetActive(false);
        _upgradeCanvas.SetActive(true);

        if( _openCloseAdWindow.isSettingsOpen)
        {
            _openCloseAdWindow.OnResumeButtonPress();
        }
        if(_openCloseSettings.isSettingsOpen)
        {
            _openCloseSettings.OnResumeButtonPress();
        }

        isSettingsOpen = true;
        animator.SetTrigger("Open");
    }

    public void OnResumeButtonPress()
    {
        isSettingsOpen = false;
        //MainGameCanvas.SetActive(true);
        animator.SetTrigger("Close");
    }

    /*public void OnCloseAnimationEnd()
    {
        _upgradeCanvas.SetActive(false); // ��������� ����
    }*/


    #endregion

    #region Simple Increases

    public void SimpleCookieIncrease(double amount)
    {
        CurrentCookieCount += amount;
        UpdateCookieUI();
    }

    public void SimpleCookiePerSecondIncrease(double amount)
    {
        CurrentCookiesPerSecond += amount;
        UpdateCookiesPerSecondUI();
    }

    #endregion

    #region Events

    public void OnUpgradeButtonClick(CookieUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        if (CurrentCookieCount >= upgrade.CurrentUpgradeCost)
        {
            // ��������� ���������
            upgrade.ApplyUpgrade();

            // ����������� ������� � ����� UpgradeAmount ��� ���������� ������
            buttonRef.UpdateUpgradeLevel();
            buttonRef.UpdateTotalUpgradeAmount(upgrade.UpgradeAmount);

            // ��������� ������� ���������� �������
            CurrentCookieCount -= upgrade.CurrentUpgradeCost;

            // ��������� ���������������� ���������
            UpdateCookieUI();

            // ������������� ��������� ���������� ���������
            upgrade.CurrentUpgradeCost = Mathf.Round(
                (float)(upgrade.CurrentUpgradeCost * (1 + upgrade.CostIncreaseMultiplierPerPurchase))
            );

            // ��������� ����� �� ������
            buttonRef.UpgradeCostText.text = "Cost: " + upgrade.CurrentUpgradeCost;
        }
    }


    #endregion
}
