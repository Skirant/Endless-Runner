using UnityEngine;

public class InitializeUpgrades : MonoBehaviour
{
    public void Initialize(CookieUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i;

            // Создаем UI элемент для улучшения
            GameObject go = Instantiate(UIToSpawn, spawnParent);

            // Сбрасываем стоимость улучшения
            upgrades[currentIndex].CurrentUpgradeCost = upgrades[currentIndex].OriginalUpgradeCost;

            // Устанавливаем текстовые значения для кнопки
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();
            buttonRef.UpgradeButtonText.text = upgrades[currentIndex].UpgradeButtonText;
            buttonRef.UpgradeDescriptionText.SetText(upgrades[currentIndex].UpgradeButtonDescription, upgrades[currentIndex].UpgradeAmount);
            buttonRef.UpgradeCostText.text = "Cost: " + upgrades[currentIndex].CurrentUpgradeCost;

            // Добавляем событие OnClick
            buttonRef.UpgradeButton.onClick.AddListener(delegate
            {
                CookieManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef);
            });
        }
    }
}
