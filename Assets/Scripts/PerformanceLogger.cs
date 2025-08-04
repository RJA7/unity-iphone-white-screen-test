using System;
using TMPro;
using UnityEngine;

public class PerformanceLogger
{
    public static void LogPerformanceInfo(GameObject label, float totalAllocated)
    {
        var totalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
        var monoHeap = GC.GetTotalMemory(false);
        var fps = 1f / Time.deltaTime;

        var text =
            $"FPS: {fps:F2} \n"
            + $"Reserved: {FormatBytes(totalReserved)} \n"
            + $"Mono Heap: {FormatBytes(monoHeap)} \n"
            + $"Total : {totalAllocated:F2} \n";

        label.GetComponent<TextMeshProUGUI>().text = text;
    }

    static string FormatBytes(long bytes)
    {
        return $"{(bytes / (1024f * 1024f)):F2} MB";
    }
}
