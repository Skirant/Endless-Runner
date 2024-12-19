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

        if (YG2.envir.isMobile) // Если мобильное устройство
        {
            // Задаем пресет "Stretch по ширине, прижато к низу"
            rectTransform.anchorMin = new Vector2(0, 0); // Левая нижняя точка
            rectTransform.anchorMax = new Vector2(1, 0); // Правая нижняя точка
            rectTransform.pivot = new Vector2(0.5f, 0.5f);  // Pivot внизу по центру
            rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y); // Left
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y); // Right
        }
        /*else if (YG2.envir.isDesktop) // Если ПК
        {
            // Задаем пресет "Bottom Center"
            rectTransform.anchorMin = new Vector2(0.5f, 0); // Центр по горизонтали
            rectTransform.anchorMax = new Vector2(0.5f, 0); // Центр по горизонтали
            rectTransform.pivot = new Vector2(0.5f, 0);     // Pivot внизу по центру
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(951f, 662f);
        }*/
    }
}
