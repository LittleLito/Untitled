using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Energy : MonoBehaviour
{
    private const int Point = 500;
    public float Speed;
    public float FallingStopY;
    private bool collected = false;

    public void InitForSky(float fallingStopY, Vector2 pos)
    {
        FallingStopY = fallingStopY; // 下落终点
        transform.position = pos; // 下落起始位置
        Speed = Random.Range(0.8f, 1.2f);
        collected = false;

        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void InitForReactor(Vector2 pos)
    {
        transform.position = pos;
        FallingStopY = 10000f;
        collected = true;

        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

    }
    
    // Update is called once per frame
    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (collected) return;

        if (transform.position.y <= FallingStopY)
        {
            Invoke(nameof(Recycle), 10); // 超过十秒能量消失
            return;
        }

        transform.Translate(Vector3.down * (Time.deltaTime * Speed));
    }

    /// <summary>
    /// 收集能量
    /// </summary>
    public void Collect()
    {
        if (Camera.main is null) return;
        
        // 取消消失计时
        CancelInvoke();

        // 转换目的地坐标
        var energyPointsPos = Camera.main.ScreenToWorldPoint(new Vector3(75.7f, 153.5f, 0f));
        energyPointsPos.z = 0;
        
        transform.DOMove(energyPointsPos, 10).SetSpeedBased().OnComplete(
            () =>
            {
                PlayerManager.Instance.EnergyPoints += Point;
                Recycle();
            });
        collected = true;
    }
    

    /// <summary>
    /// 反应堆产生能量时，跳一下
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoJump()
    {
        var startPos = transform.position;
        var isLeft = Random.Range(0, 2) == 0;
        float x;
        // 如果向左边跳跃
        if (isLeft)
        {
            x = -0.01f;
        }
        else // 右边~
        {
            x = 0.01f;
        }

        while (transform.position.y <= startPos.y + 0.8f) // 跳~
        {
            yield return new WaitForSeconds(0.005f);
            transform.Translate(x, 0.05f, 0f);
        }

        while (transform.position.y >= startPos.y) // 下落~
        {
            yield return new WaitForSeconds(0.005f);
            transform.Translate(x, -0.05f, 0f);
        }

        // 可以收了
        Collect();
    }

    /// <summary>
    /// 回收废旧游戏对象
    /// </summary>
    public void Recycle()
    {
        // 取消全部协程和延迟调用
        StopAllCoroutines();
        CancelInvoke();

        // 回库
        PoolManager.Instance.PushGameObj(GameManager.Instance.GameConfig.Energy, gameObject);
    }
}