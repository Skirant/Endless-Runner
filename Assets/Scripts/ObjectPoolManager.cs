using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // ������ ����� ��������, ������� �������� ���������� � ������ ����
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    /// <summary>
    /// ������� ������ �� ���� ��� ������� �����, ���� ������ ����������� � ����.
    /// </summary>
    /// <param name="objectToSpawn">������, ������� ����� ������� ��� ����� �� ����.</param>
    /// <param name="parentTransform">������������ ������ (Transform), � �������� ����� ���������� ��������� ������.</param>
    /// <returns>������ �� ��������� ��� ������ �� ���� ������.</returns>
    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        // ���� ��� � ��������, ��� �������� ��������� � objectToSpawn.name
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

        if (pool == null) // ���� ��� �� ������, ������� �����
        {
            pool = new PooledObjectInfo
            {
                lookupString = objectToSpawn.name, // ������������� ��� ������� ��� ������
            };
            objectPools.Add(pool); // ��������� ����� ��� � ������ �����
        }

        // �������� ����� ������ ���������� ������ � ����
        GameObject go = pool.inactiveObjects.FirstOrDefault();

        if (go == null) // ���� ���������� �������� ���, ������� ����� ������
        {
            go = Instantiate(objectToSpawn, parentTransform); // ������� ������ � ��������� � ��������
        }
        else // ���� ����� ���������� ������
        {
            pool.inactiveObjects.Remove(go); // ������� ��� �� ������ ���������� ��������
            go.SetActive(true); // ���������� ������
        }

        // ���������� ������
        return go;
    }

    /// <summary>
    /// ���������� ������ ������� � ���, ����� ��� ����������.
    /// </summary>
    /// <param name="obj">������, ������� ����� ������� � ���.</param>
    public static void ReturnObjectToPool(GameObject obj)
    {
        // ��������� ��� ������� ��� ��������� 7 �������� (��������, "(Clone)")
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        // ���� ��������������� ��� ��� ������� �� ��� �����
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == goName);

        if (pool != null) // ���� ��� ������
        {
            obj.SetActive(false); // ������������ ������
            pool.inactiveObjects.Add(obj); // ��������� ��� � ������ ���������� �������� ����
        }
    }

    /// <summary>
    /// ��������� ��� �������� ���������� � ���� ��������.
    /// </summary>
    public class PooledObjectInfo
    {
        public string lookupString; // ���������� ������������� ��� ������ (������ ��� �������)
        public List<GameObject> inactiveObjects = new List<GameObject>(); // ������ ���������� �������� � ����
    }
}
