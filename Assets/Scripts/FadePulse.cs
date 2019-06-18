using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadePulse : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color oldColor = GetComponent<TextMeshPro>().color;
        if (oldColor.a <= 0f)
        {
            fadingOut = false;
            if (hide)
            {
                transform.parent.gameObject.SetActive(false);
                MainController.Instance.SetSceneIdx(1);
            }
        }
        if (oldColor.a >= 1f) fadingOut = true;
        GetComponent<TextMeshPro>().color = new Vector4(oldColor.r, oldColor.g, oldColor.b, oldColor.a + (fadingOut ? -0.005f : 0.005f));
    }
}
