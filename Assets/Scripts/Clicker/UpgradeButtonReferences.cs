using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonReferences : MonoBehaviour
{
    public Button UpgradeButton;
    //public TextMeshProUGUI UpgradeButtonText;
    public TextMeshProUGUI UpgradeLevelText;
    public TextMeshProUGUI UpgradeCostText;
    public TextMeshProUGUI UpgradeButtonDescription;
    public TextMeshProUGUI DamageTypeDescription;
    public TextMeshProUGUI LVLDescription;

    // Новые переменные
    public TextMeshProUGUI TotalUpgradeAmountText; // Отображение суммы UpgradeAmount
    public int UpgradeLevel { get; private set; } = 0; // Текущий уровень апгрейда
    private double totalUpgradeAmount = 0; // Общая сумма UpgradeAmount для этого апгрейда

    // Обновление UI для общей суммы UpgradeAmount
    public void UpdateTotalUpgradeAmount(double amount)
    {
        totalUpgradeAmount += amount;
        TotalUpgradeAmountText.text = totalUpgradeAmount.ToString();
    }

    // Обновление уровня апгрейда
    public void UpdateUpgradeLevel()
    {
        UpgradeLevel++;
        UpgradeLevelText.text = UpgradeLevel.ToString();
    }
}
