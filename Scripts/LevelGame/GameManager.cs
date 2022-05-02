using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // 配置资源
    public GameConfig GameConfig { get; private set; }


    private void Awake()
    {
        Instance = this;
        GameConfig = Resources.Load<GameConfig>("GameConfig");
    }
    
}
