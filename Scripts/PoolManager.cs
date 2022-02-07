using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 缓存池模块，用于节省性能
/// </summary>
public class PoolManager
{
    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PoolManager();
            }

            return _instance;
        }
    }

    private GameObject _poolObj;

    // 数据字典
    // key->   预制体
    // value-> 具体obj
    private readonly Dictionary<GameObject, List<GameObject>> _poolDataDic = new Dictionary<GameObject, List<GameObject>>();

    /// <summary>
    /// 通过预制体获取字典中的资源
    /// </summary>
    /// <param name="prefab">预制体（键）</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public GameObject GetGameObj(GameObject prefab, Transform parent)
    {
        GameObject obj;
        // 如果有这个预制体的键，同时也有值
        if (_poolDataDic.ContainsKey(prefab) && _poolDataDic[prefab].Count > 1)
        {
            // 返回list中的第一个
            obj = _poolDataDic[prefab][0];
            _poolDataDic[prefab].RemoveAt(0);
        }
        // 没有则创建
        else
        {
            obj = Object.Instantiate(prefab);
        }

        obj.SetActive(true);
        obj.transform.SetParent(parent);
        return obj;
    }

    /// <summary>
    /// 将对象存回缓存池
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="obj"></param>
    public void PushGameObj(GameObject prefab, GameObject obj)
    {
        // 检查是否存在根游戏对象
        if (_poolObj == null) _poolObj = new GameObject("PoolManager");

        // 字典中有
        if (_poolDataDic.ContainsKey(prefab))
        {
            // 存进去
            _poolDataDic[prefab].Add(obj);
        }
        // 字典中没有
        else
        {
            // 创建
            _poolDataDic.Add(prefab, new List<GameObject> {obj});
        }

        // 如果没有子目录，则创建
        if (!_poolObj.transform.Find(prefab.name))
        {
            new GameObject(prefab.name).transform.SetParent(_poolObj.transform);
        }

        // 隐藏
        obj.SetActive(false);
        // 设置父物体
        obj.transform.SetParent(_poolObj.transform.Find(prefab.name));
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearGameObj()
    {
        _poolDataDic.Clear();
    }

}