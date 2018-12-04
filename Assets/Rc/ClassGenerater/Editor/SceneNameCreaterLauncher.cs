using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// シーンインポート時にシーン名定義を更新します
/// </summary>
public class SceneNameCreaterLauncher : AssetPostprocessor
{
    const string SCENE_ASSET_EXT = ".unity";

    public static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
    {
        bool bExistSceneAsset = importedAssets.Any( (assetName) => assetName.EndsWith(SCENE_ASSET_EXT))
            || deletedAssets.Any( (assetName) => assetName.EndsWith(SCENE_ASSET_EXT))
            || movedAssets.Any( (assetName) => assetName.EndsWith(SCENE_ASSET_EXT));
        if (bExistSceneAsset)
        {
            SceneNameCreator.Create();
        }
    }

}
