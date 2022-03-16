using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LocationManager : MonoBehaviour
{
    private String _type;
    private List<Vector2> _playerLocations = new List<Vector2>
    {
        new Vector2(0f, 0.464f),
        new Vector2(-0.156f, 0.174f),
        new Vector2(0.156f, 0.174f),
        new Vector2(0.342f, -0.008f),
        new Vector2(-0.342f, -0.008f),
        new Vector2(0.544f, -0.234f),
        new Vector2(-0.544f, -0.234f),
        new Vector2(0.147f, -0.224f),
        new Vector2(-0.147f, -0.224f),
        new Vector2(0f, -0.34f)
    };
    private List<Location> _locations = new List<Location>();

    public void Init(string type)
    {
        if (type == "Player")
        {
            _type = type;
            for (var i = 0; i < _playerLocations.Count; i++)
            {
                _locations.Add(new Location(i + 1, _playerLocations[i], true, transform.parent));
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 通过世界坐标获取网格坐标
    /// </summary>
    /// <returns></returns>
    public Location GetLocationByWorldPos(Vector2 worldPos)
    {
        var dis = 1000000f;
        Location location = null;

        foreach (var location1 in _locations)
        {
            var parent = transform.parent;
            if (Vector2.Distance(worldPos, location1.GetWorldPosition()) < dis)
            {
                
                dis = Vector2.Distance(worldPos, location1.GetWorldPosition());
                location = location1;

            }
        }

        return location;
    }

    public Location GetLocationByMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var location = GetLocationByWorldPos(mousePos);
        return Vector2.Distance(mousePos, location.GetWorldPosition()) < 0.3f ? location : null;
    }
}

public class Location
{
    public int Num;
    public Vector2 LocalPosition;
    public Transform Parent;
    
    // 有装备与否
    private EquipBase _equipBase;
    public EquipBase EquipBase
    {
        get => _equipBase;
        set
        {
            if (_equipBase == value)
            {
                return;
            }

            // 装备被卸掉
            if (value is null)
            {
                PlayerManager.Instance.Speed += 0.2f;
            }
            // 装备被装上
            else if (_equipBase is null)
            {
                PlayerManager.Instance.Speed -= 0.2f;
            }
            
            _equipBase = value;
        }
    }

    public Location()
    {
        Num = 0;
        LocalPosition = Vector2.positiveInfinity;
        Parent = null;
        EquipBase = null;
    }
    public Location(int num, Vector2 pos, bool available, Transform parent)
    {
        Num = num;
        LocalPosition = pos;
        Parent = parent;
        EquipBase = null;
    }

    public Vector2 GetWorldPosition()
    {
        return GetWorldPosition(Parent);
    }
    public Vector2 GetWorldPosition(Transform parent)
    {
        return (Vector2) parent.position + LocalPosition;
    }
}
