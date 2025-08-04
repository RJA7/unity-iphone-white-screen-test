using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    private readonly List<Texture2D> loadedTextures = new();
    private AssetBundle imagesBundle;

    void Start()
    {
        StartCoroutine(LoadAllPNGsFromBundle());
    }

    IEnumerator LoadAllPNGsFromBundle()
    {
        yield return new WaitForSeconds(5f);

        string bundleUrl = Path.Combine(Application.streamingAssetsPath, "mainbundle");

#if UNITY_WEBGL && !UNITY_EDITOR
    // For WebGL, the path is already a URL
#else
        bundleUrl = "file://" + bundleUrl; // For Editor/Standalone
#endif

        Debug.Log("Loading bundle from: " + bundleUrl);

        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download AssetBundle: " + www.error);
            yield break;
        }

        imagesBundle = DownloadHandlerAssetBundle.GetContent(www);

        if (imagesBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle from content!");
            yield break;
        }

        for (int i = 1; i <= 300; i++)
        {
            string assetName = $"Image ({i})";
            AssetBundleRequest request = imagesBundle.LoadAssetAsync<Texture2D>(assetName);
            yield return request;

            Texture2D texture = request.asset as Texture2D;

            if (texture != null)
            {
                loadedTextures.Add(texture);
                Debug.Log($"Successfully loaded {loadedTextures.Count} PNG(s).");
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                Debug.LogWarning($"Failed to load {assetName} from AssetBundle.");
            }
        }
    }

    public int GetLoadedPngsCount()
    {
        return loadedTextures.Count;
    }

    public void releaseFirstTexture()
    {
        if (loadedTextures.Count > 0)
        {
            Destroy(loadedTextures[0]);
            loadedTextures.RemoveAt(0);
            Debug.Log("First texture released.");
        }
        else
        {
            Debug.Log("No textures to release.");
        }
    }

    void OnDestroy()
    {
        if (imagesBundle != null)
            imagesBundle.Unload(false);
    }
}
