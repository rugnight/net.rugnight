using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rc
{
    /// <summary>
    /// Json形式で保存する設定ファイル
    /// </summary>
    public class SettingsFile
    {

        virtual public void Save(string path)
        {
            var json = JsonUtility.ToJson(this, true);
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (var ws = new StreamWriter(fs))
            {
                try
                {
                    ws.Write(json);
                }
                catch (Exception)
                {
                    Debug.LogErrorFormat("{0} の保存に失敗", path);
                }
            }
        }

        virtual public void Load(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (var rs = new StreamReader(fs))
                {
                    string json = rs.ReadToEnd();
                    JsonUtility.FromJsonOverwrite(json, this);
                }
            }
            catch (Exception)
            {
                // 初回作成時はないのが普通
            }
        }

#if UNITY_EDITOR
        // ==================================================
        // エディタ向けの保存機能
        // ==================================================

        public string EditorSettingFilePath 
        {
            get { return Path.ChangeExtension(Path.Combine(Application.dataPath + "/../ProjectSettings", this.GetType().Name), ".json"); }
        }

        public void SaveForEditorSettings()
        {
            this.Save(EditorSettingFilePath);
        }

        public void LoadForEditorSettings()
        {
            this.Load(EditorSettingFilePath);
        }
#endif
    }
} // namespace rc
