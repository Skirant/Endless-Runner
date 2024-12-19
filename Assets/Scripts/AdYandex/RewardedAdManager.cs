using System.Collections;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public string rewardID; // ������������� �������
    private CookieManager cookieManager;

    public GameObject AdWindows;

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
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
    }

    private void OnReward(string id)
    {
        if (id == rewardID)
        {
            StartCoroutine(cookieManager.ActivateDoubleReward(60)); // ���������� ����� �� 2 ������
            StartCoroutine(StartCountdown(60)); // �������� ������ � �������
            AdWindows.SetActive(false);
        }
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
