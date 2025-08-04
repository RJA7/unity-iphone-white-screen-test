using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Loader : MonoBehaviour
{
    private readonly List<Texture2D> loadedTextures = new();
    private List<AsyncOperationHandle<Texture2D>> handles = new();

    void Start()
    {
        StartCoroutine(LoadAllPNGsFromAddressables());
    }

    IEnumerator LoadAllPNGsFromAddressables()
    {
        yield return new WaitForSeconds(5f); // Keep your initial delay if needed

        for (int i = 1; i <= 300; i++)
        {
            string address = $"Assets/Art/Image ({i}).png";

            // Start loading
            AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>(address);
            handles.Add(handle);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Texture2D texture = handle.Result;
                loadedTextures.Add(texture);
                Debug.Log($"Successfully loaded {loadedTextures.Count} PNG(s) from Addressables.");
                yield return new WaitForSeconds(0.2f); // Optional, for pacing
            }
            else
            {
                Debug.LogWarning($"Failed to load {address} from Addressables.");
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
            // Release Addressables handle instead of Destroy()
            Addressables.Release(handles[0]);
            loadedTextures.RemoveAt(0);
            handles.RemoveAt(0);
            Debug.Log("First texture released.");
        }
        else
        {
            Debug.Log("No textures to release.");
        }
    }

    void OnDestroy()
    {
        // Release all Addressables handles to clean up memory
        foreach (var handle in handles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        handles.Clear();
    }
}
