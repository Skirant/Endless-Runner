using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float _startingVelocity = 750f; // Начальная скорость всплывающего текста
    [SerializeField] private float _velocityDecayRate = 1500f; // Скорость снижения начальной скорости
    [SerializeField] private float _timeBeforeFadeStarts = 0.6f; // Время перед началом исчезновения текста
    [SerializeField] private float _fadeSpeed = 3f; // Скорость затухания текста
    [SerializeField] private AudioClip _clickSound; // Звук, воспроизводимый при клике

    private TextMeshProUGUI _clickAmountText; // Ссылка на компонент TextMeshPro для отображения текста
    private Vector2 _currentVelocity; // Текущая скорость текста
    private Color _startColor; // Начальный цвет текста
    private float _timer; // Таймер для отслеживания времени жизни текста
    private float _textAlpha; // Прозрачность текста
    private AudioSource _audioSource; // Источник звука

    private void OnEnable()
    {
        // Получаем компонент TextMeshProUGUI для работы с текстом всплывающего окна
        _clickAmountText = GetComponent<TextMeshProUGUI>();

        // Устанавливаем новый цвет текста
        Color newColor = _clickAmountText.color; // Берем текущий цвет текста
        newColor.a = 1f; // Устанавливаем прозрачность (альфа-канал) на максимум
        _clickAmountText.color = newColor; // Применяем новый цвет к тексту

        // Сохраняем начальный цвет текста
        _startColor = newColor;

        // Сбрасываем таймер
        _timer = 0f;

        // Устанавливаем прозрачность текста на максимум
        _textAlpha = 1f;

        // Получаем или добавляем AudioSource
        if (!_audioSource)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (!_audioSource)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false; // Звук не должен проигрываться автоматически
            }
        }
    }

    /// <summary>
    /// Создает всплывающий текст через ObjectPoolManager.
    /// </summary>
    /// <param name="amount">Количество, которое будет отображено во всплывающем тексте.</param>
    /// <returns>Возвращает объект PopupText.</returns>
    public static PopupText Create(double amount, Vector3 clickPosition)
    {
        // Спавним объект через пул
        GameObject popupObj = ObjectPoolManager.SpawnObject(
            CookieManager.instance.CookieTextPopup, // Префаб всплывающего текста
            CookieManager.instance.MainGameCanvas.transform // Родительский Canvas
        );

        // Получаем компонент PopupText у созданного объекта
        PopupText cookiePopUp = popupObj.GetComponent<PopupText>();
        cookiePopUp.Init(amount, clickPosition); // Инициализируем объект с количеством и позицией
        return cookiePopUp; // Возвращаем всплывающее окно
    }

    /// <summary>
    /// Инициализация всплывающего текста.
    /// </summary>
    /// <param name="amount">Количество, которое будет отображено во всплывающем тексте.</param>
    public void Init(double amount, Vector3 spawnPosition)
    {
        // Устанавливаем позицию объекта
        transform.position = spawnPosition;

        // Генерируем случайное движение для естественности
        float randomX = Random.Range(-50f, 50f); // Небольшой разброс
        _currentVelocity = new Vector2(randomX, _startingVelocity);

        // Округляем и устанавливаем текст с префиксом +
        CookieDisplay cookieDisplay = CookieManager.instance.GetComponent<CookieDisplay>(); // Получаем CookieDisplay
        if (cookieDisplay != null)
        {
            cookieDisplay.UpdateCookieText(amount, _clickAmountText, "", "+");
        }
        else
        {
            // Если CookieDisplay не найден, используем резервное форматирование
            _clickAmountText.text = "+" + amount.ToString("0");
        }

        // Воспроизведение звука при инициализации
        /*if (_audioSource && _clickSound)
        {
            _audioSource.PlayOneShot(_clickSound);
        }*/

        FindAnyObjectByType<AudioManager>().Play("Click");
    }

    private void Update()
    {
        // Движение текста
        _currentVelocity.y -= Time.deltaTime * _velocityDecayRate; // Уменьшаем скорость по оси Y
        transform.Translate(_currentVelocity * Time.deltaTime); // Перемещаем объект на основе текущей скорости

        // Изменение цвета (затухание)
        _timer += Time.deltaTime; // Увеличиваем таймер

        if (_timer >= _timeBeforeFadeStarts) // Проверяем, пора ли начинать затухание текста
        {
            _textAlpha -= Time.deltaTime * _fadeSpeed; // Уменьшаем прозрачность текста
            _startColor.a = _textAlpha; // Устанавливаем прозрачность для цвета
            _clickAmountText.color = _startColor; // Применяем измененный цвет к тексту

            // Если прозрачность текста стала нулевой или меньше
            if (_textAlpha <= 0f)
            {
                // Возвращаем объект обратно в пул
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}

