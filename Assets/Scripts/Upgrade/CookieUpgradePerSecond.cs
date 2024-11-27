using UnityEngine;

[CreateAssetMenu(menuName = "Cookie Upgrade/Cookies Per Second", fileName = "Cookies per Second")]
public class CookieUpgradePerSecond : CookieUpgrade
{
        public override void ApplyUpgrade()
        {
            GameObject go = Instantiate(CookieManager.instance.CookiesPerSecondObjToSpawn, Vector3.zero, Quaternion.identity);
            go.GetComponent<CookiePerSecondTimer>().CookiePerSecond = UpgradeAmount;

            CookieManager.instance.SimpleCookiePerSecondIncrease(UpgradeAmount);
        }  
}
