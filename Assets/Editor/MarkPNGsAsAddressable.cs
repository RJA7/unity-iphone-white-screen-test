using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;

public class MarkPNGsAsAddressable
{
    [MenuItem("Tools/Mark PNGs In Folder As Addressables")]
    static void MarkAllPNGsInFolder()
    {
        // Set your folder path here (relative to the Assets folder)
        string folderPath = "Assets/Art"; // Change to your folder!

        // Get all png files in the folder (non-recursive)
        string[] pngFiles = Directory.GetFiles(folderPath, "*.png", SearchOption.TopDirectoryOnly);

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found. Make sure Addressables package is installed.");
            return;
        }

        foreach (string filePath in pngFiles)
        {
            string assetPath = filePath.Replace('\\', '/');
            var guid = AssetDatabase.AssetPathToGUID(assetPath);

            // Check if the asset is already addressable
            AddressableAssetEntry entry = settings.FindAssetEntry(guid);
            if (entry == null)
            {
                // Add asset as addressable to the default group
                settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                Debug.Log($"Marked {assetPath} as Addressable.");
            }
            else
            {
                Debug.Log($"{assetPath} is already Addressable.");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Done marking PNGs as Addressables.");
    }
}
