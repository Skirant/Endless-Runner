using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEditor.PackageManager.UI;
using System.Collections;

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
    [HideInInspector] public bool isSettingsOpen = false; // Флаг для отслеживания состояния окна


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

    private double passiveIncomePerSecond = 0.0; // Очки от пассивного дохода
    private double passiveIncomeAccumulated = 0.0; // Накопленный остаток для плавного распределения

    private GameObject activeCookiesPerSecondObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _cookieDisplay = GetComponent<CookieDisplay>();

        // Debug.Log("Initializing Cookie Manager...");
        UpdateCookieUI();
        UpdateCookiesPerSecondUI();

        _upgradeCanvas.SetActive(false);
        MainGameCanvas.SetActive(true);

        _initializeUpgrades = GetComponent<InitializeUpgrades>();
        _initializeUpgrades.Initialize(CookieUpgrades, _upgradeUIToSpawn, _upgradeUIParent);

        //Отключение дебаг панели
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
    }

    private void Start()
    {
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
        // Рассчитываем очки, добавляемые за кадр
        double incomeThisFrame = passiveIncomePerSecond * deltaTime;
       // Debug.Log($"Income This Frame: {incomeThisFrame}");
        passiveIncomeAccumulated += incomeThisFrame;

        // Добавляем только целую часть, остаток сохраняем для следующих кадров
        int wholePointsToAdd = Mathf.FloorToInt((float)passiveIncomeAccumulated);
        //Debug.Log($"Whole Points To Add: {wholePointsToAdd}");
        CurrentCookieCount += wholePointsToAdd;
        passiveIncomeAccumulated -= wholePointsToAdd; // Убираем добавленное из накопленного
    }

    #region On Clicker

    public void OnCookieClicker()
    {
       // Debug.Log("Cookie clicked!");
        IncreaseCookie();

        // Анимация нажатия
        _cookieObj.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(CookiesScaleBack);
        _backgroundObj.transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0.05f), 0.05f).OnComplete(BackgroundScaleBack);

        // Получаем RectTransform объекта, по которому спавним
        RectTransform rectTransform = _cookieObj.GetComponent<RectTransform>();

        // Вычисляем случайную точку внутри RectTransform
        Vector2 randomPoint = new Vector2(
            Random.Range(rectTransform.rect.xMin, rectTransform.rect.xMax), // Случайная точка по ширине
            Random.Range(rectTransform.rect.yMin, rectTransform.rect.yMax)  // Случайная точка по высоте
        );

        // Преобразуем локальную точку в мировые координаты
        Vector3 worldPoint = rectTransform.TransformPoint(randomPoint);

        // Создаем всплывающий текст
        PopupText.Create(1 + CookiesPerClickUpgrade * RewardMultiplier, worldPoint);
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
       // Debug.Log($"Current Cookie Count: {CurrentCookieCount}");
        UpdateCookieUI();
    }

    public IEnumerator ActivateDoubleReward(float duration)
    {
        RewardMultiplier = 2.0; // Устанавливаем множитель на 2
       // Debug.Log("Double reward activated!");
        yield return new WaitForSeconds(duration); // Ждём указанное время
        RewardMultiplier = 1.0; // Возвращаем множитель к 1
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
        _cookieDisplay.UpdateCookieText(passiveIncomePerSecond, _cookiesPerSecondText, " M/C");
    }

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
    }

    public void OnResumeButtonPress()
    {
        Debug.Log("Resume button pressed!");
        isSettingsOpen = false;
        //MainGameCanvas.SetActive(true);
        animator.SetTrigger("Close");
    }

    /*public void OnCloseAnimationEnd()
    {
        _upgradeCanvas.SetActive(false); // Выключаем окно
    }*/


    #endregion

    #region Simple Increases

    public void SimpleCookieIncrease(double amount)
    {
        Debug.Log($"Simple Cookie Increase: {amount}");
        CurrentCookieCount += amount;
        UpdateCookieUI();
    }

    public void SimpleCookiePerSecondIncrease(double amount)
    {
        Debug.Log($"Increasing Passive Income Per Second: {amount}");
        passiveIncomePerSecond += amount; // Увеличиваем пассивный доход
        UpdateCookiesPerSecondUI();

        if (activeCookiesPerSecondObj == null)
        {
            Debug.Log("Creating CookiesPerSecond object.");
            activeCookiesPerSecondObj = Instantiate(CookiesPerSecondObjToSpawn, MainGameCanvas.transform);
            activeCookiesPerSecondObj.transform.SetParent(MainGameCanvas.transform, false); // Устанавливаем родителя и сохраняем локальные координаты
        }
        else
        {
            Debug.Log("CookiesPerSecond object already exists. Skipping creation.");
        }
    }

    #endregion

    #region Events

    public void OnUpgradeButtonClick(CookieUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        Debug.Log($"Upgrade button clicked for: {upgrade.name}");
        if (CurrentCookieCount >= upgrade.CurrentUpgradeCost)
        {
            // Применяем улучшение
            upgrade.ApplyUpgrade();

            // Увеличиваем уровень и сумму UpgradeAmount для конкретной кнопки
            buttonRef.UpdateUpgradeLevel();
            buttonRef.UpdateTotalUpgradeAmount(upgrade.UpgradeAmount);

            // Уменьшаем текущее количество печенек
            CurrentCookieCount -= upgrade.CurrentUpgradeCost;
            Debug.Log($"Upgrade applied. Current Cookie Count: {CurrentCookieCount}");

            // Обновляем пользовательский интерфейс
            UpdateCookieUI();

            // Пересчитываем стоимость следующего улучшения
            upgrade.CurrentUpgradeCost = Mathf.Round(
                (float)(upgrade.CurrentUpgradeCost * (1 + upgrade.CostIncreaseMultiplierPerPurchase))
            );

            // Обновляем текст на кнопке
            buttonRef.UpgradeCostText.text = "Cost: " + upgrade.CurrentUpgradeCost;
        }
        else
        {
            Debug.Log("Not enough cookies for upgrade.");
        }
    }


    #endregion
}
