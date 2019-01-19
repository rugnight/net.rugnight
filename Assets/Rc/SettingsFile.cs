using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc
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
    }

#if UNITY_EDITOR
    /// <summary>
    /// ProjectSettingsに設定ファイルを保存する
    /// </summary>
    public class EditorSettingsFile : SettingsFile
    {
        public string FilePath { get { return Path.ChangeExtension(Path.Combine(Application.dataPath + "/../ProjectSettings", this.GetType().Name), ".json"); } }

        public void Save()
        {
            base.Save(FilePath);
        }

        public void Load()
        {
            base.Load(FilePath);
        }
    }
#endif // UNITY_EDITOR

} // namespace rc