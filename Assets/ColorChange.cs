using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{

    [Header("Visuals")]
    public Color[] colors;
    public int currentIndex = 0;
    private int nextIndex;

    public float changeColorTime = 10.0f;
    private float lastChange = 0.0f;
    private float timer = 0.0f;
    public Color tempColor;

    void Start()
    {
        nextIndex = (currentIndex + 1) % colors.Length;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > changeColorTime)
        {
            currentIndex = (currentIndex + 1) % colors.Length;
            nextIndex = (currentIndex + 1) % colors.Length;
            timer = 0.0f;
        }

        tempColor = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColorTime);
    }
}
