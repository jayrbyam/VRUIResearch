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
    public bool keyboard = false;
    public bool numberPad = false;
    private int selected = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_Teleport[SteamVR_Input_Sources.Any].stateDown)
        {
            Vector2 normalizedPosition = SteamVR_Actions.default_DPad[SteamVR_Input_Sources.Any].axis.Rotate(45f);
            if (normalizedPosition.x <= 0 && normalizedPosition.y > 0 && selected > width - 1)  // If up pressed and not in top row
            {
                Select(selected - width);
            }
            if (normalizedPosition.x > 0 && normalizedPosition.y <= 0 && selected < height * width - width)  // If down pressed and not in bottom row
            {
                Select(selected + width);
            }
            if (normalizedPosition.x <= 0 && normalizedPosition.y <= 0 && selected % width != 0)  // If left pressed and not in first column
            {
                Select(selected - 1);
            }
            if (normalizedPosition.x > 0 && normalizedPosition.y > 0 && (selected + 1) % width != 0)  // If right pressed and not in last column
            {
                Select(selected + 1);
            }
        }
        if (SteamVR_Actions.default_InteractUI[SteamVR_Input_Sources.Any].stateDown)
        {
            transform.GetChild(selected).GetComponent<Button>().onClick.Invoke();
        }
    }

    private void Select(int idx)
    {
        if (idx < 0 || idx > width * height - 1) return;
        Color unselectedBackground = Color.gray;
        ColorUtility.TryParseHtmlString("#373F51", out unselectedBackground);
        transform.GetChild(selected).GetComponent<ProceduralImage>().color = unselectedBackground;
        Color unselectedText = Color.white;
        ColorUtility.TryParseHtmlString("#D8DBE2", out unselectedText);
        transform.GetChild(selected).GetComponentInChildren<Text>().color = unselectedText;

        selected = idx;
        if (keyboard && selected == 20) selected++;
        if (keyboard && selected == 29) selected--;
        if (numberPad && selected == 9) selected++;

        Color selectedBackground = Color.gray;
        ColorUtility.TryParseHtmlString("#46ACC2", out selectedBackground);
        transform.GetChild(selected).GetComponent<ProceduralImage>().color = selectedBackground;
        transform.GetChild(selected).GetComponentInChildren<Text>().color = Color.white;
    }
}

public static class Vector2Extension
{

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}