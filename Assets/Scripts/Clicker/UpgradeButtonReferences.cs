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

    // ����� ����������
    public TextMeshProUGUI TotalUpgradeAmountText; // ����������� ����� UpgradeAmount
    public int UpgradeLevel { get; private set; } = 0; // ������� ������� ��������
    private double totalUpgradeAmount = 0; // ����� ����� UpgradeAmount ��� ����� ��������

    // ���������� UI ��� ����� ����� UpgradeAmount
    public void UpdateTotalUpgradeAmount(double amount)
    {
        totalUpgradeAmount += amount;
        TotalUpgradeAmountText.text = totalUpgradeAmount.ToString();
    }

    // ���������� ������ ��������
    public void UpdateUpgradeLevel()
    {
        UpgradeLevel++;
        UpgradeLevelText.text = UpgradeLevel.ToString();
    }
}
