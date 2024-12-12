using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // ����� ��� ����������� �����
    [SerializeField] private GameObject _win;

    public void Restart()
    {
        // ������������� ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _win.SetActive(false);
    }
}
