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
            return null;
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
    /// 用y轴去朝向物体
    /// </summary>
    /// <param name="trSelf">朝向的本体</param>
    /// <param name="lookPos">朝向的目标</param>
    public static void YLookAt(Transform trSelf, Vector3 lookPos)
    {
        Vector2 dir = lookPos - trSelf.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trSelf.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}