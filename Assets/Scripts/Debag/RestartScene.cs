using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private GameObject _win;

    public void Restart()
    {
        // Устанавливаем данные в дефолтные значения вручную
        SetDefaultValues();

        // Сохраняем прогресс
        YG2.SaveProgress();

        // Перезапускаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Деактивируем победный экран
        _win.SetActive(false);
    }

    private void SetDefaultValues()
    {
        YG2.saves.savedCookieCount = 0; // Сброс количества печенек
        YG2.saves.savedCookiesPerSecond = 0; // Сброс прибыли в секунду

        for (int i = 0; i < YG2.saves.savedUpgradeLevels.Length; i++)
        {
            YG2.saves.savedUpgradeLevels[i] = 0; // Сброс уровня улучшений

            // Устанавливаем стоимость улучшения на базовое значение
            YG2.saves.savedUpgradeCosts[i] = CookieManager.instance.CookieUpgrades[i].OriginalUpgradeCost;
        }
    }
}

