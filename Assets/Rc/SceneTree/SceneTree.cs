using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace rc.scene
{
    static public class SceneTree
    {

        // シーンから子のリストを取得
        static Dictionary<Scene, List<Scene>> m_childrenDictionary = new Dictionary<Scene, List<Scene>>();
        // シーンから親を取得
        static Dictionary<Scene, Scene> m_parentDictionary = new Dictionary<Scene, Scene>();

        // シーンのロードオプション
        public enum LoadOption
        {
            LoadEnd,        // ロード完了まで待つ
            SceneEnd,       // シーン終了まで待つ
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            SceneManager.sceneUnloaded += onSceneUnloaded;
        }

        static public void LoadScene(string sceneName)
        {
            GameObject go = new GameObject("LoadChildScene " + sceneName);
            SceneLoadTask task = go.AddComponent<SceneLoadTask>();
            task.parent = new Scene();
            task.sceneName = sceneName;
            task.loadOption = LoadOption.LoadEnd;
        }

        static public void LoadChildScene(Scene parent, string sceneName)
        {
            GameObject go = new GameObject("LoadChildScene " + sceneName);
            SceneLoadTask task = go.AddComponent<SceneLoadTask>();
            task.parent = parent;
            task.sceneName = sceneName;
            task.loadOption = LoadOption.LoadEnd;
        }

        static public void LoadSiblingScene(Scene sibling, string sceneName)
        {
            if (!m_parentDictionary.ContainsKey(sibling))
            {
                Debug.LogErrorFormat("{0} はシーン管理に登録されていません", sibling);
                return;
            }
            LoadChildScene(m_parentDictionary[sibling], sceneName);
        }

        static public IEnumerator LoadChildScene(Scene parent, string sceneName, LoadOption loadOption = LoadOption.SceneEnd)
        {
            // ロード完了イベント登録
            bool bLoaded = false;
            UnityAction<Scene, LoadSceneMode> onLoaded = (Scene scene, LoadSceneMode loadSceneMode) =>
            {
                bLoaded = true;
                onSceneLoaded(parent, scene, loadSceneMode);
            };
            SceneManager.sceneLoaded += onLoaded;

            // アンロード完了イベント登録
            bool bUnloaded = false;
            UnityAction<Scene> onUnloaded = (Scene scene) =>
            {
                bUnloaded = true;
            };
            SceneManager.sceneUnloaded += onUnloaded;

            // ロードリクエスト
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // ロード待ち
            while (!bLoaded)
            {
                yield return null;
            }
            if (LoadOption.LoadEnd == loadOption)
            {
                SceneManager.sceneLoaded -= onLoaded;
                SceneManager.sceneUnloaded -= onUnloaded;
                yield break;
            }

            // シーン終了待ち
            while (!bUnloaded)
            {
                yield return null;
            }
            SceneManager.sceneLoaded -= onLoaded;
            SceneManager.sceneUnloaded -= onUnloaded;
        }

        static public void UnloadScene(Scene scene)
        {
            GameObject go = new GameObject("UnloadScene " + scene.name);
            SceneUnloadTask task = go.AddComponent<SceneUnloadTask>();
            task.scene = scene;
        }

        static public IEnumerator CoUnloadScene(Scene scene)
        {
            // 子を先に削除
            if (m_childrenDictionary.ContainsKey(scene))
            {
                for (int i = m_childrenDictionary[scene].Count - 1; 0 <= i; --i)
                {
                    IEnumerator childUnload = CoUnloadScene(m_childrenDictionary[scene][i]);
                    while (childUnload.MoveNext())
                    {
                        yield return null;
                    }
                }
            }
            // 自分を削除
            if (1 < SceneManager.sceneCount)
            {
                AsyncOperation async = SceneManager.UnloadSceneAsync(scene);
                while (async != null && false == async.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                // 最後のシーン、アプリケーションを終了
                Application.Quit();
            }
            yield return null;
        }

        // シーンツリーのパスを取得
        static string GetPath(Scene scene)
        {
            // 親がいない、ルート
            if (!m_parentDictionary.ContainsKey(scene))
            {
                return scene.name;
            }

            StringBuilder sb = new StringBuilder();
            Scene parent = scene;
            while (true)
            {
                sb.Insert(0, "/" + parent.name);
                if (m_parentDictionary.ContainsKey(parent))
                {
                    parent = m_parentDictionary[parent];
                }
                else
                {
                    break;
                }
            }
            return sb.ToString();
        }

        static void onSceneLoaded(Scene parent, Scene child, LoadSceneMode loadSceneMode)
        {
            // 親を登録
            m_parentDictionary.Add(child, parent);

            // 子を登録
            if (!m_childrenDictionary.ContainsKey(parent))
            {
                m_childrenDictionary.Add(parent, new List<Scene>());
            }
            m_childrenDictionary[parent].Add(child);

            Debug.LogFormat("シーン {0}:{1} をロード", GetPath(child), child.GetHashCode());
        }

        static void onSceneUnloaded(Scene scene)
        {
            Debug.LogFormat("シーン {0}:{1} をアンロード", GetPath(scene), scene.GetHashCode());

            // 子を登録解除
            Scene parent = m_parentDictionary[scene];
            m_childrenDictionary[parent].Remove(scene);

            // 親を登録解除
            m_parentDictionary.Remove(scene);
        }
    }

    class SceneLoadTask : MonoBehaviour
    {

        public Scene parent { get; set; }
        public string sceneName { get; set; }
        public SceneTree.LoadOption loadOption { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            StartCoroutine(CoTask());
        }

        IEnumerator CoTask()
        {
            yield return StartCoroutine(SceneTree.LoadChildScene(parent, sceneName, loadOption));
            GameObject.Destroy(gameObject);
        }
    }

    class SceneUnloadTask : MonoBehaviour
    {

        public Scene scene { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            StartCoroutine(CoTask());
        }

        IEnumerator CoTask()
        {
            yield return StartCoroutine(SceneTree.CoUnloadScene(scene));
            GameObject.Destroy(gameObject);
        }
    }

    

}
