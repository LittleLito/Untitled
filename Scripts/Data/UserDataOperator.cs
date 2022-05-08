using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class UserDataOperator
{
    private static UserData _userData;
    public static UserData UserData
    {
        get => _userData ?? LoadUserData();
        set => _userData = value;
    }

    private static string _path = Application.streamingAssetsPath + "/userData.save";

    /// <summary>
    /// 读取玩家的数据
    /// </summary>
    /// <returns></returns>
    public static UserData LoadUserData()
    {
        // 如果路径上有文件，就读取文件
        if (File.Exists(_path))
        {
            // 读取数据
            var bf = new BinaryFormatter();
            var fileStream = File.Open(_path, FileMode.Open);
            _userData = (UserData) bf.Deserialize(fileStream);
            fileStream.Close();
        }
        // 如果没有文件，就创建一个PlayerData
        else
        {
            _userData = new UserData();
        }

        return _userData;
    }

    /// <summary>
    /// 保存玩家的数据
    /// </summary>
    public static void SaveUserData()
    {
        var bf = new BinaryFormatter();
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }

        var fileStream = File.Create(_path);
        bf.Serialize(fileStream, _userData);
        fileStream.Close();
        
    }
}
