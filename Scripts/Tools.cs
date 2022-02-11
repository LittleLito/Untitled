using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tools
{
    public static List<T> Apply<T>(List<T> list, Action<T> func)
    {
        foreach (var obj in list)
        {
            func(obj);
        }

        return list;
    }

    public static EnemyBase RandomEnemyWithWeight(List<EnemyBase> enemies)
    {
        if (enemies.Count == 0)
        {
            throw new Exception();
        }

        var totalWeight = enemies.Aggregate(0, (all, next) => all + next.WEIGHT);
        var cursor = 0;
        var random = Random.Range(0, totalWeight);
        foreach (var enemy in enemies)
        {
            cursor += enemy.WEIGHT;
            if (cursor > random)
            {
                return enemy;
            }
        }

        return null;
    }

    /// <summary>
    /// 用某个轴去朝向物体
    /// </summary>
    /// <param name="trSelf">朝向的本体</param>
    /// <param name="lookPos">朝向的目标</param>
    public static float AxisLookAt(Transform trSelf, Vector3 lookPos)
    {
        /*var dui = lookPos.y - trSelf.position.y;
        var lin = -(lookPos.x - trSelf.position.x);
        
        Debug.Log(dui);
        Debug.Log(lin);

        var ang = Mathf.Atan(dui / lin) * Mathf.Rad2Deg;
        var a = -(ang + 90);
        
        Debug.Log(a);
        
        trSelf.rotation = Quaternion.Euler(0, 0, a);*/

        Vector2 dir = lookPos - trSelf.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trSelf.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        return dir.y / dir.x;
    }
}