using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI percentageText;

    [Header("Game Settings")]
    [SerializeField] private double targetPoints = 1000000.0; // ���� ��� ���������� ����

    private CookieManager cookieManager;
    private double totalEarnedPoints = 0.0; // ������� ��� ������ ���������� ������������ �����
    private double previousCookieCount = 0.0; // ������� ���������� ����� ��� ������������ ��������

    [Space]
    [SerializeField] private GameObject _win;

    void Start()
    {
        // �������� ������ �� CookieManager
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
        // ��������� ������� �����
        double currentCookieCount = cookieManager.CurrentCookieCount;
        if (currentCookieCount > previousCookieCount)
        {
            totalEarnedPoints += currentCookieCount - previousCookieCount;
        }
        previousCookieCount = currentCookieCount;
    }

    private void UpdateUI()
    {
        // ��������� ������� �� ������ ������ ������������� ���������� �����
        progressSlider.value = (float)(totalEarnedPoints / targetPoints) * 100;

        // ��������� ����� � ����������
        double percentage = (totalEarnedPoints / targetPoints) * 100.0;
        percentageText.text = percentage.ToString("F4") + "%";

        if(progressSlider.value >= 100)
        {
            _win.SetActive(true);
        }
    }
}
