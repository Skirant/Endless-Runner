using UnityEngine;

[CreateAssetMenu(menuName = "Cookie Upgrade/Cokies Per Click", fileName = "Cookies Per Click")]
public class CookieUpgradePerClick : CookieUpgrade
{
    public override void ApplyUpgrade()
    {
        CookieManager.instance.CookiesPerClickUpgrade += UpgradeAmount;
    }
}
