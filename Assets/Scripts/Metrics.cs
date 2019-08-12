using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrics
{
    public string peqVRE { get; set; }  // Pre-experiment questions: VR experience
    public string peqDH { get; set; }   // Pre-experiment questions: dominant hand
    public float st1T { get; set; }     // Skill Test #1: time
    public int st1S { get; set; }       // Skill Test #1: score
    public int st2S { get; set; }       // Skill Test #2: score
    public float st3T { get; set; }     // Skill Test #3: time
    public float e1aT { get; set; }     // Experiment #1a: time
    public int e1aCA { get; set; }      // Experiment #1a: correct answers (out of 8)
    public float e1bT { get; set; }     // Experiment #1b: time
    public int e1bCA { get; set; }      // Experiment #1b: correct answers (out of 8)
    public float e1cT { get; set; }     // Experiment #1c: time
    public int e1cCA { get; set; }      // Experiment #1c: corrent answers (out of 9)
    // Add time it took for alert to enter camera view

    public Metrics()
    {
        e1aCA = 0;
        e1bCA = 0;
        e1cCA = 0;
    }
}
