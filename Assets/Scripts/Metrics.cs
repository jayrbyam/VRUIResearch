using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Metrics
{
    public string peqVRE { get; set; }  // Pre-experiment questions: VR experience
    public string peqDH { get; set; }   // Pre-experiment questions: dominant hand
    public float st1T { get; set; }     // Skill Test #1: time
    public int st1S { get; set; }       // Skill Test #1: score
    public int st2S { get; set; }       // Skill Test #2: score
    public float st3T { get; set; }     // Skill Test #3: time
    public int st3H { get; set; }       // Skill Test #3: no selection hovers
    public int st3M { get; set; }       // Skill Test #3: mistakes
    //public float e1aT { get; set; }     // Experiment #1a: time
    //public int e1aCA { get; set; }      // Experiment #1a: correct answers (out of 8)
    //public float e1bT { get; set; }     // Experiment #1b: time
    //public int e1bCA { get; set; }      // Experiment #1b: correct answers (out of 8)
    //public int e1cCA { get; set; }      // Experiment #1c: corrent answers (out of 9)
    //public float e1cTV1 { get; set; }   // Experiment #1c: time at 1st alert in view
    //public float e1cTV2 { get; set; }   // Experiment #1c: time at 2nd alert in view
    //public float e1cTV3 { get; set; }   // Experiment #1c: time at 3rd alert in view
    //public float e1cTV4 { get; set; }   // Experiment #1c: time at 4th alert in view
    //public float e1cTV5 { get; set; }   // Experiment #1c: time at 5th alert in view
    //public float e1cTV6 { get; set; }   // Experiment #1c: time at 6th alert in view
    //public float e1cTV7 { get; set; }   // Experiment #1c: time at 7th alert in view
    //public float e1cTV8 { get; set; }   // Experiment #1c: time at 8th alert in view
    //public float e1cTV9 { get; set; }   // Experiment #1c: time at 9th alert in view
    //public float e1cTV10 { get; set; }  // Experiment #1c: time at 10th alert in view
    //public float e1cTA1 { get; set; }   // Experiment #1c: time at action on 1st alert
    //public float e1cTA2 { get; set; }   // Experiment #1c: time at action on 2nd alert
    //public float e1cTA3 { get; set; }   // Experiment #1c: time at action on 3rd alert
    //public float e1cTA4 { get; set; }   // Experiment #1c: time at action on 4th alert
    //public float e1cTA5 { get; set; }   // Experiment #1c: time at action on 5th alert
    //public float e1cTA6 { get; set; }   // Experiment #1c: time at action on 6th alert
    //public float e1cTA7 { get; set; }   // Experiment #1c: time at action on 7th alert
    //public float e1cTA8 { get; set; }   // Experiment #1c: time at action on 8th alert
    //public float e1cTA9 { get; set; }   // Experiment #1c: time at action on 9th alert
    //public float e1cTA10 { get; set; }  // Experiment #1c: time at action on 10th alert
    public float e1TA1 { get; set; }    // Experiment #1: time to first action on 1st sequence
    public float e1TA2 { get; set; }    // Experiment #1: time to first action on 2nd sequence
    public float e1TA3 { get; set; }    // Experiment #1: time to first action on 3rd sequence
    public float e1TA4 { get; set; }    // Experiment #1: time to first action on 4th sequence
    public float e1TA5 { get; set; }    // Experiment #1: time to first action on 5th sequence
    public float e1TA6 { get; set; }    // Experiment #1: time to first action on 6th sequence
    public float e1TC1 { get; set; }    // Experiment #1: time to completion of 1st sequence after first action
    public float e1TC2 { get; set; }    // Experiment #1: time to completion of 2nd sequence after first action
    public float e1TC3 { get; set; }    // Experiment #1: time to completion of 3rd sequence after first action
    public float e1TC4 { get; set; }    // Experiment #1: time to completion of 4th sequence after first action
    public float e1TC5 { get; set; }    // Experiment #1: time to completion of 5th sequence after first action
    public float e1TC6 { get; set; }    // Experiment #1: time to completion of 6th sequence after first action
    public int e1M1 { get; set; }       // Experiment #1: mistakes made on 1st sequence
    public int e1M2 { get; set; }       // Experiment #1: mistakes made on 2nd sequence
    public int e1M3 { get; set; }       // Experiment #1: mistakes made on 3rd sequence
    public int e1M4 { get; set; }       // Experiment #1: mistakes made on 4th sequence
    public int e1M5 { get; set; }       // Experiment #1: mistakes made on 5th sequence
    public int e1M6 { get; set; }       // Experiment #1: mistakes made on 6th sequence
    public int e1H1 { get; set; }       // Experiment #1: no selection hovers for 1st sequence
    public int e1H2 { get; set; }       // Experiment #1: no selection hovers for 2nd sequence
    public int e1H3 { get; set; }       // Experiment #1: no selection hovers for 3rd sequence
    public int e1H4 { get; set; }       // Experiment #1: no selection hovers for 4th sequence
    public int e1H5 { get; set; }       // Experiment #1: no selection hovers for 5th sequence
    public int e1H6 { get; set; }       // Experiment #1: no selection hovers for 6th sequence
    public int e1ML1 { get; set; }      // Experiment #1: unhappy/happy laser pointer manikin
    public int e1ML2 { get; set; }      // Experiment #1: calm/excited laser pointer manikin
    public int e1ML3 { get; set; }      // Experiment #1: in control/not in control laser pointer manikin
    public int e1MT1 { get; set; }      // Experiment #1: unhappy/happy touch manikin
    public int e1MT2 { get; set; }      // Experiment #1: calm/excited touch manikin
    public int e1MT3 { get; set; }      // Experiment #1: in control/not in control touch manikin
    public int e1MB1 { get; set; }      // Experiment #1: unhappy/happy thumbpad manikin
    public int e1MB2 { get; set; }      // Experiment #1: calm/excited thumbpad manikin
    public int e1MB3 { get; set; }      // Experiment #1: in control/not in control thumbpad manikin
    public int e1MH1 { get; set; }      // Experiment #1: unhappy/happy head mounted menu manikin
    public int e1MH2 { get; set; }      // Experiment #1: calm/excited head mounted menu manikin
    public int e1MH3 { get; set; }      // Experiment #1: in control/not in control head mounted menu manikin
    public int e1MC1 { get; set; }      // Experiment #1: unhappy/happy hand mounted menu manikin
    public int e1MC2 { get; set; }      // Experiment #1: calm/excited hand mounted menu manikin
    public int e1MC3 { get; set; }      // Experiment #1: in control/not in control hand mounted menu manikin
    public int e1MW1 { get; set; }      // Experiment #1: unhappy/happy world space menu manikin
    public int e1MW2 { get; set; }      // Experiment #1: calm/excited world space menu manikin
    public int e1MW3 { get; set; }      // Experiment #1: in control/not in control world space menu manikin
    public float e2TTA { get; set; }    // Experiment #2: time until first action with teleporting
    public float e2TTL1 { get; set; }   // Experiment #2: lap 1 time with teleporting
    public float e2TTL2 { get; set; }   // Experiment #2: lap 2 time with teleporting
    public float e2TTL3 { get; set; }   // Experiment #2: lap 3 time with teleporting
    public float e2JTA { get; set; }    // Experiment #2: time until first action with joystick
    public float e2JTL1 { get; set; }   // Experiment #2: lap 1 time with joystick
    public float e2JTL2 { get; set; }   // Experiment #2: lap 2 time with joystick
    public float e2JTL3 { get; set; }   // Experiment #2: lap 3 time with joystick
    public int e2MT1 { get; set; }      // Experiment #2: unhappy/happy teleport manikin
    public int e2MT2 { get; set; }      // Experiment #2: calm/excited teleport manikin
    public int e2MT3 { get; set; }      // Experiment #2: in control/not in control teleport manikin
    public int e2MT4 { get; set; }      // Experiment #2: comfortable/sick teleport manikin
    public int e2MJ1 { get; set; }      // Experiment #2: unhappy/happy joystick manikin
    public int e2MJ2 { get; set; }      // Experiment #2: calm/excited joystick manikin
    public int e2MJ3 { get; set; }      // Experiment #2: in control/not in control joystick manikin
    public int e2MJ4 { get; set; }      // Experiment #2: comfortable/sick joystick manikin


    public Metrics()
    {
        st3M = 0;
        //e1aCA = 0;
        //e1bCA = 0;
        //e1cCA = 0;
        //e1cTV1 = -1;
        //e1cTV2 = -1;
        //e1cTV3 = -1;
        //e1cTV4 = -1;
        //e1cTV5 = -1;
        //e1cTV6 = -1;
        //e1cTV7 = -1;
        //e1cTV8 = -1;
        //e1cTV9 = -1;
        //e1cTV10 = -1;
        e1TA1 = -1;
        e1TA2 = -1;
        e1TA3 = -1;
        e1TA4 = -1;
        e1TA5 = -1;
        e1TA6 = -1;
        e1M1 = 0;
        e1M2 = 0;
        e1M3 = 0;
        e1M4 = 0;
        e1M5 = 0;
        e1M6 = 0;
        e2TTA = -1;
        e2JTA = -1;
    }

    public void OutputMetrics()
    {
        string output = "";
        output += "Experience: " + peqVRE + "\n";
        output += "Dominant Hand: " + peqDH + "\n";
        output += "Skill Test #1 Time: " + st1T + "\n";
        output += "Skill Test #1 Score: " + st1S + "\n";
        output += "Skill Test #2 Score: " + st2S + "\n";
        output += "Skill Test #3 Time: " + st3T + "\n";
        output += "Skill Test #3 Hovers: " + st3H + "\n";
        output += "Skill Test #3 Mistakes: " + st3M + "\n";
        output += "Experiment #1 Time 'Til First Action 1st Sequence: " + e1TA1 + "\n";
        output += "Experiment #1 Time 'Til First Action 2nd Sequence: " + e1TA2 + "\n";
        output += "Experiment #1 Time 'Til First Action 3rd Sequence: " + e1TA3 + "\n";
        output += "Experiment #1 Time 'Til First Action 4th Sequence: " + e1TA4 + "\n";
        output += "Experiment #1 Time 'Til First Action 5th Sequence: " + e1TA5 + "\n";
        output += "Experiment #1 Time 'Til First Action 6th Sequence: " + e1TA6 + "\n";
        output += "Experiment #1 Time to Complete 1st Sequence: " + e1TC1 + "\n";
        output += "Experiment #1 Time to Complete 2nd Sequence: " + e1TC2 + "\n";
        output += "Experiment #1 Time to Complete 3rd Sequence: " + e1TC3 + "\n";
        output += "Experiment #1 Time to Complete 4th Sequence: " + e1TC4 + "\n";
        output += "Experiment #1 Time to Complete 5th Sequence: " + e1TC5 + "\n";
        output += "Experiment #1 Time to Complete 6th Sequence: " + e1TC6 + "\n";
        output += "Experiment #1 Mistakes on 1st Sequence: " + e1M1 + "\n";
        output += "Experiment #1 Mistakes on 2nd Sequence: " + e1M2 + "\n";
        output += "Experiment #1 Mistakes on 3rd Sequence: " + e1M3 + "\n";
        output += "Experiment #1 Mistakes on 4th Sequence: " + e1M4 + "\n";
        output += "Experiment #1 Mistakes on 5th Sequence: " + e1M5 + "\n";
        output += "Experiment #1 Mistakes on 6th Sequence: " + e1M6 + "\n";
        output += "Experiment #1 Hovers on 1st Sequence: " + e1H1 + "\n";
        output += "Experiment #1 Hovers on 2nd Sequence: " + e1H2 + "\n";
        output += "Experiment #1 Hovers on 3rd Sequence: " + e1H3 + "\n";
        output += "Experiment #1 Hovers on 4th Sequence: " + e1H4 + "\n";
        output += "Experiment #1 Hovers on 5th Sequence: " + e1H5 + "\n";
        output += "Experiment #1 Hovers on 6th Sequence: " + e1H6 + "\n";
        output += "Experiment #1 Laser Pointer Happy Manikin: " + e1ML1 + "\n";
        output += "Experiment #1 Laser Pointer Excited Manikin: " + e1ML2 + "\n";
        output += "Experiment #1 Laser Pointer In Control Manikin: " + e1ML3 + "\n";
        output += "Experiment #1 Touch Happy Manikin: " + e1MT1 + "\n";
        output += "Experiment #1 Touch Excited Manikin: " + e1MT2 + "\n";
        output += "Experiment #1 Touch In Control Manikin:" + e1MT3 + "\n";
        output += "Experiment #1 Thumbpad Happy Manikin: " + e1MB1 + "\n";
        output += "Experiment #1 Thumbpad Excited Manikin: " + e1MB2 + "\n";
        output += "Experiment #1 Thumbpad In Control Manikin:" + e1MB3 + "\n";
        output += "Experiment #1 Head Mounted Menu Happy Manikin: " + e1MH1 + "\n";
        output += "Experiment #1 Head Mounted Menu Excited Manikin: " + e1MH2 + "\n";
        output += "Experiment #1 Head Mounted Menu In Control Manikin:" + e1MH3 + "\n";
        output += "Experiment #1 Hand Mounted Menu Happy Manikin: " + e1MC1 + "\n";
        output += "Experiment #1 Hand Mounted Menu Excited Manikin: " + e1MC2 + "\n";
        output += "Experiment #1 Hand Mounted Menu In Control Manikin:" + e1MC3 + "\n";
        output += "Experiment #1 World Space Menu Happy Manikin: " + e1MW1 + "\n";
        output += "Experiment #1 World Space Menu Excited Manikin: " + e1MW2 + "\n";
        output += "Experiment #1 World Space Menu In Control Manikin:" + e1MW3 + "\n";
        output += "Experiment #2 Time to 1st Teleport Action: " + e2TTA + "\n";
        output += "Experiment #2 Time to Complete 1st Teleport Lap: " + e2TTL1 + "\n";
        output += "Experiment #2 Time to Complete 2nd Teleport Lap: " + e2TTL2 + "\n";
        output += "Experiment #2 Time to Complete 3rd Teleport Lap: " + e2TTL3 + "\n";
        output += "Experiment #2 Time to 1st Joystick Action: " + e2JTA + "\n";
        output += "Experiment #2 Time to Complete 1st Joystick Lap: " + e2JTL1 + "\n";
        output += "Experiment #2 Time to Complete 2nd Joystick Lap: " + e2JTL2 + "\n";
        output += "Experiment #2 Time to Complete 3rd Joystick Lap: " + e2JTL3 + "\n";
        output += "Experiment #2 Teleport Happy Manikin: " + e2MT1 + "\n";
        output += "Experiment #2 Teleport Excited Manikin: " + e2MT2 + "\n";
        output += "Experiment #2 Teleport In Control Manikin: " + e2MT3 + "\n";
        output += "Experiment #2 Teleport Sick Manikin: " + e2MT4 + "\n";
        output += "Experiment #2 Joystick Happy Manikin: " + e2MJ1 + "\n";
        output += "Experiment #2 Joystick Excited Manikin: " + e2MJ2 + "\n";
        output += "Experiment #2 Joystick In Control Manikin: " + e2MJ3 + "\n";
        output += "Experiment #2 Joystick Sick Manikin: " + e2MJ4 + "\n";
        Debug.Log(output);

        string line = "";
        line += peqVRE + ",";
        line += peqDH + ",";
        line += st1T + ",";
        line += st1S + ",";
        line += st2S + ",";
        line += st3T + ",";
        line += st3H + ",";
        line += st3M + ",";
        line += e1TA1 + ",";
        line += e1TA2 + ",";
        line += e1TA3 + ",";
        line += e1TA4 + ",";
        line += e1TA5 + ",";
        line += e1TA6 + ",";
        line += e1TC1 + ",";
        line += e1TC2 + ",";
        line += e1TC3 + ",";
        line += e1TC4 + ",";
        line += e1TC5 + ",";
        line += e1TC6 + ",";
        line += e1M1 + ",";
        line += e1M2 + ",";
        line += e1M3 + ",";
        line += e1M4 + ",";
        line += e1M5 + ",";
        line += e1M6 + ",";
        line += e1H1 + ",";
        line += e1H2 + ",";
        line += e1H3 + ",";
        line += e1H4 + ",";
        line += e1H5 + ",";
        line += e1H6 + ",";
        line += e1ML1 + ",";
        line += e1ML2 + ",";
        line += e1ML3 + ",";
        line += e1MT1 + ",";
        line += e1MT2 + ",";
        line += e1MT3 + ",";
        line += e1MB1 + ",";
        line += e1MB2 + ",";
        line += e1MB3 + ",";
        line += e1MH1 + ",";
        line += e1MH2 + ",";
        line += e1MH3 + ",";
        line += e1MC1 + ",";
        line += e1MC2 + ",";
        line += e1MC3 + ",";
        line += e1MW1 + ",";
        line += e1MW2 + ",";
        line += e1MW3 + ",";
        line += e2TTA + ",";
        line += e2TTL1 + ",";
        line += e2TTL2 + ",";
        line += e2TTL3 + ",";
        line += e2JTA + ",";
        line += e2JTL1 + ",";
        line += e2JTL2 + ",";
        line += e2JTL3 + ",";
        line += e2MT1 + ",";
        line += e2MT2 + ",";
        line += e2MT3 + ",";
        line += e2MT4 + ",";
        line += e2MJ1 + ",";
        line += e2MJ2 + ",";
        line += e2MJ3 + ",";
        line += e2MJ4 + "\n";

        var csv = new StringBuilder();
        csv.AppendLine(line);
        File.AppendAllText(@"C:\Users\jayrb\VRUIResearch\data.csv", csv.ToString());
        Debug.Log("Metrics saved to file.");
    }
}