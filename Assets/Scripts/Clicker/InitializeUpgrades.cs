using UnityEngine;
using YG;

public class InitializeUpgrades : MonoBehaviour
{
    private CookieUpgrade[] upgrades; // ���� ��� �������� ������� ���������
    private GameObject[] upgradeUIElements; // ���� ��� �������� ������� ��������� UI-���������

    public CookieDisplay _cookieDisplay;

    private void Awake()
    {
        _cookieDisplay = GetComponent<CookieDisplay>();
    }
    public void ApplyLoadedUpgrades()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            // ��������� ��������� �� ������ ����������� ������
            for (int j = 0; j < upgrades[i].UpgradeLevel; j++)
            {
                upgrades[i].ApplyUpgrade();
            }
        }
    }

    public void Initialize(CookieUpgrade[] upgrades, GameObject UIToSpawn, Transform spawnParent)
    {
        this.upgrades = upgrades;
        upgradeUIElements = new GameObject[upgrades.Length];

        // ���������, ���� �� ���������� ������
        if (YG2.saves.savedUpgradeCosts == null || YG2.saves.savedUpgradeLevels == null)
        {
            YG2.saves.savedUpgradeCosts = new double[upgrades.Length];
            YG2.saves.savedUpgradeLevels = new int[upgrades.Length];
        }

        for (int i = 0; i < upgrades.Length; i++)
        {
            int currentIndex = i;

            // ������ UI-�������
            GameObject go = Instantiate(UIToSpawn, spawnParent);
            upgradeUIElements[currentIndex] = go;

            // �������� ���������� ������
            upgrades[currentIndex].CurrentUpgradeCost = YG2.saves.savedUpgradeCosts[i] > 0
                ? YG2.saves.savedUpgradeCosts[i]
                : upgrades[currentIndex].OriginalUpgradeCost;

            upgrades[currentIndex].UpgradeLevel = YG2.saves.savedUpgradeLevels[i];

            // �������� ������ �� UI
            UpgradeButtonReferences buttonRef = go.GetComponent<UpgradeButtonReferences>();

            UpdateUpgradeCostText(upgrades[currentIndex], buttonRef);
            buttonRef.TotalUpgradeAmountText.text = upgrades[currentIndex].UpgradeLevel.ToString();
            buttonRef.UpgradeLevelText.text = upgrades[currentIndex].UpgradeLevel.ToString();

            buttonRef.UpdateUpgradeLevel(YG2.saves.savedUpgradeLevels[currentIndex]);
            buttonRef.UpdateTotalUpgradeAmount(upgrades[currentIndex].UpgradeAmount * YG2.saves.savedUpgradeLevels[currentIndex]);

            // ��������� ���������� ������
            buttonRef.UpgradeButton.onClick.AddListener(delegate
            {
                CookieManager.instance.OnUpgradeButtonClick(upgrades[currentIndex], buttonRef);

                // ���������� ������ ����� �������
                YG2.saves.savedUpgradeCosts[currentIndex] = upgrades[currentIndex].CurrentUpgradeCost;
                YG2.saves.savedUpgradeLevels[currentIndex] = upgrades[currentIndex].UpgradeLevel;
                YG2.SaveProgress();

                UpdateUpgradeCostText(upgrades[currentIndex], buttonRef);
            });
        }
    }

    private void OnEnable()
    {
        YG2.onSwitchLang += UpdateAllLanguages; // �������� �� ����� �����
    }

    private void OnDisable()
    {
        YG2.onSwitchLang -= UpdateAllLanguages; // ������� �� ����� �����
    }

    private void UpdateAllLanguages(string lang)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            SwitchLanguage(lang, i);
        }
    }

    public void SwitchLanguage(string lang, int index)
    {
        UpgradeButtonReferences buttonRef = upgradeUIElements[index].GetComponent<UpgradeButtonReferences>();

        switch (lang)
        {
            case "ru":
                buttonRef.UpgradeButtonDescription.text = upgrades[index].UpgradeButtonDescriptionRu;
                buttonRef.DamageTypeDescription.text = upgrades[index].DamageTypeDescriptionRu;
                break;
            case "tr":
                // �������� ����� ��� ��������� �����, ���� ���������
                break;
            default:
                buttonRef.UpgradeButtonDescription.text = upgrades[index].UpgradeButtonDescriptionEn;
                buttonRef.DamageTypeDescription.text = upgrades[index].DamageTypeDescriptionEn;
                break;
        }
    }

    private void UpdateUpgradeCostText(CookieUpgrade upgrade, UpgradeButtonReferences buttonRef)
    {
        _cookieDisplay.UpdateCookieText(upgrade.CurrentUpgradeCost, buttonRef.UpgradeCostText, " ");
    }
}
