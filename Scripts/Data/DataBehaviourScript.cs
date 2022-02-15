using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameData.InitLevelInfos();
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
