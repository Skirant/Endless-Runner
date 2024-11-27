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
    public void UpdateCookieText(double cookieCount, TextMeshProUGUI textToChange, string optionalEndText = null)
    {
        // Массив суффиксов для больших чисел
        string[] suffixes = { "", "k", "M", "B", "T", "Q" };
        int index = 0; // Текущий индекс для суффикса

        // Пока число >= 1000 и мы не достигли конца массива суффиксов
        while (cookieCount >= 1000 && index < suffixes.Length - 1)
        {
            cookieCount /= 1000; // Делим число на 1000
            index++; // Переходим к следующему суффиксу

            // Если достигли последнего суффикса и число все еще больше или равно 1000, прекращаем деление
            if (index >= suffixes.Length - 1 && cookieCount >= 1000)
            {
                break;
            }
        }

        // Подготовка форматированного текста для отображения
        string formattedText;

        if (index == 0)
        {
            // Если суффикс не нужен, просто преобразуем число в строку
            formattedText = cookieCount.ToString();
        }
        else
        {
            // Если суффикс нужен, добавляем одну цифру после запятой для лучшей читаемости
            formattedText = cookieCount.ToString("F1") + suffixes[index];
        }

        // Устанавливаем итоговый текст в текстовый элемент TextMeshPro
        textToChange.text = formattedText + optionalEndText;
    }
}
