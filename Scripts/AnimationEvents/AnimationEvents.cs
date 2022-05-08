using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    /// <summary>
    /// 重复使用的动画如枪口火焰等
    /// </summary>
    /// <param name="visible"></param>
    public void SetSpriteRendererVisible(int visible)
    {
        if (visible == 1)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().Play("0");
        }
    }

    /// <summary>
    /// 子弹爆炸后销毁自身
    /// </summary>
    public void BulletBoomRecycle()
    {
        GetComponent<Animator>().Play("0");
        GetComponent<BulletBase>().Recycle();
    }
}
