using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using rc.constants;

namespace rc
{
    /// <summary>
    /// マスタ生成機能のウィンドウ
    /// </summary>
    public class MasterLoaderWindow : EditorWindow
    {
        const string WindowName = "MasterLoader";

        MasterLoaderSettings m_settings = new MasterLoaderSettings();

        Vector2 scrollPos = new Vector2();

        [MenuItem(EditorExtension.TOOL_MENU_ROOT + WindowName)]
        private static void Open()
        {
            var window = GetWindow<MasterLoaderWindow>(WindowName);
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnChangePlayMode;
            m_settings.LoadForEditorSettings();
        }

        private void OnDisable()
        {
            m_settings.SaveForEditorSettings();
            EditorApplication.playModeStateChanged -= OnChangePlayMode;
        }

        private void OnChangePlayMode(PlayModeStateChange playModeStateChange)
        {
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
            GUI.enabled = m_settings.IsReady();

            if (GUILayout.Button("マスタクラス生成"))
            {
                var infos = m_settings.masterInfoList.Where((info) => info.enable);
                foreach (var data in infos.Select((info, index) => new { info, index }))
                {
                    EditorUtility.DisplayProgressBar(string.Format("マスタクラス生成({0}/{1})", data.index, infos.Count()), data.info.masterName, (float)data.index/ (float)infos.Count());
                    var json = MasterLoader.RequestJson(data.info.masterName, data.info.sheetUrl, m_settings.apiUrl);
                    MasterLoader.CreateClassFile(json, m_settings.namespaceName, data.info.masterName, m_settings.assetDir, m_settings.accessorDir);
                }
                EditorUtility.ClearProgressBar();
            }
            if (GUILayout.Button("マスタアセット生成"))
            {
                var infos = m_settings.masterInfoList.Where((info) => info.enable);
                foreach (var data in infos.Select((info, index) => new { info, index }))
                {
                    EditorUtility.DisplayProgressBar(string.Format("マスタ生成({0}/{1})", data.index, infos.Count()), data.info.masterName, (float)data.index/ (float)infos.Count());
                    var json = MasterLoader.RequestJson(data.info.masterName, data.info.sheetUrl, m_settings.apiUrl);
                    MasterLoader.CreateAssetFile(json, m_settings.namespaceName, data.info.masterName, m_settings.assetDir);
                }
                EditorUtility.ClearProgressBar();
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
