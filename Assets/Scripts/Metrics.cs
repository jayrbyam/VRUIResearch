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
    public int e1cCA { get; set; }      // Experiment #1c: corrent answers (out of 9)
    public float e1cTV1 { get; set; }   // Experiment #1c: time at 1st alert in view
    public float e1cTV2 { get; set; }   // Experiment #1c: time at 2nd alert in view
    public float e1cTV3 { get; set; }   // Experiment #1c: time at 3rd alert in view
    public float e1cTV4 { get; set; }   // Experiment #1c: time at 4th alert in view
    public float e1cTV5 { get; set; }   // Experiment #1c: time at 5th alert in view
    public float e1cTV6 { get; set; }   // Experiment #1c: time at 6th alert in view
    public float e1cTV7 { get; set; }   // Experiment #1c: time at 7th alert in view
    public float e1cTV8 { get; set; }   // Experiment #1c: time at 8th alert in view
    public float e1cTV9 { get; set; }   // Experiment #1c: time at 9th alert in view
    public float e1cTV10 { get; set; }  // Experiment #1c: time at 10th alert in view
    public float e1cTA1 { get; set; }   // Experiment #1c: time at action on 1st alert
    public float e1cTA2 { get; set; }   // Experiment #1c: time at action on 2nd alert
    public float e1cTA3 { get; set; }   // Experiment #1c: time at action on 3rd alert
    public float e1cTA4 { get; set; }   // Experiment #1c: time at action on 4th alert
    public float e1cTA5 { get; set; }   // Experiment #1c: time at action on 5th alert
    public float e1cTA6 { get; set; }   // Experiment #1c: time at action on 6th alert
    public float e1cTA7 { get; set; }   // Experiment #1c: time at action on 7th alert
    public float e1cTA8 { get; set; }   // Experiment #1c: time at action on 8th alert
    public float e1cTA9 { get; set; }   // Experiment #1c: time at action on 9th alert
    public float e1cTA10 { get; set; }  // Experiment #1c: time at action on 10th alert
    public float e2TA1 { get; set; }    // Experiment #2: time at action on 1st sequence
    public float e2TA2 { get; set; }    // Experiment #2: time at action on 2nd sequence
    public float e2TA3 { get; set; }    // Experiment #2: time at action on 3rd sequence
    public float e2TA4 { get; set; }    // Experiment #2: time at action on 4th sequence
    public float e2TA5 { get; set; }    // Experiment #2: time at action on 5th sequence
    public float e2TA6 { get; set; }    // Experiment #2: time at action on 6th sequence
    public float e2TC1 { get; set; }    // Experiment #2: time at completion of 1st sequence
    public float e2TC2 { get; set; }    // Experiment #2: time at completion of 2nd sequence
    public float e2TC3 { get; set; }    // Experiment #2: time at completion of 3rd sequence
    public float e2TC4 { get; set; }    // Experiment #2: time at completion of 4th sequence
    public float e2TC5 { get; set; }    // Experiment #2: time at completion of 5th sequence
    public float e2TC6 { get; set; }    // Experiment #2: time at completion of 6th sequence
    public int e2M1 { get; set; }       // Experiment #2: mistakes made on 1st sequence
    public int e2M2 { get; set; }       // Experiment #2: mistakes made on 2nd sequence
    public int e2M3 { get; set; }       // Experiment #2: mistakes made on 3rd sequence
    public int e2M4 { get; set; }       // Experiment #2: mistakes made on 4th sequence
    public int e2M5 { get; set; }       // Experiment #2: mistakes made on 5th sequence
    public int e2M6 { get; set; }       // Experiment #2: mistakes made on 6th sequence

    public Metrics()
    {
        e1aCA = 0;
        e1bCA = 0;
        e1cCA = 0;
        e1cTV1 = -1;
        e1cTV2 = -1;
        e1cTV3 = -1;
        e1cTV4 = -1;
        e1cTV5 = -1;
        e1cTV6 = -1;
        e1cTV7 = -1;
        e1cTV8 = -1;
        e1cTV9 = -1;
        e1cTV10 = -1;
        e2TA1 = -1;
        e2TA2 = -1;
        e2TA3 = -1;
        e2TA4 = -1;
        e2TA5 = -1;
        e2TA6 = -1;
        e2M1 = 0;
        e2M2 = 0;
        e2M3 = 0;
        e2M4 = 0;
        e2M5 = 0;
        e2M6 = 0;
    }
}
