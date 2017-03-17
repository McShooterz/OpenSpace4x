#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        Directory.CreateDirectory("AssetBundles");

        string[] files = Directory.GetFiles("AssetBundles");
        foreach (var asset in files)
        {
            File.Delete(asset);
        }

        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);

        files = Directory.GetFiles("AssetBundles");

        foreach (string asset in files)
        {
            if (!asset.Contains("."))
            {
                File.Move(asset, asset + ".unity3d");
            }
            else
            {
                File.Delete(asset);
            }
        }
    }
}
#endif
