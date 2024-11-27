using UnityEngine;

public class CookiePerSecondTimer : MonoBehaviour
{
    public float TimerDuration = 1f; // Длительность таймера
    public double CookiePerSecond { get; set; } // Количество печенек в секунду

    private float _counter; // Счетчик времени

    private void Update()
    {
        _counter += Time.deltaTime; // Увеличиваем счетчик на прошедшее время

        if (_counter >= TimerDuration)
        {
            // Увеличиваем количество печенек через менеджер
            CookieManager.instance.SimpleCookieIncrease(CookiePerSecond);

            // Сбрасываем счетчик
            _counter = 0;
        }
    }
}
