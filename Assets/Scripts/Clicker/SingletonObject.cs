using UnityEngine;

public class SingletonObject : MonoBehaviour
{
    private static SingletonObject instance;

    void Awake()
    {
        if (instance == null)
        {
            // ���� ���������� ���, ��������� �������
            instance = this;
            DontDestroyOnLoad(gameObject); // ������ ����������� ����� �������
        }
        else
        {
            // ���� ��������� ��� ����������, ���������� ������� ������
            Destroy(gameObject);
        }
    }
}
