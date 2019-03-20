using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;
using rc.constants;

namespace rc
{

    public class ScreenGrapher : EditorWindow {

        // ===========================================================================
        // 変数
        // ===========================================================================

        // 保存先の情報
        static readonly string m_saveDirName = "ScreenShot";
        static readonly string m_saveDirPath = System.IO.Path.Combine(Application.persistentDataPath, m_saveDirName);

        // 撮影サイズ
        static int m_size = 0;

        // 遅延撮影時間
        float m_delayTime = 1.0f;

        // 撮影間隔
        float m_timeSpan = 1.0f;
        // 撮影時間
        float m_duration = 5.0f;

        // シーンロード後撮影のON/OFF
        bool m_bSceneLoadShot = false;
        // シーンロード後何秒後に撮影するか
        float m_sceneLoadShotDelayTime = 0.5f;

        // 撮影コルーチンリスト
        List<IEnumerator> m_coroutines = new List<IEnumerator>();

        // ===========================================================================
        // Static 関数
        // ===========================================================================

        // メニューウィンドウ
        [MenuItem(EditorExtension.TOOL_MENU_ROOT + "ScreenGrapher")]
        static void CreateWindow() {
            GetWindow<ScreenGrapher>();
        }

        // 初期設定
        static ScreenGrapher() {
            Debug.Log("Start ScreenGrapher");

            // 保存先がなければ作成
            if (!System.IO.Directory.Exists(m_saveDirPath)) {
                System.IO.Directory.CreateDirectory(m_saveDirPath);
                Debug.Log("Create Directory " + m_saveDirPath);
            }

            // キー入力撮影用のイベントを登録
            FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
            functions += OnEditorUpdate;
            info.SetValue(null, (object)functions);
        }

        // エディタの更新に合わせて呼ばれる更新
        static void OnEditorUpdate() {
            // キー入力があれば撮影
            if (Event.current.type == EventType.KeyUp) {
                if (Event.current.keyCode == (KeyCode.A)) {
                    Capture();
                }
            }
        }

        // キャプチャ実行
        static public void Capture() {
            string filepath = m_saveDirPath + "/" + DateTime.Now.ToString("yyMMddHHmmssff") + ".png";
            ScreenCapture.CaptureScreenshot(filepath, m_size);
            Debug.Log("Screen Shot " + filepath);
        }

        // ===========================================================================
        // 関数
        // ===========================================================================

        // 有効になったときにシーンロード監視を開始
        void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // 無効時にシーンロード監視を終了
        void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // シーンロード後の撮影処理
        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (m_bSceneLoadShot) {
                m_coroutines.Add(CoDelayShot(m_sceneLoadShotDelayTime));
            }
        }

        // メニューの描画
        void OnGUI() {

            // --------------------------------------------------
            // 全体設定
            EditorGUILayout.LabelField("全体設定");
            EditorGUILayout.LabelField("キャプチャサイズ");
            m_size = EditorGUILayout.IntSlider(m_size, 0, 4);

            if (GUILayout.Button("保存先を開く")) {
                EditorUtility.OpenWithDefaultApp(m_saveDirPath);
            }

            // --------------------------------------------------
            // シーンロード後の自動撮影
            EditorGUILayout.LabelField("シーン読込後自動撮影");
            using (new EditorGUILayout.HorizontalScope()) {
                m_bSceneLoadShot = EditorGUILayout.Toggle(m_bSceneLoadShot);
                EditorGUILayout.LabelField("シーン読込後");
                m_sceneLoadShotDelayTime = EditorGUILayout.FloatField(m_sceneLoadShotDelayTime);
                EditorGUILayout.LabelField("秒後に撮影");
            }

            // --------------------------------------------------
            // 遅延撮影
            EditorGUILayout.LabelField("遅延撮影");
            using (new EditorGUILayout.HorizontalScope()) {
                m_delayTime = EditorGUILayout.FloatField(m_delayTime);
                EditorGUILayout.LabelField("秒後に撮影");
            }

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("開始")) {
                m_coroutines.Add(CoDelayShot(m_delayTime));
            }
            GUI.enabled = true;

            // --------------------------------------------------
            // 連続撮影
            EditorGUILayout.LabelField("連続撮影");
            using (new EditorGUILayout.HorizontalScope()) {
                m_timeSpan = EditorGUILayout.FloatField(m_timeSpan);
                EditorGUILayout.LabelField("秒ごとに");
                m_duration = EditorGUILayout.FloatField(m_duration);
                EditorGUILayout.LabelField("秒間撮影");
            }
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("開始")) {
                m_coroutines.Add(CoBurstShot(m_timeSpan, m_duration));
            }
            GUI.enabled = true;
        }

        // 撮影コルーチンの更新
        void Update() {
            for (int i = m_coroutines.Count - 1; 0 <= i; --i) {
                if (!m_coroutines[i].MoveNext()) {
                    m_coroutines.RemoveAt(i);
                }
            }
        }

        // 遅延撮影用コルーチン
        IEnumerator CoDelayShot(float delayTime) {
            float startTime = Time.time;
            while ((Time.time - startTime) < delayTime) {
                yield return null;
            }
            ScreenGrapher.Capture();
        }

        // 連続撮影用コルーチン
        IEnumerator CoBurstShot(float timeSpan, float duration) {
            float startTime = Time.time;
            float preShotTime = startTime;
            ScreenGrapher.Capture();
            while ((Time.time - startTime) < duration) {
                if (timeSpan <= (Time.time - preShotTime)) {
                    ScreenGrapher.Capture();
                    preShotTime = Time.time;
                }
                yield return null;
            }
        }
    }

} // namespace rc
