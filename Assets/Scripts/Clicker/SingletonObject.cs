using UnityEngine;

public class SingletonObject : MonoBehaviour
{
    private static SingletonObject instance;

    void Awake()
    {
        if (instance == null)
        {
            // Если экземпляра нет, сохраняем текущий
            instance = this;
            DontDestroyOnLoad(gameObject); // Объект сохраняется между сценами
        }
        else
        {
            // Если экземпляр уже существует, уничтожаем текущий объект
            Destroy(gameObject);
        }
    }
}
