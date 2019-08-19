using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FadePulse : MonoBehaviour
{
    public bool meshPro = true;
    public float delay = 0f;
    private bool hide;
    public bool Hide {
        set
        {
            hide = value;
            fadingOut = value;
        }
        get { return hide; }
    }
    private bool fadingOut = true;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0f) time += Time.deltaTime;

        if (delay == 0f || time > delay)
        {
            Color oldColor = meshPro ? GetComponent<TextMeshPro>().color : GetComponent<RawImage>().color;
            if (oldColor.a <= 0f)
            {
                fadingOut = false;
                if (hide)
                {
                    MainController.Instance.SetSceneIdx(1);
                    transform.parent.gameObject.SetActive(false);
                }
            }
            if (oldColor.a >= 1f) fadingOut = true;
            if (meshPro) GetComponent<TextMeshPro>().color = new Vector4(oldColor.r, oldColor.g, oldColor.b, oldColor.a + (fadingOut ? -0.005f : 0.005f));
            else GetComponent<RawImage>().color = new Vector4(oldColor.r, oldColor.g, oldColor.b, oldColor.a + (fadingOut ? -0.005f : 0.005f));
        }
    }
}
