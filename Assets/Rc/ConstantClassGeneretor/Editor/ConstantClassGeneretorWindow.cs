using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/// <summary>
/// 各名前定数クラス作成ウィンドウ
/// </summary>
public class ConstantClassGeneretorWindow : EditorWindow
{
    [MenuItem("Window/Rc/ConstantClassGeneretor")]
    private static void Open()
    {
        GetWindow<ConstantClassGeneretorWindow>("ConstantClassGeneretor");
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void OnGUI()
    {
        // 出力先はエディタに保存しておく
        var outputDir = EditorPrefs.GetString("net.rugnight.NameClassCreatorWindow.outputDir", "Assets");

        using (var scope = new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("出力先ディレクトリ", GUILayout.Width(150));
            outputDir = EditorGUILayout.TextField(outputDir);

            if (GUI.changed)
            {
                EditorPrefs.SetString("net.rugnight.NameClassCreatorWindow.outputDir", outputDir);
            }
        }

        // シーン名
        {
            var fileName = outputDir + "/SceneName.cs";
            var classDesc = "シーン名を定数で管理するクラス";
            var nameClassCreator = new ConstantClassGeneretor(fileName, classDesc);

            GUI.enabled = nameClassCreator.CanCreate();

            if (GUILayout.Button(fileName + "作成"))
            {
                foreach (var n in AssetDatabase.FindAssets("t:scene")
                    .Select(c => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(c)))
                    .Distinct()
                    .Select(c => new { var = ConstantClassGeneretor.RemoveInvalidChars(c), val = c }))
                {
                    nameClassCreator.AddBodyLine(string.Format(@"public const string {0} = ""{1}"";", n.var, n.val));
                }
                nameClassCreator.CreateScript();

                EditorUtility.DisplayDialog(fileName, fileName + " の作成が完了しました", "OK");
            }
        }

        // レイヤー名
        {
            var fileName = outputDir + "/LayerName.cs";
            var classDesc = "シーン名を定数で管理するクラス";
            var nameClassCreator = new ConstantClassGeneretor(fileName, classDesc);

            GUI.enabled = nameClassCreator.CanCreate();

            if (GUILayout.Button(fileName + "作成"))
            {
                foreach (var n in InternalEditorUtility.layers.
                    Select(c => new { var = ConstantClassGeneretor.RemoveInvalidChars(c), val = LayerMask.NameToLayer(c) }))
                {
                    nameClassCreator.AddBodyLine(string.Format(@"public const int {0} = {1};", n.var, n.val));
                }
                foreach (var n in InternalEditorUtility.layers.
                    Select(c => new { var = ConstantClassGeneretor.RemoveInvalidChars(c), val = 1 << LayerMask.NameToLayer(c) }))
                {
                    nameClassCreator.AddBodyLine(string.Format(@"public const int {0}Mask = {1};", n.var, n.val));
                }
                nameClassCreator.CreateScript();

                EditorUtility.DisplayDialog(fileName, fileName + " の作成が完了しました", "OK");
            }
        }
    }
}

