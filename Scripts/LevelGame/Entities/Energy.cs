using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public enum EnergyType
{
    Null,
	Sun,
    SunOnReactor,
	Moon,
    MoonOnReactor
}

public class Energy : MonoBehaviour
{
    private EnergyType _energyType;
    public EnergyType EnergyType 
    {
        set
        {
            if (_energyType.Equals(value)) return;

            _energyType = value;

            switch (value)
            {
                case EnergyType.Sun:
                    speed = Random.Range(0.8f, 1.5f);
                    animator.Play("SunEnergy", 0, 0f);
                    light2D.color = Color.yellow;
                    light2D.pointLightOuterRadius = 2.42f;
                    light2D.intensity = 0.51f;
                    _point = 500;
                    break;
                case EnergyType.SunOnReactor:
                    animator.Play("SunEnergy", 0, 0f);
                    light2D.color = Color.yellow;
                    light2D.pointLightOuterRadius = 2.42f;
                    light2D.intensity = 0.17f;
                    break;
                case EnergyType.Moon:
                    speed = Random.Range(0.6f, 1f);
                    animator.Play("MoonEnergy", 0, 0f);
                    _point = 250;
                    light2D.color = Color.white;
                    light2D.pointLightOuterRadius = 1.5f;
                    light2D.intensity = 0.17f;
                    break;
                case EnergyType.MoonOnReactor:
                    animator.Play("MoonEnergy", 0, 0f);
                    light2D.color = Color.white;
                    light2D.pointLightOuterRadius = 1.5f;
                    light2D.intensity = 0.17f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
    private int _point = 500;
    public float speed;
    private float _fallingStopY;
    private bool _collected;
    public PolygonCollider2D polygonCollider2D;
    public Animator animator;
    public Light2D light2D;


    public void InitForSky(EnergyType type, float fallingStopY, Vector2 pos)
    {
        _fallingStopY = fallingStopY; // 下落终点
        transform.position = pos; // 下落起始位置
        _collected = false;

        EnergyType = type;
        polygonCollider2D.enabled = true;
    }

    public void InitForReactor(EnergyType type, Vector2 pos)
    {
        transform.position = pos;
        _fallingStopY = 10000f;
        _collected = true;

        EnergyType = type;
        polygonCollider2D.enabled = false;

    }
    
    // Update is called once per frame
    private void Update()
    {
        if (LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        if (_collected) return;

        if (transform.position.y <= _fallingStopY)
        {
            Invoke(nameof(Recycle), 10); // 超过十秒能量消失
            return;
        }

        transform.Translate(Vector3.down * (Time.deltaTime * speed));
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
        
        transform.DOMove(energyPointsPos, 10).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                PlayerManager.Instance.EnergyPoints += _point;
                Recycle();
            });
        _collected = true;
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