using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Valve.VR.InteractionSystem;

public class VRNumberPad : MonoBehaviour
{

    public Text prompt;
    public Text entered;

    public string promptText
    {
        get
        {
            return prompt.text;
        }
        set
        {
            prompt.text = value;
        }
    }
    public string enteredText
    {
        get
        {
            return entered.text;
        }
        set
        {
            entered.text = value;
        }
    }

    private string redHex = "#E84118";
    private string greenHex = "#4CD137";
    private bool completed = false;

    // Update is called once per frame
    void Update()
    {

    }

    public void KeyPressed(Button button)
    {
        if (completed) return;
        enteredText += button.GetComponentInChildren<Text>().text;
        if (enteredText == promptText) Completed();
    }

    public void Backspace()
    {
        if (enteredText != "" && !completed)
        {
            MainController.Instance.metrics.st3M++;
            enteredText = enteredText.Substring(0, enteredText.Length - 1);
            if (enteredText == promptText) Completed();
        }
    }

    private void Reset()
    {
        enteredText = "";
        completed = false;
        Color red = Color.red;
        ColorUtility.TryParseHtmlString(redHex, out red);
        entered.transform.parent.GetComponent<RawImage>().color = red;
    }

    private void Completed()
    {
        MainController.Instance.successSound.Stop();
        MainController.Instance.successSound.Play();
        completed = true;
        Color green = Color.green;
        ColorUtility.TryParseHtmlString(greenHex, out green);
        entered.transform.parent.GetComponent<RawImage>().color = green;
        StartCoroutine(NextNumber());
    }

    private IEnumerator NextNumber()
    {
        MainController.Instance.timer.StopTimer();
        yield return new WaitForSeconds(1f);
        Reset();
        MainController.numbersCompleted++;
    }
}
