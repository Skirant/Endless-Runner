using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider setup")]
    [SerializeField, Range(0, 1f)] private float sliderValue; // Значение слайдера (от 0 до 1)
    public bool CurrentValue { get; private set; } // Текущее состояние переключателя (вкл/выкл)

    private Slider _slider; // Ссылка на компонент Slider

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f; // Длительность анимации
    [SerializeField] private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1); // Кривая плавности анимации

    private Coroutine _animateSliderCoroutine; // Ссылка на текущую корутину анимации

    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn; // Событие, срабатывающее при включении
    [SerializeField] private UnityEvent onToggleOff; // Событие, срабатывающее при выключении

    private ToggleSwitchGroupManager _toggleSwitchGroupManager; // Менеджер группы переключателей

    // Вызывается в редакторе при изменении свойств объекта
    protected void OnValidate()
    {
        SetupToggleComponents(); // Настраиваем компоненты
        _slider.value = sliderValue; // Устанавливаем значение слайдера
    }

    // Настройка компонентов переключателя
    private void SetupToggleComponents()
    {
        if (_slider != null) // Если слайдер уже настроен, выходим
            return;

        SetUpSliderComponent(); // Инициализация слайдера
    }

    // Метод для инициализации слайдера (добавить логику, если потребуется)
    // Метод для настройки компонента Slider
    private void SetUpSliderComponent()
    {
        _slider = GetComponent<Slider>(); // Пытаемся найти компонент Slider

        if (_slider == null) // Если компонент не найден
        {
            Debug.Log(message: "No slider found!", context: this); // Выводим сообщение в консоль
            return;
        }

        _slider.interactable = false; // Делаем слайдер недоступным для взаимодействия
        var sliderColors = _slider.colors; // Получаем текущие цвета слайдера
        sliderColors.disabledColor = Color.white; // Устанавливаем цвет для недоступного состояния
        _slider.colors = sliderColors; // Применяем измененные цвета
        _slider.transition = Selectable.Transition.None; // Отключаем переходы слайдера
    }

    // Метод для установки менеджера группы переключателей
    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchGroupManager = manager; // Присваиваем переданный менеджер
    }

    // Вызывается при инициализации объекта
    private void Awake()
    {
        SetupToggleComponents(); // Настраиваем компоненты переключателя
    }

    // Обрабатываем клик по переключателю
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle(); // Переключаем состояние
    }

    // Логика переключения состояния
    private void Toggle()
    {
        if (_toggleSwitchGroupManager != null) // Если менеджер группы задан
        {
            _toggleSwitchGroupManager.ToggleGroup(toggleSwitch: this); // Сообщаем менеджеру о переключении
        }
        else
        {
            SetStateAndStartAnimation(!CurrentValue); // Меняем состояние и запускаем анимацию
        }
    }

    // Метод для управления переключением через менеджер
    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo); // Устанавливаем состояние и запускаем анимацию
    }
    // Метод для установки состояния и запуска анимации
    private void SetStateAndStartAnimation(bool state)
    {
        CurrentValue = state; // Устанавливаем текущее состояние

        // Вызываем соответствующее событие
        if (CurrentValue)
            onToggleOn?.Invoke(); // Событие при включении
        else
            onToggleOff?.Invoke(); // Событие при выключении

        // Останавливаем текущую корутину анимации, если она запущена
        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        // Запускаем новую корутину анимации
        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    // Корутина для анимации слайдера
    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value; // Начальное значение слайдера
        float endValue = CurrentValue ? 1 : 0; // Конечное значение (1 для включенного состояния, 0 для выключенного)

        float time = 0; // Переменная для отслеживания времени
        if (animationDuration > 0) // Если длительность анимации больше 0
        {
            while (time < animationDuration) // Выполняем анимацию в течение заданного времени
            {
                time += Time.deltaTime; // Увеличиваем время на прошедший кадр

                // Вычисляем текущий коэффициент интерполяции с использованием кривой плавности
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                // Интерполируем значение слайдера
                _slider.value = Mathf.Lerp(startValue, endValue, lerpFactor);

                yield return null; // Ждем следующий кадр
            }
        }

        _slider.value = endValue; // Устанавливаем конечное значение слайдера
    }
}
