using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rc
{

    /// <summary>
    /// マスタ生成機能のウィンドウ
    /// </summary>
    public class MasterLoaderWindow : EditorWindow
    {
        const string MenuPath = "Window/Rc/MasterLoader";
        const string WindowName = "MasterLoader";

        enum State {
            Clear,
            Idle,
            Request,
            CreateClass,
            WaitCompile,
            CreateAsset,
        }

        State m_state = State.Idle;

        MasterLoaderSettings m_settings = null;

        int m_processCount = 0;

        string m_processingJson = null;

        MasterLoaderSettings.MasterInfo m_processingInfo = null;

        Queue<MasterLoaderSettings.MasterInfo> m_processQueue = new Queue<MasterLoaderSettings.MasterInfo>();

        Vector2 scrollPos = new Vector2();

        [MenuItem(MenuPath)]
        private static void Open()
        {
            var window = GetWindow<MasterLoaderWindow>(WindowName);
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnChangePlayMode;
            m_settings = MasterLoaderSettings.Load();
            m_state = State.Clear;
        }

        private void OnDisable()
        {
            m_state = State.Clear;
            MasterLoaderSettings.Save(m_settings);
            EditorApplication.playModeStateChanged -= OnChangePlayMode;
        }

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
        }

        private void Update()
        {
            switch (m_state)
            {
                case State.Clear:
                    m_processingJson = null;
                    m_processingInfo = null;
                    m_state = State.Idle;
                    break;

                case State.Idle:
                    if (0 < m_processQueue.Count)
                    {
                        //m_processingInfo = m_processQueue.Dequeue();
                        m_processingInfo = m_processQueue.Peek();
                        m_state = State.Request;
                    }
                    break;

                case State.Request:
                    if (m_processingInfo != null)
                    {
                        m_processingJson = MasterLoader.RequestJson(m_processingInfo.masterName, m_processingInfo.sheetUrl, m_settings.apiUrl);
                        if (!string.IsNullOrEmpty(m_processingJson))
                        {
                            m_state = State.CreateClass;
                        }
                        else
                        {
                            m_state = State.Clear;
                        }
                    }
                    else
                    {
                        m_state = State.Clear;
                    }
                    break;

                case State.CreateClass:
                    MasterLoader.CreateClassFile(m_processingJson, m_settings.namespaceName, m_processingInfo.masterName, m_settings.assetDir, m_settings.accessorDir);
                    AssetDatabase.Refresh();
                    m_state = State.WaitCompile;
                    break;

                case State.WaitCompile:
                    if (!EditorApplication.isCompiling && !EditorApplication.isUpdating)
                    {
                        m_state = State.CreateAsset;
                    }
                    break;

                case State.CreateAsset:
                    MasterLoader.CreateAssetFile(m_processingJson, m_settings.namespaceName, m_processingInfo.masterName, m_settings.assetDir);
                    m_processQueue.Dequeue();
                    m_state = State.Clear;
                    break;

            }
            // プログレスバー
            if (m_processCount != 0 && 0 < m_processQueue.Count)
            {
                var progress = 1.0f - ((float)m_processQueue.Count / (float)m_processCount);
                var info = string.Format("{0} ({1}/{2})", m_processQueue.Peek().masterName, (m_processCount - m_processQueue.Count), m_processCount);
                EditorUtility.DisplayProgressBar("MasterLoader", info, progress);
            }
            else
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void OnGUI()
        {
            // API URL
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("API URL", GUILayout.Width(150));
                m_settings.apiUrl = EditorGUILayout.TextField(m_settings.apiUrl);
            }

            // アクセスクラス出力先
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("アクセスクラス出力先", GUILayout.Width(150));
                m_settings.accessorDir = EditorGUILayout.TextField(m_settings.accessorDir).TrimEnd('/');
            }

            // マスタアセット出力先
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("マスタアセット出力先", GUILayout.Width(150));
                m_settings.assetDir = EditorGUILayout.TextField(m_settings.assetDir).TrimEnd('/');
            }

            // マスタクラス名前空間
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("マスタクラス名前空間", GUILayout.Width(150));
                m_settings.namespaceName = EditorGUILayout.TextField(m_settings.namespaceName).Trim();
            }

            // 生成に必要な情報が入力されていなければUI無効に
            GUI.enabled = m_settings.IsReady() && (0 == m_processQueue.Count);

            if (GUILayout.Button("マスタ生成"))
            {
                m_processCount = 0;
                foreach (var info in m_settings.masterInfoList)
                {
                    if (info.enable)
                    {
                        m_processCount++;
                        m_processQueue.Enqueue(info);
                    }
                }
            }

            GUI.enabled = true;

            if (GUILayout.Button("追加"))
            {
                m_settings.AddNew();
            }
            m_settings.DrawGUI(ref scrollPos);

        }

#if false
    /// <summary>
    /// ツールバーを描画する
    /// </summary>
    private void DrawToolbar()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.ExpandWidth(true)))
        {
            GUILayout.Button("button", EditorStyles.toolbarButton);
            GUILayout.Toggle(true, "toggle", EditorStyles.toolbarButton);
            EditorGUILayout.IntPopup(0, new string[] { "popup" }, new int[] { 0 }, EditorStyles.toolbarPopup);
            EditorGUILayout.IntPopup(0, new string[] { "dropdown" }, new int[] { 0 }, EditorStyles.toolbarDropDown);
            EditorGUILayout.TextField("", EditorStyles.toolbarTextField, GUILayout.Width(150));
        }
    }
#endif

        /// <summary>
        /// Selectionが切り替わった時に呼ばれる
        /// </summary>
        private void OnSelectionChange()
        {
            Debug.Log(Selection.activeObject);

            // 表示内容を変更し、即座に反映したい場合にはRepaint()
            Repaint();
        }

#if false
    private void Update()
    {
        // 他のwindowがフォーカスされているときの処理
        if (focusedWindow.titleContent.text == "Hierarchy")
        {
        }
    }
#endif
    }

} // namespace rc 
