using UnityEngine;

public class EnergyReactor : EquipBase
{
    public override int Cost => 500;
    public override float CD => 7.5f;
    public override EquipType Type => EquipType.EnergyReactor;
    public override EquipFamily Family => EquipFamily.Enlighten;

    private bool _canCreate;
    private float _createCD => LevelManager.Instance.LevelInfo.IsNight ? Random.Range(60f, 70f) : Random.Range(18f, 24f);


    /// <summary>
    /// 放置时初始化
    /// </summary>
    public override void Place()
    {
        base.Place();
        
        // 生成能量动画（懒得再画了）
        var energy = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Energy, transform);
        energy.transform.position = transform.position;
        energy.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        Destroy(energy.GetComponent<PolygonCollider2D>());
        energy.GetComponent<Energy>().EnergyType = EnergyType.SunOnReactor;
        Destroy(energy.GetComponent<Energy>());

        Invoke(nameof(CreateEnergy), Random.Range(6f, 10f));
    }


    // Update is called once per frame
    private void Update()
    {
        if (!_canCreate || LevelManager.Instance.LevelState != LevelState.InGame && LevelManager.Instance.LevelState != LevelState.Boss) return;
        CreateEnergy();
    }

    /// <summary>
    /// 创建能量
    /// </summary>
    private void CreateEnergy()
    {
        _canCreate = false;
        // 生成能量
        var energy = PoolManager.Instance.GetGameObj(GameManager.Instance.GameConfig.Energy, null).GetComponent<Energy>();
        energy.InitForReactor(EnergyType.Sun, transform.position);
        // 跳跃动画
        StartCoroutine(energy.DoJump());
        Invoke(nameof(SetCanCreate), _createCD); // 计时开始
    }

    /// <summary>
    /// 计时结束后设置为可生成
    /// </summary>
    private void SetCanCreate()
    {
        _canCreate = true;
    }
}