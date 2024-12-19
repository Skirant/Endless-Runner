using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private GameObject _win;

    public void Restart()
    {
        // ������������� ������ � ��������� �������� �������
        SetDefaultValues();

        // ��������� ��������
        YG2.SaveProgress();

        // ������������� ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // ������������ �������� �����
        _win.SetActive(false);
    }

    private void SetDefaultValues()
    {
        YG2.saves.savedCookieCount = 0; // ����� ���������� �������
        YG2.saves.savedCookiesPerSecond = 0; // ����� ������� � �������

        for (int i = 0; i < YG2.saves.savedUpgradeLevels.Length; i++)
        {
            YG2.saves.savedUpgradeLevels[i] = 0; // ����� ������ ���������

            // ������������� ��������� ��������� �� ������� ��������
            YG2.saves.savedUpgradeCosts[i] = CookieManager.instance.CookieUpgrades[i].OriginalUpgradeCost;
        }
    }
}

