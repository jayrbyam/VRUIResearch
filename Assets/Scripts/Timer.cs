using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer
{
    public Text text;
    public float time = 0f;
    private bool timing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (timing)
        {
            time += Time.deltaTime;
            if (text != null)
            {
                string minutes = "00";
                int m = 0;
                if (time > 60f)
                {
                    m = (int)Mathf.Floor(time / 60f);
                    minutes = (m > 9 ? "" : "0") + m.ToString();
                }
                string seconds = "00";
                if (time > 1f)
                {
                    int s = (int)Mathf.Floor(time - m * 60f);
                    seconds = (s > 9 ? "" : "0") + s.ToString();
                }
                int n = (int)(60f * (time - Mathf.Floor(time)));
                string milliseconds = (n > 9 ? "" : "0") + n.ToString();
                text.text = minutes + ":" + seconds + ":" + milliseconds;
            }
        }
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
