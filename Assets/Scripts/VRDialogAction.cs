using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

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
    public Action<string> callback { get; set; }
    public Color background {
        get
        {
            return GetComponent<ProceduralImage>().color;
        }
        set
        {
            GetComponent<ProceduralImage>().color = value;
        }
    }
    public bool requireAction { get; set; }
}
