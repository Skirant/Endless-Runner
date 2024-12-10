using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // Список пулов объектов, который содержит информацию о каждом пуле
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    /// <summary>
    /// Спавнит объект из пула или создает новый, если объект отсутствует в пуле.
    /// </summary>
    /// <param name="objectToSpawn">Объект, который нужно создать или взять из пула.</param>
    /// <param name="parentTransform">Родительский объект (Transform), к которому будет прикреплен созданный объект.</param>
    /// <returns>Ссылка на созданный или взятый из пула объект.</returns>
    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        // Ищем пул с объектом, имя которого совпадает с objectToSpawn.name
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

        if (pool == null) // Если пул не найден, создаем новый
        {
            pool = new PooledObjectInfo
            {
                lookupString = objectToSpawn.name, // Устанавливаем имя объекта для поиска
            };
            objectPools.Add(pool); // Добавляем новый пул в список пулов
        }

        // Пытаемся найти первый неактивный объект в пуле
        GameObject go = pool.inactiveObjects.FirstOrDefault();

        if (go == null) // Если неактивных объектов нет, создаем новый объект
        {
            go = Instantiate(objectToSpawn, parentTransform); // Создаем объект с привязкой к родителю
        }
        else // Если нашли неактивный объект
        {
            pool.inactiveObjects.Remove(go); // Удаляем его из списка неактивных объектов
            go.SetActive(true); // Активируем объект
        }

        // Возвращаем объект
        return go;
    }

    /// <summary>
    /// Возвращает объект обратно в пул, делая его неактивным.
    /// </summary>
    /// <param name="obj">Объект, который нужно вернуть в пул.</param>
    public static void ReturnObjectToPool(GameObject obj)
    {
        // Извлекаем имя объекта без последних 7 символов (например, "(Clone)")
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        // Ищем соответствующий пул для объекта по его имени
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == goName);

        if (pool != null) // Если пул найден
        {
            obj.SetActive(false); // Деактивируем объект
            pool.inactiveObjects.Add(obj); // Добавляем его в список неактивных объектов пула
        }
    }

    /// <summary>
    /// Структура для хранения информации о пуле объектов.
    /// </summary>
    public class PooledObjectInfo
    {
        public string lookupString; // Уникальный идентификатор для поиска (обычно имя объекта)
        public List<GameObject> inactiveObjects = new List<GameObject>(); // Список неактивных объектов в пуле
    }
}
