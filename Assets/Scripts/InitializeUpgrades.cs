using UnityEngine;

public class InitializeUpgrades : MonoBehaviour
{
    public void Initialize(CookieUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i;

            // ������� UI ������� ��� ���������
            GameObject go = Instantiate(UIToSpawn, spawnParent);

            // ���������� ��������� ���������
            upgrades[currentIndex].CurrentUpgradeCost = upgrades[currentIndex].OriginalUpgradeCost;

            // ������������� ��������� �������� ��� ������
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();
            buttonRef.UpgradeButtonText.text = upgrades[currentIndex].UpgradeButtonText;
            buttonRef.UpgradeDescriptionText.SetText(upgrades[currentIndex].UpgradeButtonDescription, upgrades[currentIndex].UpgradeAmount);
            buttonRef.UpgradeCostText.text = "Cost: " + upgrades[currentIndex].CurrentUpgradeCost;

            // ��������� ������� OnClick
            buttonRef.UpgradeButton.onClick.AddListener(delegate
            {
                CookieManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef);
            });
        }
    }
}
