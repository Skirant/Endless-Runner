using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Income Upgrade", menuName = "Upgrades/PassiveIncomeUpgrade")]
public class PassiveIncomeUpgrade : CookieUpgrade
{
    public override void ApplyUpgrade()
    {
        CookieManager.instance.SimpleCookiePerSecondIncrease(UpgradeAmount);
    }
}

[CreateAssetMenu(fileName = "New Click Damage Upgrade", menuName = "Upgrades/ClickDamageUpgrade")]
public class ClickDamageUpgrade : CookieUpgrade
{
    public override void ApplyUpgrade()
    {
        CookieManager.instance.CookiesPerClickUpgrade += UpgradeAmount;
    }
}

public abstract class CookieUpgrade : ScriptableObject
{
    public float UpgradeAmount = 1f;

    public double OriginalUpgradeCost = 100;
    public double CurrentUpgradeCost = 100;
    public double CostIncreaseMultiplierPerPurchase = 0.05f;

    public string UpgradeButtonText;
    [TextArea(3, 10)]
    public string LVLDescription;

    public string UpgradeButtonDescriptionRu;
    public string UpgradeButtonDescriptionEn;
    public string DamageTypeDescriptionRu;
    public string DamageTypeDescriptionEn;

    public int UpgradeLevel { get; set; } = 0; // Новый уровень улучшения

    public abstract void ApplyUpgrade();

    private void OnValidate()
    {
        CurrentUpgradeCost = OriginalUpgradeCost;
    }
}
