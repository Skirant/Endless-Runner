using UnityEngine;

public class InitializeUpgrades : MonoBehaviour
{
    public void Initialize(CookieUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        // Проходим по каждому улучшению в массиве
        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i; // Сохраняем текущий индекс для использования внутри лямбда-функции

            // Создаем UI-элемент для улучшения, используя заданный префаб и родительский объект
            GameObject go = Instantiate(UIToSpawn, spawnParent);

            // Сбрасываем стоимость улучшения к оригинальной
            upgrades[currentIndex].CurrentUpgradeCost = upgrades[currentIndex].OriginalUpgradeCost;

            // Получаем ссылку на компоненты UI-элемента
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();

            // Устанавливаем текст на кнопке улучшения
            //buttonRef.UpgradeButtonText.text = upgrades[currentIndex].UpgradeButtonText;

            buttonRef.UpgradeButtonDescription.text = upgrades[currentIndex].UpgradeButtonDescription;
            buttonRef.DamageTypeDescription.text = upgrades[currentIndex].DamageTypeDescription;
            buttonRef.LVLDescription.text = upgrades[currentIndex].LVLDescription;

            // Устанавливаем описание улучшения с учётом количества
            buttonRef.UpgradeLevelText.SetText(upgrades[currentIndex].UpgradeButtonDescription, upgrades[currentIndex].UpgradeAmount);

            // Устанавливаем текст стоимости улучшения
            buttonRef.UpgradeCostText.text = "Cost: " + upgrades[currentIndex].CurrentUpgradeCost;

            buttonRef.TotalUpgradeAmountText.text = "0";
            buttonRef.UpgradeLevelText.text = "0";

            // Добавляем обработчик события на кнопку, чтобы при нажатии вызывалась функция обработки улучшения
            buttonRef.UpgradeButton.onClick.AddListener(delegate
            {
                // Передаем в обработчик текущее улучшение и ссылки на UI
                CookieManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef);
            });
        }
    }
}
