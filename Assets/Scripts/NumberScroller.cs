using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class NumberScroller : MonoBehaviour
{
    public string prefix;

    private TMP_Text display;
    private float current, target;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var scrollSpeed = Mathf.Max(10f, Mathf.Abs(target - current));
        current = Mathf.MoveTowards(current, target, Time.deltaTime * scrollSpeed * 2f);

        display.text = prefix + current.ToString("#,0");
    }

    public void Add(float amount)
    {
        target += amount;
    }

    public void Clear()
    {
        target = 0;
    }

    public float GetValue()
    {
        return target;
    }
}
