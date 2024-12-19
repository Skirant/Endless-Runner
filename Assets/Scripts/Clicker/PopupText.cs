using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float _startingVelocity = 750f; // ��������� �������� ������������ ������
    [SerializeField] private float _velocityDecayRate = 1500f; // �������� �������� ��������� ��������
    [SerializeField] private float _timeBeforeFadeStarts = 0.6f; // ����� ����� ������� ������������ ������
    [SerializeField] private float _fadeSpeed = 3f; // �������� ��������� ������
    [SerializeField] private AudioClip _clickSound; // ����, ��������������� ��� �����

    private TextMeshProUGUI _clickAmountText; // ������ �� ��������� TextMeshPro ��� ����������� ������
    private Vector2 _currentVelocity; // ������� �������� ������
    private Color _startColor; // ��������� ���� ������
    private float _timer; // ������ ��� ������������ ������� ����� ������
    private float _textAlpha; // ������������ ������
    private AudioSource _audioSource; // �������� �����

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

        // �������� ��� ��������� AudioSource
        if (!_audioSource)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (!_audioSource)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false; // ���� �� ������ ������������� �������������
            }
        }
    }

    /// <summary>
    /// ������� ����������� ����� ����� ObjectPoolManager.
    /// </summary>
    /// <param name="amount">����������, ������� ����� ���������� �� ����������� ������.</param>
    /// <returns>���������� ������ PopupText.</returns>
    public static PopupText Create(double amount, Vector3 clickPosition)
    {
        // ������� ������ ����� ���
        GameObject popupObj = ObjectPoolManager.SpawnObject(
            CookieManager.instance.CookieTextPopup, // ������ ������������ ������
            CookieManager.instance.MainGameCanvas.transform // ������������ Canvas
        );

        // �������� ��������� PopupText � ���������� �������
        PopupText cookiePopUp = popupObj.GetComponent<PopupText>();
        cookiePopUp.Init(amount, clickPosition); // �������������� ������ � ����������� � ��������
        return cookiePopUp; // ���������� ����������� ����
    }

    /// <summary>
    /// ������������� ������������ ������.
    /// </summary>
    /// <param name="amount">����������, ������� ����� ���������� �� ����������� ������.</param>
    public void Init(double amount, Vector3 spawnPosition)
    {
        // ������������� ������� �������
        transform.position = spawnPosition;

        // ���������� ��������� �������� ��� ��������������
        float randomX = Random.Range(-50f, 50f); // ��������� �������
        _currentVelocity = new Vector2(randomX, _startingVelocity);

        // ��������� � ������������� ����� � ��������� +
        CookieDisplay cookieDisplay = CookieManager.instance.GetComponent<CookieDisplay>(); // �������� CookieDisplay
        if (cookieDisplay != null)
        {
            cookieDisplay.UpdateCookieText(amount, _clickAmountText, "", "+");
        }
        else
        {
            // ���� CookieDisplay �� ������, ���������� ��������� ��������������
            _clickAmountText.text = "+" + amount.ToString("0");
        }

        // ��������������� ����� ��� �������������
        /*if (_audioSource && _clickSound)
        {
            _audioSource.PlayOneShot(_clickSound);
        }*/

        FindAnyObjectByType<AudioManager>().Play("Click");
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

