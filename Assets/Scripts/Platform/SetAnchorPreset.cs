using UnityEngine;
using YG;

public class SetAnchorPreset : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform attached to this GameObject.");
            return;
        }

        if (YG2.envir.isMobile) // ���� ��������� ����������
        {
            // ������ ������ "Stretch �� ������, ������� � ����"
            rectTransform.anchorMin = new Vector2(0, 0); // ����� ������ �����
            rectTransform.anchorMax = new Vector2(1, 0); // ������ ������ �����
            rectTransform.pivot = new Vector2(0.5f, 0.5f);  // Pivot ����� �� ������
            rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y); // Left
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y); // Right
        }
        /*else if (YG2.envir.isDesktop) // ���� ��
        {
            // ������ ������ "Bottom Center"
            rectTransform.anchorMin = new Vector2(0.5f, 0); // ����� �� �����������
            rectTransform.anchorMax = new Vector2(0.5f, 0); // ����� �� �����������
            rectTransform.pivot = new Vector2(0.5f, 0);     // Pivot ����� �� ������
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(951f, 662f);
        }*/
    }
}
