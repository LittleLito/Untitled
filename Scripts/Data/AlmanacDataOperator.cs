using System;
using System.Collections.Generic;

[Serializable]
public class AlmanacDataOperator
{
    public List<EquipmentsData> EquipmentsDatas;
    public List<ProjectilesData> ProjectilesDatas;
    public List<EnemyData> EnemiesDatas;
}

[Serializable]
public class AlmanacDataBase
{
    public int Id;
}

[Serializable]
public class EquipmentsData : AlmanacDataBase
{
    public string Name;
    public string ChineseName;
    public string Conclusion;
    public double CD;
    public List<string> PropertyItems;
    public List<string> PropertyValues;
    public string Description;
}

[Serializable]
public class ProjectilesData : AlmanacDataBase
{
    public string Name;
    public string ChineseName;
    public string Conclusion;
    public List<string> PropertyItems;
    public List<string> PropertyValues;
    public string Description;
}

[Serializable]
public class EnemyData : AlmanacDataBase
{
    public string Name;
    public string ChineseName;
    public string Conclusion;
    public int Level;
    public int Weight;
    public int MaxHealth;
    public List<string> PropertyItems;
    public List<string> PropertyValues;
    public string Description;
}