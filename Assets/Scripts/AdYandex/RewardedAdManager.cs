using System.Collections;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public string rewardID; // Идентификатор рекламы
    private CookieManager cookieManager;

    private void Start()
    {
        cookieManager = CookieManager.instance; // Получаем ссылку на CookieManager
    }

    public void ShowRewardedAd()
    {
        YG2.RewardedAdvShow(rewardID);
    }

    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
    }

    private void OnReward(string id)
    {
        if (id == rewardID)
        {
            StartCoroutine(cookieManager.ActivateDoubleReward(120)); // Активируем бонус на 2 минуты
        }
    }
}
