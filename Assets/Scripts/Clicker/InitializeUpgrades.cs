using UnityEngine;

public class InitializeUpgrades : MonoBehaviour
{
    public void Initialize(CookieUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        // �������� �� ������� ��������� � �������
        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i; // ��������� ������� ������ ��� ������������� ������ ������-�������

            // ������� UI-������� ��� ���������, ��������� �������� ������ � ������������ ������
            GameObject go = Instantiate(UIToSpawn, spawnParent);

            // ���������� ��������� ��������� � ������������
            upgrades[currentIndex].CurrentUpgradeCost = upgrades[currentIndex].OriginalUpgradeCost;

            // �������� ������ �� ���������� UI-��������
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();

            // ������������� ����� �� ������ ���������
            //buttonRef.UpgradeButtonText.text = upgrades[currentIndex].UpgradeButtonText;

            buttonRef.UpgradeButtonDescription.text = upgrades[currentIndex].UpgradeButtonDescription;
            buttonRef.DamageTypeDescription.text = upgrades[currentIndex].DamageTypeDescription;
            buttonRef.LVLDescription.text = upgrades[currentIndex].LVLDescription;

            // ������������� �������� ��������� � ������ ����������
            buttonRef.UpgradeLevelText.SetText(upgrades[currentIndex].UpgradeButtonDescription, upgrades[currentIndex].UpgradeAmount);

            // ������������� ����� ��������� ���������
            buttonRef.UpgradeCostText.text = "Cost: " + upgrades[currentIndex].CurrentUpgradeCost;

            buttonRef.TotalUpgradeAmountText.text = "0";
            buttonRef.UpgradeLevelText.text = "0";

            // ��������� ���������� ������� �� ������, ����� ��� ������� ���������� ������� ��������� ���������
            buttonRef.UpgradeButton.onClick.AddListener(delegate
            {
                // �������� � ���������� ������� ��������� � ������ �� UI
                CookieManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef);
            });
        }
    }
}
