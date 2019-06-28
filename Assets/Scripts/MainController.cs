using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MainController : MonoBehaviour
{
    // Singleton pattern
    private static MainController _instance;
    public static MainController Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<GameObject> scenes;
    public VRTK_ControllerEvents leftControllerEvents;
    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_Pointer leftPointer;
    public VRTK_Pointer rightPointer;
    public GameObject dialog;
    private VRDialog questionsDialog;
    private bool dominantRight = true;

    // Indeces for "scenes" in the experience
    // 0 - Start screen
    // 1 - Pre-experience questions
    // 2 - Skill test
    private int sceneIdx = 0;

    // Scene 0
    public FadePulse startText;

    // Scene 1
    private int? questionIdx = 0;

    // Scene 2
    private int testIdx = 0;
    private bool testStarted = false;
    public List<Transform> skillTests;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject child in scenes)
        {
            child.SetActive(false);
        }
        scenes[sceneIdx].SetActive(true);
        questionsDialog = dialog.GetComponent<VRDialog>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (sceneIdx)
        {
            case 0: // Starting screen
                if (leftControllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick) || rightControllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick))
                {
                    startText.Hide = true;
                }
                break;
            case 1: // Pre-experiment questions
                if (!dialog.activeSelf)
                {

                    ToggleDialog(true);
                }

                if (questionIdx != null)
                {
                    switch (questionIdx)
                    {
                        case 0:
                            questionIdx = null;
                            questionsDialog.text = "Hi! Thanks for participating.  We're going to start off with just a few questions.";
                            questionsDialog.question = false;
                            Color disabledActionColor = Color.grey;
                            ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                            Color enabledActionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Got it.",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 1;
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 1:
                            questionIdx = null;
                            questionsDialog.text = "How would you categorize yourself as a VR user?";
                            questionsDialog.question = true;
                            disabledActionColor = Color.grey;
                            ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                            enabledActionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Next",
                                    callback = answer => {
                                        Debug.Log("'" + answer + "' selected.");
                                        questionsDialog.Reset();
                                        questionIdx = 2;
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = true
                                }
                            });
                            questionsDialog.SetAnswers(new List<string>() { "First timer", "Tried it before", "Skilled", "Seasoned veteran" });
                            break;
                        case 2:
                            questionIdx = null;
                            questionsDialog.text = "Select your dominant hand.";
                            questionsDialog.question = true;
                            disabledActionColor = Color.grey;
                            ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                            enabledActionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Oops",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 1;
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = false
                                },
                                new VRDialogActionValues()
                                {
                                    text = "Next",
                                    callback = answer => {
                                        Debug.Log("'" + answer + "' selected.");
                                        dominantRight = answer == "Right";
                                        ToggleDialog(false); // Force reload of dominant hand pointer
                                        questionsDialog.Reset();
                                        questionIdx = 3;
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = true
                                }
                            });
                            questionsDialog.SetAnswers(new List<string>() { "Left", "Right" });
                            break;
                        case 3:
                            questionIdx = null;
                            questionsDialog.text = "Nice work! You will now proceed to the pre-experiment skill test.";
                            questionsDialog.question = false;
                            disabledActionColor = Color.grey;
                            ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                            enabledActionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Back",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 2;
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = false
                                },
                                new VRDialogActionValues()
                                {
                                    text = "Alright!",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        ToggleDialog(false);
                                        SetSceneIdx(2);
                                    },
                                    disabledBackground = disabledActionColor,
                                    enabledBackground = enabledActionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                    }
                }
                break;
            case 2: // Skill test
                switch (testIdx)
                {
                    case 0:
                        if (!testStarted)
                        {
                            if (!dialog.activeSelf)
                            {
                                ToggleDialog(true);
                                questionsDialog.text = "Our first test is a simple game of 'Virtually Throw the Virtual Darts at the Virtual Dartboard'. Use the trigger to grab and throw each ball. Remember, you will be timed and the score will be recorded.";
                                questionsDialog.question = false;
                                Color disabledActionColor = Color.grey;
                                ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                                Color enabledActionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                                questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Ready!",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            ToggleDialog(false);
                                            testStarted = true;
                                        },
                                        disabledBackground = disabledActionColor,
                                        enabledBackground = enabledActionColor,
                                        requireAnswer = false
                                    }
                                });
                            }
                        } else
                        {
                            if (!skillTests[testIdx].gameObject.activeSelf) skillTests[testIdx].gameObject.SetActive(true);
                            

                        }
                        break;
                }
                break;
        }
        
    }

    public void SetSceneIdx (int idx)
    {
        scenes[sceneIdx].SetActive(false);
        sceneIdx = idx;
        scenes[sceneIdx].SetActive(true);
    }

    private void ToggleDialog(bool show)
    {
        dialog.SetActive(show);
        rightPointer.Toggle(show && dominantRight);
        leftPointer.Toggle(show && !dominantRight);
    }
}
