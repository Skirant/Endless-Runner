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
    public void UpdateCookieText(double cookieCount, TextMeshProUGUI textToChange, string optionalEndText = null)
    {
        // ������ ��������� ��� ������� �����
        string[] suffixes = { "", "k", "M", "B", "T", "Q" };
        int index = 0; // ������� ������ ��� ��������

        // ���� ����� >= 1000 � �� �� �������� ����� ������� ���������
        while (cookieCount >= 1000 && index < suffixes.Length - 1)
        {
            cookieCount /= 1000; // ����� ����� �� 1000
            index++; // ��������� � ���������� ��������

            // ���� �������� ���������� �������� � ����� ��� ��� ������ ��� ����� 1000, ���������� �������
            if (index >= suffixes.Length - 1 && cookieCount >= 1000)
            {
                break;
            }
        }

        // ���������� ���������������� ������ ��� �����������
        string formattedText;

        if (index == 0)
        {
            // ���� ������� �� �����, ������ ����������� ����� � ������
            formattedText = cookieCount.ToString();
        }
        else
        {
            // ���� ������� �����, ��������� ���� ����� ����� ������� ��� ������ ����������
            formattedText = cookieCount.ToString("F1") + suffixes[index];
        }

        // ������������� �������� ����� � ��������� ������� TextMeshPro
        textToChange.text = formattedText + optionalEndText;
    }
}
