using System;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AllocationController : MonoBehaviour
{
    [SerializeField]
    private GameObject amount;

    [SerializeField]
    public GameObject freeButton;

    private float _allocationSize;

    private AllocMethod _allocMethod;

    private unsafe void* _allocationPtr;

    private NativeArray<float> _nativeArray;

    public void Init(float allocationSize, AllocMethod allocMethod)
    {
        _allocationSize = allocationSize;
        _allocMethod = allocMethod;

        var allocMethodShort = string.Join(
            "",
            Enum.GetName(typeof(AllocMethod), allocMethod)!
                .ToCharArray()
                .Where(c => c.ToString().ToUpper() == c.ToString())
        );
        amount.GetComponent<TextMeshProUGUI>().text = $"{allocationSize}MB {allocMethodShort}";

        switch (_allocMethod)
        {
            case AllocMethod.UnsafeUtility:
                UnsafeUtilityAllocate();
                break;
            case AllocMethod.NativeArray:
                NativeArrayAllocate();
                break;
            default:
                Debug.LogError($"AllocMethod {_allocMethod} is not supported");
                break;
        }
    }

    public void OnDestroy()
    {
        switch (_allocMethod)
        {
            case AllocMethod.UnsafeUtility:
                UnsafeUtilityFree();
                break;
            case AllocMethod.NativeArray:
                NativeArrayFree();
                break;
            default:
                Debug.LogError($"AllocMethod {_allocMethod} is not supported");
                break;
        }
    }

    public float GetAllocationSize()
    {
        return _allocationSize;
    }

    private unsafe void UnsafeUtilityAllocate()
    {
        var bytes = (long)(_allocationSize * 1024 * 1024);
        var alignment = 16;
        _allocationPtr = UnsafeUtility.Malloc(bytes, alignment, Allocator.Persistent);
    }

    private unsafe void UnsafeUtilityFree()
    {
        UnsafeUtility.Free(_allocationPtr, Allocator.Persistent);
    }

    private void NativeArrayAllocate()
    {
        var bytes = (int)(_allocationSize * 1024 * 1024);
        var elementSize = sizeof(float); // 4 bytes for float
        var elementCount = bytes / elementSize;

        _nativeArray = new NativeArray<float>(elementCount, Allocator.Persistent);
    }

    private void NativeArrayFree()
    {
        _nativeArray.Dispose();
    }
}
