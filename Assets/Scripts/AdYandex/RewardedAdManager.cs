using System.Collections;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public string rewardID; // ������������� �������
    private CookieManager cookieManager;

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
            StartCoroutine(cookieManager.ActivateDoubleReward(120)); // ���������� ����� �� 2 ������
        }
    }
}
