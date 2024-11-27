using TMPro;
using UnityEngine;

public class ClickerController : MonoBehaviour
{
    public int swords = 0; //���������� ����
    public TextMeshProUGUI numberSwordText; //����� �����

    private void Start()
    {
        UpdatenumberSwordText();
    }

    public void AddSword()
    {
        swords += 1;
        UpdatenumberSwordText();
        print("click");
    }

    private void UpdatenumberSwordText()
    {
        numberSwordText.text = swords.ToString();
    }

    private void OnMouseDown()
    {
        AddSword();
    }
}
