using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static public class ProjectDirectoryMaker {
    const string MenuBase = "RC/ディレクトリセット作成";
    const string BasePath = "Assets/NewScene";

    [MenuItem(MenuBase + "/シーン用フォルダセットを作成")]
    static public void MakeSceneFolders() {
        //if ( !AssetDatabase.IsValidFolder(BasePath + "/Scenes")) {
        //    AssetDatabase.CreateFolder(BasePath, "Scenes");
        //}
        if (!AssetDatabase.IsValidFolder(BasePath)) {
            AssetDatabase.CreateFolder("Assets", "NewScene");
        }
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Prefabs")) {
            AssetDatabase.CreateFolder(BasePath, "Prefabs");
        }
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Scripts")) {
            AssetDatabase.CreateFolder(BasePath, "Scripts");
        }
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Animations")) {
            AssetDatabase.CreateFolder(BasePath, "Animations");
        }
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Materials")) {
            AssetDatabase.CreateFolder(BasePath, "Materials");
        }
        //if ( !AssetDatabase.IsValidFolder(BasePath + "/Physics Materials")) {
        //    AssetDatabase.CreateFolder(BasePath, "Physics Materials");
        //}
        //if ( !AssetDatabase.IsValidFolder(BasePath + "/Fonts")) {
        //    AssetDatabase.CreateFolder(BasePath, "Fonts");
        //}
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Textures")) {
            AssetDatabase.CreateFolder(BasePath, "Textures");
        }
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Audio")) {
            AssetDatabase.CreateFolder(BasePath, "Audio");
        }
        //if ( !AssetDatabase.IsValidFolder(BasePath + "/Resources")) {
        //    AssetDatabase.CreateFolder(BasePath, "Resources");
        //}
        if ( !AssetDatabase.IsValidFolder(BasePath + "/Editor")) {
            AssetDatabase.CreateFolder(BasePath, "Editor");
        }
        //if ( !AssetDatabase.IsValidFolder(BasePath + "/Plugins")) {
        //    AssetDatabase.CreateFolder(BasePath, "Plugins");
        //}



        //foreach(string guid in Selection.assetGUIDs) {
        //    string path = AssetDatabase.GUIDToAssetPath(guid);
        //    if ( AssetDatabase.IsValidFolder(path) ) {
        //        AssetDatabase.CreateFolder(path, "");
        //    }
        //}
    }

    [MenuItem(MenuBase + "/システム用フォルダセットを作成")]
    static public void MakeProjectFolders() {
        if ( !AssetDatabase.IsValidFolder("Assets/Scenes")) {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Prefabs")) {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Scripts")) {
            AssetDatabase.CreateFolder("Assets", "Scripts");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Animations")) {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Materials")) {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Physics Materials")) {
            AssetDatabase.CreateFolder("Assets", "Physics Materials");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Fonts")) {
            AssetDatabase.CreateFolder("Assets", "Fonts");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Textures")) {
            AssetDatabase.CreateFolder("Assets", "Textures");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Audio")) {
            AssetDatabase.CreateFolder("Assets", "Audio");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Resources")) {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Editor")) {
            AssetDatabase.CreateFolder("Assets", "Editor");
        }
        if ( !AssetDatabase.IsValidFolder("Assets/Plugins")) {
            AssetDatabase.CreateFolder("Assets", "Plugins");
        }



        //foreach(string guid in Selection.assetGUIDs) {
        //    string path = AssetDatabase.GUIDToAssetPath(guid);
        //    if ( AssetDatabase.IsValidFolder(path) ) {
        //        AssetDatabase.CreateFolder(path, "");
        //    }
        //}
    }
}
