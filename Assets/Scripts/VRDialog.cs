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
    public GameObject originalAnswer;
    public GameObject originalAction;

    public string text
    {
        get
        {
            return mainText.text;
        }
        set
        {
            mainText.text = value;
        }
    }
    public bool question
    {
        get
        {
            return answersPanel.activeSelf;
        }
        set
        {
            answersPanel.SetActive(value);
        }
    }

    private string selectedAnswer;

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActions(List<VRDialogActionValues> actions)
    {
        foreach (VRDialogActionValues action in actions)
        {
            GameObject newAction = Instantiate(originalAction);
            newAction.SetActive(true);
            newAction.transform.SetParent(originalAction.transform.parent);
            newAction.transform.localPosition = new Vector3(newAction.transform.localPosition.x, newAction.transform.localPosition.y, 0f);
            newAction.transform.localScale = Vector3.one;
            VRDialogAction dialogAction = newAction.GetComponent<VRDialogAction>();
            dialogAction.text = action.text;
            newAction.GetComponent<Button>().onClick.AddListener(() => {
                if (!action.requireAnswer || selectedAnswer != null)
                {
                    action.callback(selectedAnswer);
                }
            });
            dialogAction.callback = action.callback;
            newAction.GetComponent<Button>().enabled = !action.requireAnswer;
            dialogAction.background = action.background;
            dialogAction.requireAction = action.requireAnswer;
        }
    }

    public void SetAnswers(List<string> answers)
    {
        foreach (string answer in answers)
        {
            GameObject newAnswer = Instantiate(originalAnswer);
            newAnswer.SetActive(true);
            newAnswer.transform.SetParent(originalAnswer.transform.parent);
            newAnswer.transform.localPosition = new Vector3(newAnswer.transform.localPosition.x, newAnswer.transform.localPosition.y, 0f);
            newAnswer.transform.localScale = Vector3.one;
            newAnswer.GetComponentInChildren<Text>().text = answer;
            newAnswer.GetComponent<Button>().onClick.AddListener(() => {
                selectedAnswer = answer;
                
                // Enable all actions
                foreach (Transform action in originalAction.transform.parent)
                {
                    if (action != originalAction && action.GetComponent<VRDialogAction>().requireAction)
                    {
                        action.GetComponent<Button>().enabled = true;
                    }
                }

                // Unselect all other answers
                foreach (Transform answerTransform in originalAnswer.transform.parent)
                {
                    if (answerTransform != originalAnswer)
                    {
                        Color notSelected = Color.black;
                        ColorUtility.TryParseHtmlString("#373F51", out notSelected);
                        answerTransform.GetComponent<RawImage>().color = notSelected;
                    }
                }

                // Select the current answer
                Color selected = Color.blue;
                ColorUtility.TryParseHtmlString("#46ACC2", out selected);
                newAnswer.GetComponent<RawImage>().color = selected;
            });
        }
    }

    public void Reset()
    {
        selectedAnswer = null;

        foreach (Transform answer in originalAnswer.transform.parent)
        {
            if (answer.gameObject.activeSelf) // Don't delete the original
            {
                Destroy(answer.gameObject);
            }
        }

        foreach (Transform action in originalAction.transform.parent)
        {
            if (action.gameObject.activeSelf) // Don't delete the original
            {
                Destroy(action.gameObject);
            }
        }
    }
}
