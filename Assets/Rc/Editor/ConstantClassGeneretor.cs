using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Rc.constants;

namespace Rc
{
    // Assets/Test/ConstantClassGeneretor

    /// <summary>
    /// 名前定数化クラス作成
    /// 参考 http://baba-s.hatenablog.com/entry/2014/02/26/000000
    /// </summary>
    [Serializable]
    public class ConstantClassGeneretor
    {
        // 無効な文字を管理する配列
        private static readonly string[] INVALUD_CHARS =
        {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

        string classPath = null;
        string classDesc = null;
        StringBuilder classBodyBuilder = new StringBuilder();

        private ConstantClassGeneretor() { }

        public ConstantClassGeneretor(string classPath, string classDesc)
        {
            this.classPath = classPath;
            this.classDesc = classDesc;
        }

        /// <summary>
        /// クラス本体の行を追加
        /// </summary>
        /// <param name="line"></param>
        public void AddBodyLine(string line)
        {
            classBodyBuilder.Append("\t").AppendLine(line);
        }

        /// <summary>
        /// スクリプトを作成します
        /// </summary>
        public void CreateScript()
        {
            var builder = new StringBuilder();

            builder.AppendLine("/// <summary>");
            builder.AppendLine("/// " + this.classDesc);
            builder.AppendLine("/// </summary>");
            builder.AppendFormat("public static class {0}", Path.GetFileNameWithoutExtension(this.classPath)).AppendLine();
            builder.AppendLine("{");

            builder.Append(classBodyBuilder);

            builder.AppendLine("}");

            var directoryName = Path.GetDirectoryName(this.classPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(this.classPath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }

        /// <summary>
        /// 生成可能か
        /// </summary>
        /// <returns></returns>
        public bool CanCreate()
        {
            bool editorCanCreate = !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
            if (!editorCanCreate)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.classPath))
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.classDesc))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 無効な文字を削除します
        /// </summary>
        public static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
            return str;
        }
    }

}// namespace rc 
