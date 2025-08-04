using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject scrollViewContent;

    [SerializeField]
    private GameObject _label;

    [SerializeField]
    private GameObject _allocationPrefab;

    private List<AllocationController> _allocations = new();

    public GameObject[] buttons;

    public GameObject clearCacheAndReloadButton;

    void Start()
    {
        Array.ForEach(
            buttons,
            button =>
            {
                button
                    .GetComponent<Button>()
                    .onClick.AddListener(async () =>
                    {
                        var buttonController = button.GetComponent<ButtonController>();
                        var range = buttonController.PopRange();

                        if (range == null)
                            return;

                        var allocation = Instantiate(_allocationPrefab, scrollViewContent.transform)
                            .GetComponent<AllocationController>();
                        _allocations.Add(allocation);

                        allocation
                            .freeButton.GetComponent<Button>()
                            .onClick.AddListener(() =>
                            {
                                RemoveAllocation(allocation);
                                buttonController.AddRange(range);
                            });

                        await allocation.Init(range);
                    });
            }
        );

        clearCacheAndReloadButton.GetComponent<Button>().onClick.AddListener(ClearCacheAndReload);

#if UNITY_EDITOR
        clearCacheAndReloadButton.SetActive(false);
#endif
    }

    void Update()
    {
        var imagesCount = GetTotalImagesCount();
        var compressed = imagesCount * Config.CompressedImageSize;
        var uncompressed = imagesCount * Config.UncompressedImageSize;
        PerformanceLogger.LogPerformanceInfo(_label, $"Total: {compressed:F2}/{uncompressed:F2}MB");
    }

    private float GetTotalImagesCount()
    {
        return _allocations.Aggregate(
            0f,
            (acc, allocation) => acc + allocation.range.End - allocation.range.Start
        );
    }

    private void RemoveAllocation(AllocationController allocation)
    {
        _allocations.Remove(allocation);
        Destroy(allocation.gameObject);
    }

    [DllImport("__Internal")]
    private static extern void ClearCacheAndReload();
}
