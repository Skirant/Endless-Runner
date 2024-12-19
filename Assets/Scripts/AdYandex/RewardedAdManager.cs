using System.Collections;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public string rewardID; // Идентификатор рекламы
    private CookieManager cookieManager;

    public GameObject AdWindows;

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
            StartCoroutine(cookieManager.ActivateDoubleReward(60)); // Активируем бонус на 2 минуты
            StartCoroutine(StartCountdown(60)); // Начинаем отсчет в консоли
            AdWindows.SetActive(false);
        }
    }

    private IEnumerator StartCountdown(int seconds)
    {
        while (seconds > 0)
        {
            if (seconds % 5 == 0) // Проверяем, делится ли оставшееся время на 5
            {
                Debug.Log($"Осталось времени: {seconds} секунд");
            }
            yield return new WaitForSeconds(1); // Ждем 1 секунду
            seconds--;
        }

        Debug.Log("Бонусное время истекло!");
    }
}
