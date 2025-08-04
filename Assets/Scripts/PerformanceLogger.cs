using System;
using TMPro;
using UnityEngine;

public class PerformanceLogger
{
    public static void LogPerformanceInfo(GameObject label, string customLog)
    {
        var totalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
        var monoHeap = GC.GetTotalMemory(false);
        var fps = 1f / Time.deltaTime;

        var text =
            $"FPS: {fps:F2} \n"
            + $"Reserved: {FormatBytes(totalReserved)} \n"
            + $"Mono Heap: {FormatBytes(monoHeap)} \n"
            + $"{customLog}";

        label.GetComponent<TextMeshProUGUI>().text = text;
    }

    static string FormatBytes(long bytes)
    {
        return $"{(bytes / (1024f * 1024f)):F2} MB";
    }
}
