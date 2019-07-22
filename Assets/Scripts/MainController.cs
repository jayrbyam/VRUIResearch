using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

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
    public LaserPointer leftPointer;
    public LaserPointer rightPointer;
    public GameObject dialog;
    private VRDialog questionsDialog;
    private bool dominantRight = false;
    private delegate void AfterWaitDelegate();
    public GameObject numberPad;
    public GameObject threeTwoOne;

    // Indeces for "scenes" in the experience
    // 0 - Start screen
    // 1 - Pre-experience questions
    // 2 - Skill test
    // 3 - Instructions and Alerts
    // 4 - Menus
    // 5 - Movement
    private int sceneIdx = 2;

    // Scene 0
    public FadePulse startText;

    // Scene 1
    private int? questionIdx = 0;

    // Scene 2
    private int testIdx = 0;
    private bool testStarted = false;
    public List<Transform> skillTests;
    public Transform darts;
    public Text dartScoreText;
    public int dartScore
    {
        get
        {
            return int.Parse(dartScoreText.text.Split(' ')[1]);
        }
        set
        {
            dartScoreText.text = "Score: " + value.ToString();
        }
    }
    public static int dartsThrown = 0;
    private bool endTriggered = false;
    public GameObject leftRacket;
    public GameObject rightRacket;
    public GameObject tennisBall;
    public TennisShooter shooter;
    public Text tennisScoreText;
    public int tennisScore
    {
        get
        {
            return int.Parse(tennisScoreText.text.Split(' ')[1].Split('/')[0]);
        }
        set
        {
            tennisScoreText.text = "Score: " + value.ToString() + "/10";
        }
    }
    public Text numbersTimeText;
    public static int numbersCompleted = 0;
    private int numbersHandled = 0;
    public Timer numbersTimer;

    // Scene 3
    private bool secondScreen = false;

    // Scene 4
    public GameObject leftHandMenu;
    public GameObject rightHandMenu;

    // Scene 5


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
                if (SteamVR_Actions.default_InteractUI[SteamVR_Input_Sources.Any].state)
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
                            Color actionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Got it.",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 1;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 1:
                            questionIdx = null;
                            questionsDialog.text = "How would you categorize yourself as a VR user?";
                            questionsDialog.question = true;
                            actionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
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
                                    background = actionColor,
                                    requireAnswer = true
                                }
                            });
                            questionsDialog.SetAnswers(new List<string>() { "First timer", "Tried it before", "Skilled", "Seasoned veteran" });
                            break;
                        case 2:
                            questionIdx = null;
                            questionsDialog.text = "Select your dominant hand.";
                            questionsDialog.question = true;
                            actionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Oops",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 1;
                                    },
                                    background = actionColor,
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
                                    background = actionColor,
                                    requireAnswer = true
                                }
                            });
                            questionsDialog.SetAnswers(new List<string>() { "Left", "Right" });
                            break;
                        case 3:
                            questionIdx = null;
                            questionsDialog.text = "Nice work! You will now proceed to the pre-experiment skill test.  This skill test will help us get an idea of your base VR skill before starting the actual experiment.";
                            questionsDialog.question = false;
                            actionColor = Color.black;
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Back",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 2;
                                    },
                                    background = actionColor,
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
                                    background = actionColor,
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
                                questionsDialog.text = "<b>Skill Test #1</b>\nThe first test is a game of darts. Use the trigger to grab and throw each dart. Remember, you will be timed and the score will be recorded.";
                                questionsDialog.question = false;
                                Color actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
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
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                            }
                        } else
                        {
                            if (!skillTests[0].gameObject.activeSelf)
                            {
                                skillTests[0].gameObject.SetActive(true);
                                if (!dominantRight) darts.localPosition = new Vector3(-1.2f, 0f, 0f);
                                StartCoroutine(ThreeTwoOne());
                            }
                            
                            if (dartsThrown == 5 && !endTriggered)
                            {
                                endTriggered = true;
                                StartCoroutine(WaitThenExecute(3f, () =>
                                {
                                    skillTests[0].gameObject.SetActive(false);
                                    testIdx = 1;
                                    testStarted = false;
                                    endTriggered = false;
                                }));
                            }
                        }
                        break;
                    case 1:
                        if (!testStarted)
                        {
                            if (!dialog.activeSelf)
                            {
                                ToggleDialog(true);
                                questionsDialog.text = "<b>Skill Test #2</b>\nNext, you will be given a racket, and tennis balls will be shot toward you.  Hit a ball, get a point.";
                                questionsDialog.question = false;
                                Color actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
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
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (!skillTests[1].gameObject.activeSelf)
                            {
                                skillTests[1].gameObject.SetActive(true);
                                StartCoroutine(FillWithTennisBalls());

                                if (dominantRight && !rightRacket.activeSelf)
                                {
                                    rightRacket.SetActive(true);
                                    rightPointer.gameObject.GetComponent<Hand>().Hide();
                                }
                                if (!dominantRight && !leftRacket.activeSelf)
                                {
                                    leftRacket.SetActive(true);
                                    leftPointer.gameObject.GetComponent<Hand>().Hide();
                                }
                                StartCoroutine(ThreeTwoOne(() => { shooter.active = true; }));
                            }


                            if (shooter.ballsShot == 9 && !endTriggered)
                            {
                                endTriggered = true;
                                StartCoroutine(WaitThenExecute(3f, () =>
                                {
                                    if (dominantRight)
                                    {
                                        rightRacket.SetActive(false);
                                        rightPointer.gameObject.GetComponent<Hand>().Show();
                                    }
                                    else
                                    {
                                        leftRacket.SetActive(false);
                                        leftPointer.gameObject.GetComponent<Hand>().Show();
                                    }
                                    skillTests[1].gameObject.SetActive(false);
                                    testIdx = 2;
                                    testStarted = false;
                                    endTriggered = false;
                                }));
                            }
                        }
                        break;
                    case 2:
                        if (!testStarted)
                        {
                            if (!dialog.activeSelf)
                            {
                                ToggleDialog(true);
                                questionsDialog.text = "<b>Skill Test #3</b>\nLastly, a number pad will appear.  Use the laser pointer to enter the 5 displayed sequences on the number pad, as fast and as accuractely as you can.";
                                questionsDialog.question = false;
                                Color actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
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
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (!skillTests[2].gameObject.activeSelf)
                            {
                                skillTests[2].gameObject.SetActive(true);
                                ToggleNumberPad(true);
                                StartCoroutine(ThreeTwoOne(() => 
                                {
                                    numbersTimer = new Timer();
                                    numbersTimer.text = numbersTimeText;
                                    numbersTimer.StartTimer();
                                }));
                            }
                            if (numbersTimer != null) numbersTimer.Update();
                            if (numbersCompleted > 5)
                            {
                                SetSceneIdx(3);
                                testStarted = false;
                                ToggleNumberPad(false);
                            } else
                            {
                                if (numbersCompleted == numbersHandled)
                                {
                                    numbersTimer.StartTimer();
                                    numberPad.GetComponent<VRNumberPad>().promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    numbersHandled++;
                                }
                            }
                        }
                        break;
                }
                break;
            case 3: // Instructions & Alerts
                if (!testStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        questionsDialog.text = "Great work!  Your base VR skill score has been recorded.  Now, we will begin the first component of the UI experiment.";
                        questionsDialog.question = false;
                        Color actionColor = Color.black;
                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                        {
                            new VRDialogActionValues()
                            {
                                text = "Okay",
                                callback = answer => {
                                    questionsDialog.Reset();
                                    secondScreen = true;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    } else if (secondScreen)
                    {
                        secondScreen = false;
                        questionsDialog.text = "<b>Experiment #1</b>\nA series of instructions and alerts will appear in your view.  Follow the directions and answer each question to proceed through the test.";
                        questionsDialog.question = false;
                        Color actionColor = Color.black;
                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                        {
                            new VRDialogActionValues()
                            {
                                text = "Okay",
                                callback = answer => {
                                    questionsDialog.Reset();
                                    ToggleDialog(false);
                                    testStarted = true;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    }
                }
                else
                {
                    
                }
                break;
            case 4: // Menus
                if (!testStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        questionsDialog.text = "Now, a series of menus will appear in your view.  Follow the directions and perform the menu tasks to proceed through the test.";
                        questionsDialog.question = false;
                        Color actionColor = Color.black;
                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                        {
                            new VRDialogActionValues()
                            {
                                text = "Okay",
                                callback = answer => {
                                    questionsDialog.Reset();
                                    ToggleDialog(false);
                                    testStarted = true;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    }
                }
                else
                {
                    leftHandMenu.SetActive(true);
                }
                break;
            case 5: // Movement
                if (!testStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        questionsDialog.text = "You've done well!  This is the final test.  Follow the directions and complete the movements to finish this final test.";
                        questionsDialog.question = false;
                        Color actionColor = Color.black;
                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                        {
                            new VRDialogActionValues()
                            {
                                text = "Okay",
                                callback = answer => {
                                    questionsDialog.Reset();
                                    ToggleDialog(false);
                                    testStarted = true;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    }
                }
                else
                {

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
        rightPointer.active = show && dominantRight;
        leftPointer.active = show && !dominantRight;
    }

    private void ToggleNumberPad(bool show)
    {
        numberPad.SetActive(show);
        rightPointer.active = show && dominantRight;
        leftPointer.active = show && !dominantRight;
    }

    private IEnumerator WaitThenExecute(float seconds, AfterWaitDelegate callable)
    {
        yield return new WaitForSeconds(seconds);
        callable();
    }

    private IEnumerator FillWithTennisBalls()
    {
        for (int i = 0; i < 200; i++)
        {
            GameObject newBall = Instantiate(tennisBall);
            newBall.SetActive(true);
            newBall.transform.SetParent(tennisBall.transform.parent);
            newBall.transform.localScale = tennisBall.transform.localScale;
            float newX = tennisBall.transform.localPosition.x + UnityEngine.Random.Range(-0.1f, 0.1f);
            float newY = tennisBall.transform.localPosition.y + UnityEngine.Random.Range(-0.1f, 0.1f);
            float newZ = tennisBall.transform.localPosition.z + UnityEngine.Random.Range(-0.1f, 0.1f);
            newBall.transform.localPosition = new Vector3(newX, newY, newZ);
            yield return null;
        }
    }

    private IEnumerator ThreeTwoOne(AfterWaitDelegate callable = null)
    {
        threeTwoOne.SetActive(true);
        threeTwoOne.GetComponentInChildren<Text>().text = "3";
        yield return new WaitForSeconds(1f);
        threeTwoOne.GetComponentInChildren<Text>().text = "2";
        yield return new WaitForSeconds(1f);
        threeTwoOne.GetComponentInChildren<Text>().text = "1";
        yield return new WaitForSeconds(1f);
        threeTwoOne.SetActive(false);
        callable();
    }
}
