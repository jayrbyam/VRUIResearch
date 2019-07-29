using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class Indicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(bool active)
    {
        GetComponent<ProceduralImage>().color = new Vector4(1f, 1f, 1f, active ? 1f : 0.4f);
        GetComponentInChildren<RawImage>().color = new Vector4(1f, 1f, 1f, active ? 1f : 0.2f);
    }
}
