using TMPro;
using UnityEngine;

public class PopurText : MonoBehaviour
{
    [SerializeField] private float _startingVelocity = 750f; // ��������� �������� ������������ ������
    [SerializeField] private float _velocityDecayRate = 1500f; // �������� �������� ��������� ��������
    [SerializeField] private float _timeBeforeFadeStarts = 0.6f; // ����� ����� ������� ������������ ������
    [SerializeField] private float _fadeSpeed = 3f; // �������� ��������� ������

    private TextMeshProUGUI _clickAmountText; // ������ �� ��������� TextMeshPro ��� ����������� ������
    private Vector2 _currentVelocity; // ������� �������� ������
    private Color _startColor; // ��������� ���� ������
    private float _timer; // ������ ��� ������������ ������� ����� ������
    private float _textAlpha; // ������������ ������

    private void OnEnable()
    {
        // �������� ��������� TextMeshProUGUI ��� ������ � ������� ������������ ����
        _clickAmountText = GetComponent<TextMeshProUGUI>();

        // ������������� ����� ���� ������
        Color newColor = _clickAmountText.color; // ����� ������� ���� ������
        newColor.a = 1f; // ������������� ������������ (�����-�����) �� ��������
        _clickAmountText.color = newColor; // ��������� ����� ���� � ������

        // ��������� ��������� ���� ������
        _startColor = newColor;

        // ���������� ������
        _timer = 0f;

        // ������������� ������������ ������ �� ��������
        _textAlpha = 1f;
    }

    /// <summary>
    /// ������� ����������� ����� ����� ObjectPoolManager.
    /// </summary>
    /// <param name="amount">����������, ������� ����� ���������� �� ����������� ������.</param>
    /// <returns>���������� ������ PopupText.</returns>
    public static PopurText Create(double amount)
    {
        // ������� ������ ����� ���
        GameObject popupObj = ObjectPoolManager.SpawnObject(
            CookieManager.instance.CookieTextPopup, // ������ ������������ ������
            CookieManager.instance.MainGameCanvas.transform // ������� ������������� �������
        );

        // �������� ��������� PopupText � ���������� �������
        PopurText cookiePopUp = popupObj.GetComponent<PopurText>();
        cookiePopUp.Init(amount); // �������������� ������ � �������� �����������
        return cookiePopUp; // ���������� ����������� ����
    }

    /// <summary>
    /// ������������� ������������ ������.
    /// </summary>
    /// <param name="amount">����������, ������� ����� ���������� �� ����������� ������.</param>
    public void Init(double amount)
    {
        // ������������� ����� ��� �����������
        _clickAmountText.text = "+" + amount.ToString("0");

        // ���������� ��������� ��������� �������� �� ��� X
        float randomX = Random.Range(-300f, 300f);
        _currentVelocity = new Vector2(randomX, _startingVelocity); // ������������� ��������� �������� ������
    }

    private void Update()
    {
        // �������� ������
        _currentVelocity.y -= Time.deltaTime * _velocityDecayRate; // ��������� �������� �� ��� Y
        transform.Translate(_currentVelocity * Time.deltaTime); // ���������� ������ �� ������ ������� ��������

        // ��������� ����� (���������)
        _timer += Time.deltaTime; // ����������� ������

        if (_timer >= _timeBeforeFadeStarts) // ���������, ���� �� �������� ��������� ������
        {
            _textAlpha -= Time.deltaTime * _fadeSpeed; // ��������� ������������ ������
            _startColor.a = _textAlpha; // ������������� ������������ ��� �����
            _clickAmountText.color = _startColor; // ��������� ���������� ���� � ������

            // ���� ������������ ������ ����� ������� ��� ������
            if (_textAlpha <= 0f)
            {
                // ���������� ������ ������� � ���
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
