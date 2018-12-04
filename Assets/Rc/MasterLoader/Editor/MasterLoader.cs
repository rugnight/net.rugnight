using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace rc
{
    /// <summary>
    /// Googleスプレッドシートからマスタを取得してScriptableObjectに流し込むクラス
    /// </summary>
    public class MasterLoader : Editor
    {

        /// <summary>
        /// スプレッドシートからマスタを取得する
        /// </summary>
        public static void LoadMaster(string sheetName, string sheetUrl, string apiUrl, string assetDir, string accessorDir)
        {
            string namespaceName = "Master";

            string json = RequestJson(sheetName, sheetUrl, apiUrl);

            CreateClassFile(json, namespaceName, sheetName, assetDir, accessorDir);

            CreateAssetFile(json, namespaceName, sheetName, assetDir);
        }

        /// <summary>
        /// GASからマスターデータのJSONデータを取得
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="sheetUrl"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        static public string RequestJson(string sheetName, string sheetUrl, string apiUrl)
        {
            var url = apiUrl + "?sheetName=" + sheetName + "&sheetUrl=" + sheetUrl;

            using (WWW www = new WWW(url))
            {
                while (www.MoveNext()) { }
                Debug.Log(www.text);

                return www.text;
            }
        }

        /// <summary>
        /// JSONからアセットファイルを生成
        /// </summary>
        /// <param name="jsonText"></param>
        /// <param name="namespaceName"></param>
        /// <param name="sheetName"></param>
        /// <param name="assetDir"></param>
        static public void CreateAssetFile(string jsonText, string namespaceName, string sheetName, string assetDir)
        {
            var assembly = System.Reflection.Assembly.Load("Assembly-CSharp");
            Type t = assembly.GetType(namespaceName + "." + sheetName);

            var json = SimpleJSON.JSON.Parse(jsonText);
            var jsonString = json["data"].ToString();

            var masterData = CreateInstance(t);
            JsonUtility.FromJsonOverwrite(jsonString, masterData);

            if (masterData != null)
            {
                string assetPath = assetDir + "/" + sheetName + ".asset";

                // ディレクトリがなければ作る
                var directoryName = Path.GetDirectoryName(assetPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                // すでにマスタが作成されているかを確認するために取得してみる
                var master = AssetDatabase.LoadAssetAtPath(assetPath, t);
                if (master != null)
                {
                    AssetDatabase.DeleteAsset(assetPath);
                    AssetDatabase.Refresh();
                }

                // マスタは不変の値なので、Unityでは編集できないようにする
                masterData.hideFlags = HideFlags.NotEditable;

                // マスタが取得できなければマスタを新規作成する
                AssetDatabase.CreateAsset(masterData, assetPath);
                AssetDatabase.Refresh();

                // Jsonの値をScriptableObjectに流し込む
                Debug.Log(sheetName + " load has completed");
            }
            else
            {
                // Jsonの取得に失敗している
                Debug.LogError(jsonText);
            }
        }

        /// <summary>
        /// JSONからマスター読み込みクラスを生成
        /// </summary>
        /// <param name="jsonText"></param>
        /// <param name="namespaceName"></param>
        /// <param name="className"></param>
        /// <param name="assetDir"></param>
        /// <param name="accessorDir"></param>
        static public void CreateClassFile(string jsonText, string namespaceName, string className, string assetDir, string accessorDir)
        {
            var paramClassName = className + "Parameters";
            var json = SimpleJSON.JSON.Parse(jsonText);

            var type = json["type"].AsArray;
            // データ行を1行取得
            var dataLine = type[0];

            var builderPrameters = new StringBuilder();
            var builderSerialization = new StringBuilder();

            foreach (var data in dataLine)
            {
                string varName = data.Key;
                string varNameParsed = varName + "Parsed";
                string propName = varName + "Data";

                if (data.Value.IsString)
                {
                    string dataValue = data.Value;
                    if (dataValue.ToLower() == "comment")
                    {
                        continue;
                    }
                    else if (dataValue.ToLower() == "int")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("int", varName, propName));
                    }
                    else if (dataValue.ToLower() == "uint")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("uint", varName, propName));
                    }
                    else if (dataValue.ToLower() == "long")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("long", varName, propName));
                    }
                    else if (dataValue.ToLower() == "ulong")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("ulong", varName, propName));
                    }
                    else if (dataValue.ToLower() == "float")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("float", varName, propName));
                    }
                    else if (dataValue.ToLower() == "double")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("double", varName, propName));
                    }
                    else if (dataValue.ToLower() == "string")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("string", varName, propName));
                    }
                    else if (dataValue.ToLower() == "intarray")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("List<int>", varName, propName));
                    }
                    else if (dataValue.ToLower() == "uintarray")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("List<uint>", varName, propName));
                    }
                    else if (dataValue.ToLower() == "floatarray")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("List<float>", varName, propName));
                    }
                    else if (dataValue.ToLower() == "doublearray")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("List<double>", varName, propName));
                    }
                    else if (dataValue.ToLower() == "stringarray")
                    {
                        builderPrameters.Append(MasterClassCreater.CreateDataCode("List<string>", varName, propName));
                    }
                    else if (dataValue.ToLower() == "datetime")
                    {
                        // 日付
                        builderPrameters.Append(MasterClassCreater.CreateDateTimeDataCode(varName, propName, varNameParsed));
                        // シリアライズ後にDateTime化
                        builderSerialization.Append(MasterClassCreater.CreateDateTimeSerializeCode(varName, varNameParsed));
                    }
                    else
                    {
                        // Enum
                        builderPrameters.Append(MasterClassCreater.CreateEnumDataCode(varName, propName, dataValue, varNameParsed));
                        // シリアライズ後にEnum化
                        builderSerialization.Append(MasterClassCreater.CreateEnumSerializeCode(varName, varNameParsed, dataValue));
                    }
                }
                builderPrameters.AppendLine();
            }

            // CSファイルを構築
            var builder = new StringBuilder();
            builder.Append(MasterClassCreater.CreateClassBodyCode(className, paramClassName, builderPrameters.ToString(), builderSerialization.ToString()));

            var filePath = accessorDir + "/" + className + ".cs";
            var directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            //AssetDatabase.Refresh();
        }

        static string GetAssetResourcePath(string assetDir, string className)
        {
            if (assetDir.Contains("Resources"))
            {
                int removeTo = assetDir.IndexOf("Resources") + "Resources".Length + 1;
                assetDir = assetDir.Remove(0, removeTo);
                return assetDir + "/" + className;
            }
            return assetDir + "/" + className;
        }

    }
} // namespace rc
