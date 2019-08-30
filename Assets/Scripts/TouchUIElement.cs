using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Valve.VR.InteractionSystem;

public class TouchUIElement : UIElement
{

    public bool onHoverChange = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnHandHoverBegin(Hand hand)
    {
        base.OnHandHoverBegin(hand);

        if (MainController.Instance.trackingHovers) MainController.Instance.hovers++;

        if (onHoverChange)
        {
            Color selectedBackground = Color.gray;
            ColorUtility.TryParseHtmlString("#46ACC2", out selectedBackground);
            GetComponent<ProceduralImage>().color = selectedBackground;
            GetComponentInChildren<Text>().color = Color.white;
        }
    }

    protected override void OnHandHoverEnd(Hand hand)
    {
        base.OnHandHoverEnd(hand);

        if (onHoverChange)
        {
            Color unselectedBackground = Color.gray;
            ColorUtility.TryParseHtmlString("#373F51", out unselectedBackground);
            GetComponent<ProceduralImage>().color = unselectedBackground;
            Color unselectedText = Color.white;
            ColorUtility.TryParseHtmlString("#D8DBE2", out unselectedText);
            GetComponentInChildren<Text>().color = unselectedText;
        }
    }

}
