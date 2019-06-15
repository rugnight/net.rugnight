using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rc;

public class HierarchicalSceneTest : HierarchicalScene
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) && this.ChildCount == 0)
        {
            StartCoroutine(CoLoadScene(SceneName.HierarchicalSceneTest, false, null, null));
        }
    }
}
