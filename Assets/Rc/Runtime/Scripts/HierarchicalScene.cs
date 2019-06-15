using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rc
{
    public class HierarchicalScene : MonoBehaviour
    {
        List<HierarchicalScene> children = new List<HierarchicalScene>();

        public object Argments { get; private set; } = null;

        public object ReturnValue { get; protected set; } = null;

        public int ChildCount => children.Count((_data) => _data != null);

        public IEnumerator CoLoadScene(string sceneName, bool bWaitSceneEnd, object argments, Action<object> returnValue)
        {
            Scene scene = new Scene();
            HierarchicalScene hierarchicalScene = null;
            SceneManager.sceneLoaded += (_scene, _mode) =>
            {
                scene = _scene;
                foreach (var obj in _scene.GetRootGameObjects())
                {
                    hierarchicalScene = obj.GetComponentInChildren<HierarchicalScene>(true);
                    if (hierarchicalScene != null)
                    {
                        hierarchicalScene.Argments = argments;
                        children.Add(hierarchicalScene);
                        break;
                    }
                }
            };

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (bWaitSceneEnd)
            {
                var wait = new WaitWhile(() => scene.IsValid());
                yield return wait;
                returnValue?.Invoke(hierarchicalScene?.ReturnValue);
            }
        }

        [ContextMenu("UnloadScene")]
        public void UnloadScene()
        {
            StartCoroutine(CoUnloadScene());
        }

        public IEnumerator CoUnloadScene()
        {
            yield return CoUnloadChildScenes();
            yield return SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        public IEnumerator CoUnloadChildScenes()
        {
            for (int i = children.Count - 1; 0 <= i; --i)
            {
                if (children[i] == null)
                {
                    continue;
                }
                yield return children[i].CoUnloadScene();
            }
        }
    }
}
