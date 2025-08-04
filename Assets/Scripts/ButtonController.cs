using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private int rangeStart;

    [SerializeField]
    private int rangeSize;

    private List<Range> ranges = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (var i = 0; i < 10; i++)
        {
            var range = new Range { Start = rangeStart + i * rangeSize };
            range.End = range.Start + rangeSize;
            ranges.Add(range);
        }

        UpdateLabel();
    }

    public Range PopRange()
    {
        if (ranges.Count == 0)
        {
            return null;
        }

        var range = ranges[^1];
        ranges.Remove(range);
        UpdateLabel();

        return range;
    }

    public void AddRange(Range range)
    {
        ranges.Add(range);
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        var compressed = rangeSize * Config.CompressedImageSize;
        var uncompressed = rangeSize * Config.UncompressedImageSize;

        gameObject.GetComponentInChildren<TMP_Text>().text =
            $"{compressed:F2}/{uncompressed:F2}MB ({ranges.Count} left)";

        gameObject.SetActive(ranges.Count > 0);
    }
}
