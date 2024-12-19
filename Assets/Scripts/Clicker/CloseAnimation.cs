using UnityEngine;

public class CloseAnimation : MonoBehaviour
{
    public void OnCloseAnimationEnd()
    {
        gameObject.SetActive(false); // Выключаем окно
    }
}
