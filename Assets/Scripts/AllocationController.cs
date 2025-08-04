using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class AllocationController : MonoBehaviour
{
    public GameObject freeButton;

    public GameObject label;

    public Range range;

    private AsyncOperationHandle<Texture2D>[] handles;

    public async Task Init(Range range)
    {
        this.range = range;

        var paths = new List<string>();

        for (var i = range.Start; i < range.End; i++)
        {
            paths.Add($"Assets/Art/Image ({i}).png");
        }

        handles = paths.Select(Addressables.LoadAssetAsync<Texture2D>).ToArray();

        var imagesCount = range.End - range.Start;
        var compressed = imagesCount * Config.CompressedImageSize;
        var uncompressed = imagesCount * Config.UncompressedImageSize;
        label.GetComponent<TextMeshProUGUI>().text = $"{compressed:F2}/{uncompressed:F2}MB";

        freeButton.SetActive(false);

        while (handles.All(handle => !handle.IsDone))
        {
            await Awaitable.NextFrameAsync();
        }

        freeButton.SetActive(true);
    }

    public void OnDestroy()
    {
        Array.ForEach(handles, Addressables.Release);
    }
}
