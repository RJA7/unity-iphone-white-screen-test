using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressableBundleCreator : EditorWindow
{
    [MenuItem("Tools/Generate Image Addressable Bundles")]
    public static void ShowWindow()
    {
        GetWindow<AddressableBundleCreator>("Bundle Generator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Create Bundles from Images"))
        {
            CreateBundles();
        }
    }

    private static void CreateBundles()
    {
        string imagesFolder = "Assets/Art";
        string ext = ".png";
        int fileIndex = 1;
        int[] bundleSizes = { 50, 40, 30, 20, 10, 1 };
        int bundlesPerSize = 10;

        // Load AddressableAssetSettings
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
        if (settings == null)
        {
            Debug.LogError(
                "AddressableAssetSettings not found! Make sure you have Addressables package installed and initialized."
            );
            return;
        }

        // Track current image index
        for (int sizeIndex = 0; sizeIndex < bundleSizes.Length; sizeIndex++)
        {
            int bundleSize = bundleSizes[sizeIndex];
            for (int bundleNum = 0; bundleNum < bundlesPerSize; bundleNum++)
            {
                string groupName = $"Bundle_{bundleSize}_{bundleNum + 1}";
                AddressableAssetGroup group = settings.FindGroup(groupName);
                if (group == null)
                {
                    var schemas = new List<AddressableAssetGroupSchema>();
                    foreach (var schema in settings.DefaultGroup.Schemas)
                    {
                        schemas.Add(ScriptableObject.Instantiate(schema));
                    }
                    group = settings.CreateGroup(groupName, false, false, false, schemas);
                }

                for (int i = 0; i < bundleSize; i++)
                {
                    string imagePath = $"{imagesFolder}/Image ({fileIndex}){ext}";
                    if (File.Exists(imagePath))
                    {
                        AddressableAssetEntry entry = settings.CreateOrMoveEntry(
                            AssetDatabase.AssetPathToGUID(imagePath),
                            group,
                            false,
                            false
                        );
                    }
                    else
                    {
                        Debug.LogWarning($"Image not found: {imagePath}");
                    }
                    fileIndex++;
                }
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Image bundles created and added to Addressables!");
    }
}
