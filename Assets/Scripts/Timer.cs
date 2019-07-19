using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    public float time = 0f;
    private bool timing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timing) time += Time.deltaTime;
    }

    public void StartTimer()
    {
        timing = true;
    }

    public void StopTimer()
    {
        timing = false;
    }
}
