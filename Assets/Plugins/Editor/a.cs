using TMPro;
using UnityEditor;
using UnityEngine;

public class MyTools : EditorWindow
{
    static SceneAsset objs;
    static MyTools window;
    TMP_FontAsset changeFont;
    TMP_FontAsset target;
 
    private void OnGUI()
    {
        ChangeFont();
    }
 
    [MenuItem("MY/字体设置")]
    public static void InitFont()
    {
        //objs = (SceneAsset)Selection.objects[0];
        window = (MyTools)GetWindow(typeof(MyTools));
        window.titleContent.text = "字体设置";
        window.position = new Rect(PlayerSettings.defaultScreenWidth / 2, PlayerSettings.defaultScreenHeight / 2, 400, 160);
        window.Show();
    }
 
    private void ChangeFont()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("！！！需要先打开并选择对应的预设！！！");
        GUILayout.BeginHorizontal();
        GUILayout.Label("NewFont:");
        target = (TMP_FontAsset)EditorGUILayout.ObjectField(target, typeof(TMP_FontAsset), true, GUILayout.MinWidth(100f));
        changeFont = (TMP_FontAsset)EditorGUILayout.ObjectField(changeFont, typeof(TMP_FontAsset), true, GUILayout.MinWidth(100f));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("确认"))
        {
            var tArray = Resources.FindObjectsOfTypeAll(typeof(TMP_Text));
            for (int i = 0; i < tArray.Length; i++)
            {
                TMP_Text t = tArray[i] as TMP_Text;
                //提交修改，如果没有这个代码，unity不会察觉到编辑器有改动，同时改动也不会被保存
                Undo.RecordObject(t, t.gameObject.name);
                if (changeFont && t.font == target) t.font = changeFont;
            }
            window.Close();
        }
        GUILayout.EndVertical();
    }
}
