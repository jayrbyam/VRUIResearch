using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class VRDialog : MonoBehaviour
{

    public Text mainText;
    public GameObject answersPanel;
    public bool showAnswers = true;
    public GameObject originalAnswer;
    public GameObject originalAction;

    private List<string> answers;
    private List<VRDialogAction> actions;

    public VRDialog(string text, List<VRDialogAction> actionObjects, bool question = false, List<string> answerStrings = null)
    {
        mainText.text = text;

        foreach (VRDialogAction action in actionObjects)
        {
            GameObject newAction = Instantiate(originalAction);
            newAction.SetActive(true);
            newAction.transform.SetParent(originalAction.transform.parent);
            // Fill in text from button or whatever
            // Set callback of button thing to action.callback;
            newAction.GetComponent<ProceduralImage>().color = action.background;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if (answersPanel.activeSelf && !showAnswers) answersPanel.SetActive(false);
        if (!answersPanel.activeSelf && showAnswers) answersPanel.SetActive(true);
    }
}
