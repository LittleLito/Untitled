using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
        transform.position = new Vector3(0, -0.9f, -10);
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
    /// 移动摄像机
    /// </summary>
    public void Move(float destY, float speed, LevelState? nextState)
    {
        StartCoroutine(DoMove(destY, speed, nextState));
    }

    private IEnumerator DoMove(float destY, float speed, LevelState? nextState)
    {
        // 目标位置和方向
        var dest = new Vector3(transform.position.x, destY, -10);
        var direction = (dest - transform.position).normalized;
        
        // 移动
        while (Vector3.Distance(dest, transform.position) > 0.2f)
        {
            yield return new WaitForSeconds(0.04f);
            transform.Translate(direction * speed); // 飞
        }

        // 移动结束，改变关卡状态（如果需要）
        if (nextState != null) {
            LevelManager.Instance.LevelState = (LevelState) nextState;
        }
        
    }
}
