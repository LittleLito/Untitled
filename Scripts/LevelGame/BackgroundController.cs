using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;

    public Texture bg1;
    public Texture bg2;
    
    public RawImage bg;
    public Animator animator;
    public Light2D light2D;

    private void Awake()
    {
        Instance = this;
    }

    public void InitBg(int chapterNum)
    {
        switch (chapterNum)
        {
            case 1:
                bg.texture = bg1;
                light2D.intensity = 1.21f;
                light2D.pointLightOuterRadius = 64.8f;
                animator.Play("0", 0, 0f);
                break;
            case 2:
                bg.texture = bg2;
                light2D.intensity = 1.8f;
                light2D.pointLightOuterRadius = 37.77f;
                animator.Play("Scroll", 0, 0f);
                break;
        }
    }
}
