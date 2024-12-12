using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // Метод для перезапуска сцены
    [SerializeField] private GameObject _win;

    public void Restart()
    {
        // Перезапускаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _win.SetActive(false);
    }
}
