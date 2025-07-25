using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Loader : MonoBehaviour {
    private readonly List<Texture2D> loadedTextures = new ();

    void Start() {
        StartCoroutine(LoadAllPNGsFromResources());
    }

    IEnumerator LoadAllPNGsFromResources() {
        yield return new WaitForSeconds(5f);

        for (int i = 1; i <= 128; i++) {
            string path = $"Art/Image ({i})";
            ResourceRequest request = Resources.LoadAsync<Texture2D>(path);
            yield return request;

            Texture2D texture = request.asset as Texture2D;
            
            if (texture != null) {
                loadedTextures.Add(texture);
                Debug.Log($"Successfully loaded {loadedTextures.Count} PNG(s).");
                yield return new WaitForSeconds(0.5f);
            } else {
                Debug.LogWarning($"Failed to load Art/Image ({i}).png from Resources.");
            }
        }
    }

    public int GetLoadedPngsCount() {
        return loadedTextures.Count;
    }
}
