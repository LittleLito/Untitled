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
}
