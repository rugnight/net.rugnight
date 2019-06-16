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
        public delegate void SceneUnloadEvent(HierarchicalScene scene, object returnValue);

        List<HierarchicalScene> children = new List<HierarchicalScene>();

        static List<string> loadingScenes = new List<string>();

        public object Argments { get; private set; } = null;

        public object ReturnValue { get; protected set; } = null;

        public SceneUnloadEvent OnSceneUnload;

        public int ChildCount => children.Count((_data) => _data != null);

        public void LoadScene(string sceneName, bool bWaitSceneEnd, object argments, SceneUnloadEvent returnValue)
        {
            StartCoroutine(CoLoadScene(sceneName, bWaitSceneEnd, argments, returnValue));
        }

        public IEnumerator CoLoadScene(string sceneName, bool bWaitSceneEnd, object argments, SceneUnloadEvent onSceneUnload)
        {
            Scene scene = new Scene();
            HierarchicalScene hierarchicalScene = null;

            // 同名シーンのロードタイミングをずらす
            // 同フレームで同じシーンが読まれることもあるので
            // 不要な時は yield return を通らないようにする
            if (loadingScenes.Contains(sceneName))
            {
                var waitSameSceneLoad = new WaitWhile(() => loadingScenes.Contains(sceneName));
                yield return waitSameSceneLoad;
            }
            loadingScenes.Add(sceneName);

            UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoadedAction = (_scene, _mode) =>
            {
                if (_scene.name != sceneName)
                {
                    return;
                }

                scene = _scene;
                loadingScenes.Remove(sceneName);

                foreach (var obj in _scene.GetRootGameObjects())
                {
                    hierarchicalScene = obj.GetComponentInChildren<HierarchicalScene>(true);
                    if (hierarchicalScene != null)
                    {
                        hierarchicalScene.Argments = argments;

                        hierarchicalScene.OnSceneUnload += onSceneUnload;

                        children.Add(hierarchicalScene);
                        break;
                    }
                }
            };

            SceneManager.sceneLoaded += sceneLoadedAction;
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= sceneLoadedAction;

            if (bWaitSceneEnd)
            {
                var wait = new WaitWhile(() => scene.IsValid());
                yield return wait;
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

            this.OnSceneUnload?.Invoke(this, this.ReturnValue);

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

        // --------------------------------------------------
        // for MonoBehaviour
        // --------------------------------------------------

    }
}
