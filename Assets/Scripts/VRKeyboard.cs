using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Valve.VR.InteractionSystem;

public class VRKeyboard : MonoBehaviour
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
        if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.Return))
        {
            Completed();
        }
    }

    public void KeyPressed(Button button)
    {
        if (timeToAction() == -1) setTimeToAction();
        if (completed) return;
        ControllerButtonHints.HideAllButtonHints(MainController.Instance.leftPointer.GetComponent<Hand>());
        enteredText += button.GetComponentInChildren<Text>().text;
        if (enteredText == promptText) Completed();
    }

    public void Backspace()
    {
        addMistake();
        if (timeToAction() == -1) setTimeToAction();
        if (enteredText != "" && !completed)
        {
            enteredText = enteredText.Substring(0, enteredText.Length - 1);
            if (enteredText == promptText) Completed();
        }
    }

    public void Space()
    {
        if (timeToAction() == -1) setTimeToAction();
        if (completed) return;
        enteredText += " ";
        if (enteredText == promptText) Completed();
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
        completed = true;
        MainController.Instance.successSound.Stop();
        MainController.Instance.successSound.Play();
        Color green = Color.green;
        ColorUtility.TryParseHtmlString(greenHex, out green);
        entered.transform.parent.GetComponent<RawImage>().color = green;
        StartCoroutine(Complete());
    }

    private IEnumerator Complete()
    {
        MainController.Instance.timer.StopTimer();
        yield return new WaitForSeconds(1f);
        Reset();
        MainController.Instance.stringsCompleted++;
    }

    private float timeToAction()
    {
        if (MainController.Instance.stringsCompleted == 0) return MainController.Instance.metrics.e1TA1;
        if (MainController.Instance.stringsCompleted == 1) return MainController.Instance.metrics.e1TA2;
        if (MainController.Instance.stringsCompleted == 2) return MainController.Instance.metrics.e1TA3;
        if (MainController.Instance.stringsCompleted == 3) return MainController.Instance.metrics.e1TA4;
        if (MainController.Instance.stringsCompleted == 4) return MainController.Instance.metrics.e1TA5;
        return MainController.Instance.metrics.e1TA6;
    }

    private void setTimeToAction()
    {
        if (MainController.Instance.stringsCompleted == 0) MainController.Instance.metrics.e1TA1 = MainController.Instance.timer.time;
        else if (MainController.Instance.stringsCompleted == 1) MainController.Instance.metrics.e1TA2 = MainController.Instance.timer.time;
        else if (MainController.Instance.stringsCompleted == 2) MainController.Instance.metrics.e1TA3 = MainController.Instance.timer.time;
        else if (MainController.Instance.stringsCompleted == 3) MainController.Instance.metrics.e1TA4 = MainController.Instance.timer.time;
        else if (MainController.Instance.stringsCompleted == 4) MainController.Instance.metrics.e1TA5 = MainController.Instance.timer.time;
        else MainController.Instance.metrics.e1TA6 = MainController.Instance.timer.time;
        MainController.Instance.timer.time = 0f;
    }

    private void addMistake()
    {
        if (MainController.Instance.stringsCompleted == 0) MainController.Instance.metrics.e1M1++;
        else if (MainController.Instance.stringsCompleted == 1) MainController.Instance.metrics.e1M2++;
        else if (MainController.Instance.stringsCompleted == 2) MainController.Instance.metrics.e1M3++;
        else if (MainController.Instance.stringsCompleted == 3) MainController.Instance.metrics.e1M4++;
        else if (MainController.Instance.stringsCompleted == 4) MainController.Instance.metrics.e1M5++;
        else MainController.Instance.metrics.e1M6++;
    }
}
