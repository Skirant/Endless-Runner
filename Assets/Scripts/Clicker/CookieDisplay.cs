using TMPro;
using UnityEngine;

public class CookieDisplay : MonoBehaviour
{
    /// <summary>
    /// Обновляет текст с количеством "печенек", форматируя число и добавляя суффиксы.
    /// </summary>
    /// <param name="cookieCount">Текущее количество "печенек" (double), которое нужно отформатировать.</param>
    /// <param name="textToChange">Текстовый элемент TextMeshPro, который нужно обновить.</param>
    /// <param name="optionalEndText">Дополнительный текст, который будет добавлен после числа (необязательно).</param>
    public void UpdateCookieText(double cookieCount, TextMeshProUGUI textToChange, string optionalEndText = null, string prefix = "")
    {
        // Массив суффиксов для больших чисел
        string[] suffixes = { "", "k", "M", "B", "T", "Q" };
        int index = 0;

        // Пока число >= 1000 и мы не достигли конца массива суффиксов
        while (cookieCount >= 1000 && index < suffixes.Length - 1)
        {
            cookieCount /= 1000;
            index++;
        }

        // Подготовка форматированного текста для отображения
        string formattedText = index == 0
            ? cookieCount.ToString("0")
            : cookieCount.ToString("F1") + suffixes[index];

        // Добавляем префикс, форматированный текст и опциональный текст
        textToChange.text = prefix + formattedText + optionalEndText;
    }
}
