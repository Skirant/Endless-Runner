using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using YG;

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

    public double RewardMultiplier { get; private set; } = 1.0;

    private double passiveIncomePerSecond = 0.0; // ���� �� ���������� ������
    private double passiveIncomeAccumulated = 0.0; // ����������� ������� ��� �������� �������������

    private GameObject activeCookiesPerSecondObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _cookieDisplay = GetComponent<CookieDisplay>();

        // �������� ���������� �������
        CurrentCookieCount = YG2.saves.savedCookieCount;
        CurrentCookiesPerSecond = YG2.saves.savedCookiesPerSecond;

        // Debug.Log("Initializing Cookie Manager...");
        UpdateCookieUI();
        UpdateCookiesPerSecondUI();

        _upgradeCanvas.SetActive(false);
        MainGameCanvas.SetActive(true);

        _initializeUpgrades = GetComponent<InitializeUpgrades>();
        _initializeUpgrades.Initialize(CookieUpgrades, _upgradeUIToSpawn, _upgradeUIParent);

        //���������� ����� ������
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
    }

    private void Start()
    {
        // ��������� ���������� ��������
        GetComponent<InitializeUpgrades>().ApplyLoadedUpgrades();

        // ������ ��������
        animator = _upgradeCanvas.GetComponent<Animator>();
    }

    private void Update()
    {
        DistributePassiveIncome(Time.deltaTime);
       // Debug.Log($"Passive Income Per Second: {passiveIncomePerSecond}");
       // Debug.Log($"Passive Income Accumulated: {passiveIncomeAccumulated}");
        UpdateCookieUI();
    }

    private void DistributePassiveIncome(float deltaTime)
    {
        // ������������ ����, ����������� �� ����
        double incomeThisFrame = passiveIncomePerSecond * deltaTime;
       // Debug.Log($"Income This Frame: {incomeThisFrame}");
        passiveIncomeAccumulated += incomeThisFrame;

        // ��������� ������ ����� �����, ������� ��������� ��� ��������� ������
        int wholePointsToAdd = Mathf.FloorToInt((float)passiveIncomeAccumulated);
        //Debug.Log($"Whole Points To Add: {wholePointsToAdd}");
        CurrentCookieCount += wholePointsToAdd;
        passiveIncomeAccumulated -= wholePointsToAdd; // ������� ����������� �� ������������
    }

    #region On Clicker

    public void OnCookieClicker()
    {
       // Debug.Log("Cookie clicked!");
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
        PopupText.Create((1 + CookiesPerClickUpgrade) * RewardMultiplier, worldPoint);
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
        CurrentCookieCount += (1 + CookiesPerClickUpgrade) * RewardMultiplier;
        YG2.saves.savedCookieCount = CurrentCookieCount; // ��������� ��������
        YG2.SaveProgress(); // �������� ����������
        UpdateCookieUI();
    }

    public IEnumerator ActivateDoubleReward(float duration)
    {
        RewardMultiplier = 2.0; // ������������� ��������� �� 2
       // Debug.Log("Double reward activated!");
        yield return new WaitForSeconds(duration); // ��� ��������� �����
        RewardMultiplier = 1.0; // ���������� ��������� � 1
       // Debug.Log("Double reward deactivated.");
    }

    #endregion

    #region UI Updates

    private void UpdateCookieUI()
    {
        //_cookieCountText.text = CurrentCookieCount.ToString();
        //Debug.Log($"Updating Cookie UI: {CurrentCookieCount}");
        _cookieDisplay.UpdateCookieText(CurrentCookieCount, _cookieCountText);
    }

    private void UpdateCookiesPerSecondUI()
    {
        // Debug.Log($"Updating Cookies Per Second UI: {passiveIncomePerSecond}");
        _cookieDisplay.UpdateCookieText(CurrentCookiesPerSecond, _cookiesPerSecondText, "");
    }
    /*private void UpdateCookiesPerSecondUI()
    {
        _cookieDisplay.UpdateCookieText(CurrentCookiesPerSecond, _cookiesPerSecondText, " CPS");
    }*/

    #endregion

    #region Button Presses

    public void OnUpgradeButtonPress()
    {
       // Debug.Log("Upgrade button pressed!");
        //MainGameCanvas.SetActive(false);
        _upgradeCanvas.SetActive(true);

        if (_openCloseAdWindow.isSettingsOpen)
        {
            _openCloseAdWindow.OnResumeButtonPress();
        }
        if (_openCloseSettings.isSettingsOpen)
        {
            _openCloseSettings.OnResumeButtonPress();
        }

        isSettingsOpen = true;
        animator.SetTrigger("Open");
        FindAnyObjectByType<AudioManager>().Play("Open");
    }

    public void OnResumeButtonPress()
    {
        //Debug.Log("Resume button pressed!");
        isSettingsOpen = false;
        //MainGameCanvas.SetActive(true);
        animator.SetTrigger("Close");
        FindAnyObjectByType<AudioManager>().Play("Open");
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
        YG2.saves.savedCookieCount = CurrentCookieCount; // ��������� ��������
        YG2.SaveProgress();
        UpdateCookieUI();
    }

    public void SimpleCookiePerSecondIncrease(double amount)
    {
        passiveIncomePerSecond += amount; // ����������� ��������� �����
        CurrentCookiesPerSecond = passiveIncomePerSecond; // ��������� ������� �������� CPS

        YG2.saves.savedCookiesPerSecond = CurrentCookiesPerSecond; // ��������� CPS
        YG2.SaveProgress(); // ��������� ���������

        UpdateCookiesPerSecondUI();

        if (activeCookiesPerSecondObj == null)
        {
            activeCookiesPerSecondObj = Instantiate(CookiesPerSecondObjToSpawn, MainGameCanvas.transform);
            activeCookiesPerSecondObj.transform.SetParent(MainGameCanvas.transform, false);
        }
    }

    #endregion

    #region Events

    public void OnUpgradeButtonClick(CookieUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        if (CurrentCookieCount >= upgrade.CurrentUpgradeCost)
        {
            // ��������� ���������
            upgrade.ApplyUpgrade();

            // ����������� ������� ��������� � ������
            upgrade.UpgradeLevel++;

            // ������������� UI � �������
            buttonRef.UpdateUpgradeLevel(upgrade.UpgradeLevel);
            buttonRef.UpdateTotalUpgradeAmount(upgrade.UpgradeAmount);

            // �������� ��������� �� �������� ���������� �������
            CurrentCookieCount -= upgrade.CurrentUpgradeCost;

            // ��������� UI
            UpdateCookieUI();

            // ���������� ���������
            int index = System.Array.IndexOf(CookieUpgrades, upgrade);
            YG2.saves.savedUpgradeLevels[index] = upgrade.UpgradeLevel;
            YG2.saves.savedUpgradeCosts[index] = upgrade.CurrentUpgradeCost;
            YG2.SaveProgress();

            // ��������� ��������� ���������� ���������
            upgrade.CurrentUpgradeCost = Mathf.Round(
                (float)(upgrade.CurrentUpgradeCost * (1 + upgrade.CostIncreaseMultiplierPerPurchase))
            );

            buttonRef.UpgradeCostText.text = "" + upgrade.CurrentUpgradeCost;
        }
    }


    #endregion
}
