using UnityEngine;
using System;
using TMPro;

public class PerformanceLogger : MonoBehaviour {
    [SerializeField]
    private GameObject label;
    
    [SerializeField]
    private GameObject loader;

    private float logTimer;
    private float logInterval = 0.5f; // Log every 0.5s

    void Update() {
        logTimer += Time.deltaTime;

        if (logTimer >= logInterval) {
            logTimer = 0f;
            LogPerformanceInfo();
        }
    }

    void LogPerformanceInfo() {
        // Memory info in bytes
        long totalAllocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        long totalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
        long totalUnusedReserved = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong();
        long monoHeap = GC.GetTotalMemory(false);

        // FPS
        float fps = 1f / Time.deltaTime;

        var text = $"FPS: {fps:F2} \n" +
            $"Allocated: {FormatBytes(totalAllocated)} \n" +
            $"Reserved: {FormatBytes(totalReserved)} \n" +
            $"Unused Reserved: {FormatBytes(totalUnusedReserved)} \n" +
            $"Mono Heap: {FormatBytes(monoHeap)} \n" +
            $"Loaded pngs count: {loader.GetComponent<Loader>().GetLoadedPngsCount()}";

        Debug.Log(text);
        label.GetComponent<TextMeshProUGUI>().text = text;
    }

    string FormatBytes(long bytes) {
        if ( bytes > 1024 * 1024 ) {
            return $"{(bytes / (1024f * 1024f)):F2} MB";
        }

        if ( bytes > 1024 ) {
            return $"{(bytes / 1024f):F2} KB";
        }

        return $"{bytes} B";
    }
}