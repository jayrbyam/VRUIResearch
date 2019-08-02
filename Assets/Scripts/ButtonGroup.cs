using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class ButtonGroup : MonoBehaviour
{
    public int width;
    public int height;
    private int selected = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_DPadUp[SteamVR_Input_Sources.Any].state && selected > width - 1)  // If up pressed and not in top row
        {
            Select(selected - width);
        }
        if (SteamVR_Actions.default_DPadDown[SteamVR_Input_Sources.Any].state && selected < height * width - width)  // If down pressed and not in bottom row
        {
            Select(selected + width);
        }
        if (SteamVR_Actions.default_DPadLeft[SteamVR_Input_Sources.Any].state && selected % width != 0)  // If left pressed and not in first column
        {
            Select(selected - 1);
        }
        if (SteamVR_Actions.default_DPadRight[SteamVR_Input_Sources.Any].state && (selected + 1) % width != 0)  // If right pressed and not in last column
        {
            Select(selected + 1);
        }
        if (SteamVR_Actions.default_InteractUI[SteamVR_Input_Sources.Any].state)
        {
            transform.GetChild(selected).GetComponent<Button>().onClick.Invoke();
        }
    }

    private void Select(int idx)
    {
        Color unselectedBackground = Color.gray;
        ColorUtility.TryParseHtmlString("#373F51", out unselectedBackground);
        transform.GetChild(selected).GetComponent<ProceduralImage>().color = unselectedBackground;
        Color unselectedText = Color.white;
        ColorUtility.TryParseHtmlString("#D8DBE2", out unselectedText);
        transform.GetChild(selected).GetComponentInChildren<Text>().color = unselectedText;

        selected = idx;

        Color selectedBackground = Color.gray;
        ColorUtility.TryParseHtmlString("#46ACC2", out selectedBackground);
        transform.GetChild(selected).GetComponent<ProceduralImage>().color = selectedBackground;
        transform.GetChild(selected).GetComponentInChildren<Text>().color = Color.white;
    }
}
