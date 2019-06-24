using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRDialogActionValues
{
    public string text { get; set; }
    public Action<string> callback { get; set; }
    public Color disabledBackground { get; set; }
    public Color enabledBackground { get; set; }
    public bool requireAnswer { get; set; }
}
