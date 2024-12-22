using System.Collections;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public string rewardID; // ������������� �������
    private CookieManager cookieManager;

    public GameObject AdWindows;
    public GameObject AdActivation;
    public GameObject AdError;

    public int TimerAd = 60;

    private void Start()
    {
        cookieManager = CookieManager.instance; // �������� ������ �� CookieManager
    }

    public void ShowRewardedAd()
    {
        YG2.RewardedAdvShow(rewardID);
    }

    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
        YG2.onErrorRewardedAdv += OnErrorRewardedAdv;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
        YG2.onErrorRewardedAdv -= OnErrorRewardedAdv;
    }

    private void OnReward(string id)
    {
        if (id == rewardID)
        {
            StartCoroutine(cookieManager.ActivateDoubleReward(TimerAd)); // ���������� ����� �� 2 ������
            StartCoroutine(StartCountdown(TimerAd)); // �������� ������ � �������
            AdWindows.SetActive(false);
            AdActivation.SetActive(true);
        }
    }
    private void OnErrorRewardedAdv()
    {
        AdError.SetActive(true);
    }

    private IEnumerator StartCountdown(int seconds)
    {
        while (seconds > 0)
        {
            if (seconds % 5 == 0) // ���������, ������� �� ���������� ����� �� 5
            {
                Debug.Log($"�������� �������: {seconds} ������");
            }
            yield return new WaitForSeconds(1); // ���� 1 �������
            seconds--;
        }

        Debug.Log("�������� ����� �������!");
    }
}
