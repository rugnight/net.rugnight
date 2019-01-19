using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rc
{
    [System.Serializable]
    public class MasterLoaderSettings : EditorSettingsFile
    {
        const string SaveKey = "RcMasterLoader";

        public string apiUrl = "";          // https://script.google.com/macros/s/AKfycbwcOHroxRUt-HrKXJIl8CufhetXELCoV-XC0bhg_IybVn0mVwJA/exec
        public string assetDir = "";        // Assets/otherworldly/Resources/Master
        public string accessorDir = "";     // Assets/otherworldly/Scripts/Master
        public string namespaceName = "";

        public List<MasterInfo> masterInfoList = new List<MasterInfo>();

        [System.Serializable]
        public class MasterInfo
        {
            public string masterName = "";  // シート名
            public string sheetUrl = "";    // GoogleScreadSheetのURL
            public bool enable = false;
        }

        // 新しいマスタ設定の追加
        public MasterInfo AddNew()
        {
            var newItem = new MasterInfo();
            masterInfoList.Add(newItem);
            return newItem;
        }

        // 設定が準備完了か
        public bool IsReady()
        {
            return (!string.IsNullOrEmpty(apiUrl) && !string.IsNullOrEmpty(assetDir) && !string.IsNullOrEmpty(accessorDir) && !string.IsNullOrEmpty(namespaceName));
        }

        // GUI描画
        public void DrawGUI(ref Vector2 scrollPos)
        {
            // 列タイトル
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("変換", GUILayout.Width(30));
                EditorGUILayout.LabelField("シート名", GUILayout.Width(100));
                EditorGUILayout.LabelField("シートURL", GUILayout.Width(500));
            }

            // マスタ情報
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = scrollView.scrollPosition;
                foreach (var info in masterInfoList)
                {
                    // 1行
                    using (var scope = new EditorGUILayout.HorizontalScope())
                    {
                        info.enable = EditorGUILayout.Toggle(info.enable, GUILayout.Width(30));
                        info.masterName = EditorGUILayout.TextField(info.masterName, GUILayout.Width(100));
                        info.sheetUrl = EditorGUILayout.TextField(info.sheetUrl, GUILayout.Width(500));
                        if (GUILayout.Button("開く", GUILayout.Width(50)))
                        {
                            Application.OpenURL(info.sheetUrl);
                        }
                        if (GUILayout.Button("削除", GUILayout.Width(50)))
                        {
                            if (EditorUtility.DisplayDialog("確認", "本当に削除しますか？", "削除", "キャンセル"))
                            {
                                masterInfoList.Remove(info);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
} // namespace rc