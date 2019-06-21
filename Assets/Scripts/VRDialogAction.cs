using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRDialogAction
{
    public string text { get; set; }
    public Action callback { get; set; }
    public Color background { get; set; }
}
