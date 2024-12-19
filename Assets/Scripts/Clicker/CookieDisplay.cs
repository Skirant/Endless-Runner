using TMPro;
using UnityEngine;

public class CookieDisplay : MonoBehaviour
{
    /// <summary>
    /// ��������� ����� � ����������� "�������", ���������� ����� � �������� ��������.
    /// </summary>
    /// <param name="cookieCount">������� ���������� "�������" (double), ������� ����� ���������������.</param>
    /// <param name="textToChange">��������� ������� TextMeshPro, ������� ����� ��������.</param>
    /// <param name="optionalEndText">�������������� �����, ������� ����� �������� ����� ����� (�������������).</param>
    public void UpdateCookieText(double cookieCount, TextMeshProUGUI textToChange, string optionalEndText = null, string prefix = "")
    {
        // ������ ��������� ��� ������� �����
        string[] suffixes = { "", "k", "M", "B", "T", "Q" };
        int index = 0;

        // ���� ����� >= 1000 � �� �� �������� ����� ������� ���������
        while (cookieCount >= 1000 && index < suffixes.Length - 1)
        {
            cookieCount /= 1000;
            index++;
        }

        // ���������� ���������������� ������ ��� �����������
        string formattedText = index == 0
            ? cookieCount.ToString("0")
            : cookieCount.ToString("F1") + suffixes[index];

        // ��������� �������, ��������������� ����� � ������������ �����
        textToChange.text = prefix + formattedText + optionalEndText;
    }
}
