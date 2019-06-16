using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rc;

public class HierarchicalSceneTest : HierarchicalScene
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = gameObject.scene.GetHashCode().ToString();
        if (this.Argments != null)
        {
            Debug.Log("Start : " + gameObject.name + " Args : " + this.Argments.ToString());
        }
        this.ReturnValue = gameObject.name;
    }

    private void OnDestroy()
    {
        Debug.Log("Destroy : " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) && this.ChildCount == 0)
        {
            StartCoroutine(CoLoadScene(
                SceneName.HierarchicalSceneTest,
                false,
                "Parent is " + gameObject.name,
                (scene, ret) => { Debug.Log("Returned " + ret + " to " + gameObject.name); } 
                ));
            StartCoroutine(CoLoadScene(
                SceneName.HierarchicalSceneTest,
                false,
                "Parent is " + gameObject.name,
                (scene, ret) => { Debug.Log("Returned " + ret + " to " + gameObject.name); } 
                ));
        }
    }
}
