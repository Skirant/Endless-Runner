using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI percentageText;

    [Header("Game Settings")]
    [SerializeField] private double targetPoints = 1000000.0; // Цель для завершения игры

    private CookieManager cookieManager;
    private double totalEarnedPoints = 0.0; // Счетчик для общего количества заработанных очков
    private double previousCookieCount = 0.0; // Счетчик предыдущих очков для отслеживания прироста

    [Space]
    [SerializeField] private GameObject _win;

    void Start()
    {
        // Получаем ссылку на CookieManager
        cookieManager = CookieManager.instance;
        UpdateUI();
    }

    void Update()
    {
        TrackTotalEarnedPoints();
        UpdateUI();
    }

    private void TrackTotalEarnedPoints()
    {
        // Вычисляем прирост очков
        double currentCookieCount = cookieManager.CurrentCookieCount;
        if (currentCookieCount > previousCookieCount)
        {
            totalEarnedPoints += currentCookieCount - previousCookieCount;
        }
        previousCookieCount = currentCookieCount;
    }

    private void UpdateUI()
    {
        // Обновляем слайдер на основе общего заработанного количества очков
        progressSlider.value = (float)(totalEarnedPoints / targetPoints) * 100;

        // Обновляем текст с процентами
        double percentage = (totalEarnedPoints / targetPoints) * 100.0;
        percentageText.text = percentage.ToString("F4") + "%";

        if(progressSlider.value >= 100)
        {
            _win.SetActive(true);
        }
    }
}
