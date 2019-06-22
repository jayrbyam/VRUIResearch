using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRDialogAction : MonoBehaviour
{
    public string text {
        get
        {
            return GetComponentInChildren<Text>().text;
        }
        set
        {
            GetComponentInChildren<Text>().text = value;
        }
    }
    public Action callback { get; set; }
    public Color disabledBackground { get; set; }
    public Color enabledBackground { get; set; }
}
