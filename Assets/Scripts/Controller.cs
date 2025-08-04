using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AllocMethod
{
    NativeArray,
    UnsafeUtility,
}

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject _runButton;

    [SerializeField]
    private GameObject _instructionInput;

    [SerializeField]
    private GameObject scrollViewContent;

    [SerializeField]
    private GameObject _label;

    [SerializeField]
    private GameObject _allocationPrefab;

    [SerializeField]
    private GameObject _allocMethodDropdown;

    private List<AllocationController> _allocations = new();

    void Start()
    {
        _runButton.GetComponent<Button>().onClick.AddListener(HandleRunClick);
        _instructionInput.GetComponent<TMP_InputField>().text = "100|T|100|T|100|F";

        var dropdown = _allocMethodDropdown.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(Enum.GetNames(typeof(AllocMethod))));
    }

    private async void HandleRunClick()
    {
        var inputText = _instructionInput.GetComponent<TMP_InputField>().text;
        var values = inputText.Trim().Replace("\n", "").Split('|');
        var dropdown = _allocMethodDropdown.GetComponent<TMP_Dropdown>();
        var allocMethod = (AllocMethod)dropdown.value;

        foreach (var value in values)
        {
            if (value == "T")
            {
                await Awaitable.NextFrameAsync();
                continue;
            }

            if (value == "F" && _allocations.Count > 0)
            {
                RemoveAllocation(_allocations.Last());
                continue;
            }

            if (float.TryParse(value, out float allocationSize))
            {
                var allocation = Instantiate(_allocationPrefab, scrollViewContent.transform)
                    .GetComponent<AllocationController>();
                allocation.Init(allocationSize, allocMethod);
                _allocations.Add(allocation);

                allocation
                    .freeButton.GetComponent<Button>()
                    .onClick.AddListener(() =>
                    {
                        RemoveAllocation(allocation);
                    });
            }
        }
    }

    void Update()
    {
        PerformanceLogger.LogPerformanceInfo(_label, GetTotalAllocationSize());
    }

    private float GetTotalAllocationSize()
    {
        return _allocations.Aggregate(
            0f,
            (acc, allocation) => acc + allocation.GetAllocationSize()
        );
    }

    private void RemoveAllocation(AllocationController allocation)
    {
        _allocations.Remove(allocation);
        Destroy(allocation.gameObject);
    }
}
