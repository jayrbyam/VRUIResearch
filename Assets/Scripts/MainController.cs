using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.SceneManagement;
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

    public Metrics metrics;
    public List<GameObject> scenes;
    public LaserPointer leftPointer;
    public LaserPointer rightPointer;
    public GameObject dialog;
    private VRDialog questionsDialog;
    private bool dominantRight = true;
    private delegate void AfterWaitDelegate();
    public GameObject numberPad;
    public GameObject progressBar;
    public GameObject threeTwoOne;
    public AudioSource threeTwoOneSound;
    public AudioSource successSound;
    public AudioSource ambience;
    public AudioSource music;
    public Timer timer;
    private List<string> letters = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    // Indeces for "scenes" in the experience
    // 0 - Start screen
    // 1 - Pre-experience questions
    // 2 - Skill test
    // 3 - Instructions and Alerts
    // 4 - Menus
    // 5 - Movement
    // 6 - Thank You
    private int sceneIdx = 0;

    // Scene 0
    public FadePulse startText;

    // Scene 1
    private int? questionIdx = 0;
    public bool waitingForRightRotate = false;
    public bool waitingForLeftRotate = false;

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
    public int dartsThrown = 0;
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
            tennisScoreText.text = "Score: " + value.ToString();
        }
    }
    public Text numbersTimeText;
    public static int numbersCompleted = 0;
    private int numbersHandled = 0;
    private bool testCompleted = false;

    // Scene 3
    private bool secondScreen = false;
    private int experimentIdx = 0;
    private bool testsStarted = false;
    public List<GameObject> experiments;
    private int audioSourcesHandled = 0;
    public int audioSourcesSelected = 0;
    public TwoAudio twoAudio;
    public int indicatorsHandled = 0;
    public int indicatorsSelected = 0;
    public TwoIndicators twoIndicators;
    public AlertsController alerts;
    public VisualCues visualCues;

    // Scene 4
    private int techniqueIdx = -1;
    public GameObject technique2Button;
    public ButtonGroup technique3Buttons;
    public int stringsHandled = 0;
    public int stringsCompleted = 0;
    public Text stringsTimeText;
    public VRKeyboard keyboard;
    public GameObject menusPrompt;
    public GameObject touchNumberPad;
    public GameObject headKeyboard;
    public GameObject leftHandNumberPad;
    public GameObject rightHandNumberPad;
    public GameObject buttonKeyboard;
    public GameObject leftHandKeyboard;
    public GameObject rightHandKeyboard;
    public GameObject headNumberPad;

    // Scene 5
    public Teleport teleport;
    public Joystick joystick;
    public GameObject lapStuff;
    public Text techniqueText;
    public TrackPlayer tracker;
    public GameObject secondLapStuff;
    public GameObject thirdLapStuff;
    public GameObject joystickStuff;
    public GameObject joystickSecondLapStuff;
    public GameObject joystickThirdLapStuff;
    private System.Random rand;
    private bool oneDone = false;

    // Scene 6
    private float reloadTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        metrics = new Metrics();

        foreach (GameObject child in scenes)
        {
            child.SetActive(false);
        }
        scenes[sceneIdx].SetActive(true);
        questionsDialog = dialog.GetComponent<VRDialog>();
        StartCoroutine(FadeAmbience(true));
        timer = new Timer();

        teleport.CancelTeleportHint();
        teleport.enabled = false;
        joystick.enabled = false;
        rand = new System.Random();
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
                    threeTwoOneSound.Play();
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
                                        metrics.peqVRE = answer;
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
                                        metrics.peqDH = answer;
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
                            questionsDialog.text = "Nice work! Now, notice the dots at the bottom of this dialog.  These will show you how far along you are in the experiment.";
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
                                    text = "Okay",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 4;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 4:
                            questionIdx = null;
                            questionsDialog.text = "Before we get to the fun stuff, you need to know how to rotate your view.";
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
                                        questionIdx = 3;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                },
                                new VRDialogActionValues()
                                {
                                    text = "Okay",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 5;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 5:
                            questionIdx = null;
                            questionsDialog.text = "Press on the left half of your right-hand thumbpad to rotate your view to the left.";
                            questionsDialog.question = false;
                            actionColor = Color.black;
                            waitingForLeftRotate = true;
                            ControllerButtonHints.ShowButtonHint(rightPointer.GetComponent<Hand>(), SteamVR_Actions.default_DPadLeft);
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Back",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 4;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 6:
                            questionIdx = null;
                            questionsDialog.text = "Press on the right half of your right-hand thumbpad to rotate your view to the right.";
                            questionsDialog.question = false;
                            actionColor = Color.black;
                            waitingForRightRotate = true;
                            ControllerButtonHints.ShowButtonHint(rightPointer.GetComponent<Hand>(), SteamVR_Actions.default_DPadLeft);
                            ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                            questionsDialog.SetActions(new List<VRDialogActionValues>()
                            {
                                new VRDialogActionValues()
                                {
                                    text = "Back",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        questionIdx = 5;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                            break;
                        case 7:
                            questionIdx = null;
                            questionsDialog.text = "Great! You will now proceed to the pre-experiment skill test.  This skill test will help us get an idea of your base VR skill before starting the actual experiment.";
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
                                        questionIdx = 6;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                },
                                new VRDialogActionValues()
                                {
                                    text = "Alright!",
                                    callback = answer => {
                                        questionsDialog.Reset();
                                        UpdateProgress(1);
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
                                questionsDialog.text = "<b>Skill Test #1</b>\nThe first test is a game of darts. Use the trigger to grab and throw each dart.\nHint: \"close enough\" points may be given.";
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
                                            StartCoroutine(FadeAmbience(false));
                                            StartCoroutine(FadeMusic(true));
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
                                StartCoroutine(ThreeTwoOne(() =>
                                {
                                    timer.StartTimer();
                                }));
                            }
                            timer.Update();
                            if (dartsThrown == 5 && !endTriggered)
                            {
                                timer.StopTimer();
                                metrics.st1T = timer.time;
                                timer.time = 0f;
                                metrics.st1S = dartScore;
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
                                questionsDialog.text = "<b>Skill Test #2</b>\nNext, you will be given a hoop in your hand, and balls will be shot at you.  Get a ball through the hoop, and you will get a point.\nHint: \"close enough\" points may be given.";
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
                                metrics.st2S = tennisScore;
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
                                questionsDialog.text = "<b>Skill Test #3</b>\nLastly, a number pad will appear.  Use the laser pointer to enter the 3 displayed sequences on the number pad, as fast and as accuractely as you can.";
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
                                    timer.text = numbersTimeText;
                                    timer.StartTimer();
                                    numberPad.GetComponent<VRNumberPad>().promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    numbersHandled++;
                                }));
                            }
                            timer.Update();
                            if (numbersCompleted == 3)
                            {
                                metrics.st3T = timer.time;
                                timer.time = 0f;
                                SetSceneIdx(3);
                                testStarted = false;
                                ToggleNumberPad(false);
                                StartCoroutine(FadeMusic(false));
                            } else
                            {
                                if (numbersCompleted == numbersHandled && numbersHandled != 0)
                                {
                                    timer.StartTimer();
                                    numberPad.GetComponent<VRNumberPad>().promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    numbersHandled++;
                                }
                            }
                        }
                        break;
                }
                break;
            case 3: // Instructions & Alerts
                if (!testsStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        StartCoroutine(FadeAmbience(true));
                        questionsDialog.text = "Great work!  Your base VR skill score has been recorded.  Now, we will begin the first component of the UI experiment.  Ready?";
                        questionsDialog.question = false;
                        Color actionColor = Color.black;
                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                        {
                            new VRDialogActionValues()
                            {
                                text = "Yup.",
                                callback = answer => {
                                    questionsDialog.Reset();
                                    UpdateProgress(2);
                                    ToggleDialog(false);
                                    SetSceneIdx(4);
                                    //secondScreen = true; // Skipping the instructions portion
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    } else if (secondScreen)
                    {
                        secondScreen = false;
                        questionsDialog.text = "<b>Experiment #1</b>\nA series of instructions and alerts will appear in your view.  Follow the directions and complete the actions to proceed through the test.";
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
                                    testsStarted = true;
                                    secondScreen = false;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    }
                }
                else
                {
                    if (!testCompleted)
                    {
                        switch (experimentIdx)
                        {
                            case 0:
                                if (!testStarted)
                                {
                                    if (!dialog.activeSelf)
                                    {
                                        ToggleDialog(true);
                                        questionsDialog.text = "<b>Experiment #1a</b>\nTwo audio sources will appear in front of you.  Use the laser pointer to select the audio source that is currently emitting audio.  Please make selections as quickly as possible.";
                                        questionsDialog.question = false;
                                        Color actionColor = Color.black;
                                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                                        {
                                            new VRDialogActionValues()
                                            {
                                                text = "Start",
                                                callback = answer => {
                                                    questionsDialog.Reset();
                                                    ToggleDialog(false);
                                                    StartCoroutine(FadeAmbience(false));
                                                    TogglePointer(true);
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
                                    if (!experiments[0].gameObject.activeSelf)
                                    {
                                        experiments[0].SetActive(true);
                                        StartCoroutine(ThreeTwoOne(() =>
                                        {
                                            twoAudio.EmitRandom();
                                            audioSourcesHandled++;
                                            timer.StartTimer();
                                        }));
                                    }
                                    timer.Update();
                                    if (audioSourcesSelected > 8)
                                    {
                                        metrics.e1aT = timer.time;
                                        timer.time = 0f;
                                        experiments[0].SetActive(false);
                                        experimentIdx = 1;
                                        testStarted = false;
                                    }
                                    else
                                    {
                                        if (audioSourcesSelected == audioSourcesHandled && audioSourcesHandled != 0)
                                        {
                                            twoAudio.EmitRandom();
                                            audioSourcesHandled++;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (!testStarted)
                                {
                                    if (!dialog.activeSelf)
                                    {
                                        ToggleDialog(true);
                                        StartCoroutine(FadeAmbience(true));
                                        questionsDialog.text = "<b>Experiment #1b</b>\nTwo visual indicators will appear in front of you.  Use the laser pointer to select the indicator that is currently activated.  Please make selections as quickly as possible.";
                                        questionsDialog.question = false;
                                        Color actionColor = Color.black;
                                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                                        {
                                            new VRDialogActionValues()
                                            {
                                                text = "Start",
                                                callback = answer => {
                                                    questionsDialog.Reset();
                                                    ToggleDialog(false);
                                                    TogglePointer(true);
                                                    StartCoroutine(FadeAmbience(false));
                                                    StartCoroutine(FadeMusic(true));
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
                                    if (!experiments[1].gameObject.activeSelf)
                                    {
                                        experiments[1].SetActive(true);
                                        StartCoroutine(ThreeTwoOne(() =>
                                        {
                                            twoIndicators.ActivateRandom();
                                            indicatorsHandled++;
                                            timer.StartTimer();
                                        }));
                                    }
                                    timer.Update();
                                    if (indicatorsSelected > 8)
                                    {
                                        metrics.e1bT = timer.time;
                                        timer.time = 0f;
                                        experiments[1].SetActive(false);
                                        experimentIdx = 2;
                                        testStarted = false;
                                        StartCoroutine(FadeMusic(false));
                                    }
                                    else
                                    {
                                        if (indicatorsSelected == indicatorsHandled && indicatorsHandled != 0)
                                        {
                                            twoIndicators.ActivateRandom();
                                            indicatorsHandled++;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (!testStarted)
                                {
                                    if (!dialog.activeSelf)
                                    {
                                        ToggleDialog(true);
                                        StartCoroutine(FadeAmbience(true));
                                        questionsDialog.text = "<b>Experiment #1c</b>\nAlerts will appear in and out of your view.  Use the laser pointer to answer the yes/no questions appearing on the alerts. Please do so as quickly as possible.\n<b>Remember</b>: you can use the right thumbpad to rotate your view.";
                                        questionsDialog.question = false;
                                        Color actionColor = Color.black;
                                        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                        questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Start",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            ToggleDialog(false);
                                            TogglePointer(true);
                                            StartCoroutine(FadeAmbience(false));
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
                                    if (!experiments[2].gameObject.activeSelf)
                                    {
                                        experiments[2].SetActive(true);
                                        StartCoroutine(ThreeTwoOne(() =>
                                        {
                                            alerts.Begin();
                                            timer.StartTimer();
                                        }));
                                    }
                                    timer.Update();
                                    if (alerts.completed)
                                    {
                                        timer.StopTimer();
                                        timer.time = 0f;
                                        experiments[2].SetActive(false);
                                        testCompleted = true;
                                    }
                                }
                                break;
                        }
                        break;
                    } else
                    {
                        if (!dialog.activeSelf)
                        {
                            ToggleDialog(true);
                            StartCoroutine(FadeAmbience(true));
                            questionsDialog.text = "<b>Experiment #1c</b>\nThis is where a self-assessment manikin will be for the different kind of alerts.";
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
                                        SetSceneIdx(4);
                                        experimentIdx = 3;
                                        testStarted = false;
                                        testCompleted = false;
                                    },
                                    background = actionColor,
                                    requireAnswer = false
                                }
                            });
                        }
                    }
                }
                break;
            case 4: // Menus
                if (!testStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        StartCoroutine(FadeAmbience(true));
                        questionsDialog.text = "<b>Experiment #1:</b>\nA series of menus will appear in your view.  You will be prompted to interact with each menu in one of 3 ways.";
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
                                    techniqueIdx = 0;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    } else
                    {
                        switch (techniqueIdx)
                        {
                            case 0:
                                techniqueIdx = -1;
                                questionsDialog.text = "Before we get started, you'll get to try each menu interaction technique.";
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
                                            techniqueIdx = 1;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 1:
                                techniqueIdx = -1;
                                questionsDialog.text = "<b>Technique #1: Laser Pointer</b>\nYou may recognize this technique...prove that you haven't forgotten by selecting 'Next'.";
                                questionsDialog.question = false;
                                actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Next",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            techniqueIdx = 2;
                                            TogglePointer(false);
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 2:
                                techniqueIdx = -1;
                                technique2Button.transform.parent.gameObject.SetActive(true);
                                technique2Button.GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    questionsDialog.Reset();
                                    technique2Button.transform.parent.gameObject.SetActive(false);
                                    successSound.Stop();
                                    successSound.Play();
                                    techniqueIdx = 3;
                                });
                                questionsDialog.text = "<b>Technique #2: Controller Touch</b>\nFor this technique, touch the UI button with your controller and pull the trigger to make a selection.";
                                questionsDialog.question = false;
                                break;
                            case 3:
                                techniqueIdx = -1;
                                technique3Buttons.transform.parent.gameObject.SetActive(true);
                                technique3Buttons.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    questionsDialog.Reset();
                                    technique3Buttons.transform.parent.gameObject.SetActive(false);
                                    successSound.Stop();
                                    successSound.Play();
                                    techniqueIdx = 4;
                                });
                                questionsDialog.text = "<b>Technique #3: Thumbpad</b>\nWith this technique, a button in view is highlighted.  Press on the edges of the thumbpad on the left controller to change which button is highlighted.  Pull the trigger to select the 'Correct' button.";
                                questionsDialog.question = false;
                                break;
                            case 4:
                                techniqueIdx = -1;
                                TogglePointer(true);
                                questionsDialog.text = "Experiment #1 will now begin.  Use the prompted interaction technique and the menus that appear to enter the 6 displayed sequences, much like in Skill Test #3.";
                                questionsDialog.question = false;
                                actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Start",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            ToggleDialog(false);
                                            StartCoroutine(FadeAmbience(false));
                                            StartCoroutine(FadeMusic(true));
                                            testStarted = true;
                                            StartCoroutine(ThreeTwoOne(() =>
                                            {
                                                menusPrompt.SetActive(true);
                                                touchNumberPad.SetActive(true);
                                                timer.text = stringsTimeText;
                                                timer.time = 0f;
                                                timer.StartTimer();
                                                keyboard.promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                                stringsHandled++;
                                            }));
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                        }
                    }
                }
                else
                {
                    timer.Update();
                    if (stringsCompleted > 5)
                    {
                        metrics.e2TC6 = timer.time;
                        menusPrompt.SetActive(false);
                        headNumberPad.SetActive(false);
                        SetSceneIdx(5);
                        testStarted = false;
                        ToggleNumberPad(false);
                        StartCoroutine(FadeMusic(false));
                    }
                    else
                    {
                        if (stringsCompleted == stringsHandled && stringsHandled != 0)
                        {
                            timer.StartTimer();
                            stringsHandled++;
                            switch (stringsCompleted)
                            {
                                case 1:
                                    metrics.e2TC1 = timer.time;
                                    touchNumberPad.SetActive(false);
                                    headKeyboard.SetActive(true);
                                    TogglePointer(true);
                                    string newPrompt = "";
                                    for (int i = 0; i < 6; i++)
                                    {
                                        newPrompt += letters[UnityEngine.Random.Range(0, letters.Count - 1)];
                                    }
                                    keyboard.promptText = newPrompt;
                                    break;
                                case 2:
                                    metrics.e2TC2 = timer.time;
                                    headKeyboard.SetActive(false);
                                    TogglePointer(false);
                                    if (dominantRight) leftHandNumberPad.SetActive(true);
                                    else rightHandNumberPad.SetActive(true);
                                    keyboard.promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    break;
                                case 3:
                                    metrics.e2TC3 = timer.time;
                                    if (dominantRight) leftHandNumberPad.SetActive(false);
                                    else rightHandNumberPad.SetActive(false);
                                    buttonKeyboard.SetActive(true);
                                    newPrompt = "";
                                    for (int i = 0; i < 6; i++)
                                    {
                                        newPrompt += letters[UnityEngine.Random.Range(0, letters.Count - 1)];
                                    }
                                    keyboard.promptText = newPrompt;
                                    break;
                                case 4:
                                    metrics.e2TC4 = timer.time;
                                    buttonKeyboard.SetActive(false);
                                    if (dominantRight) leftHandKeyboard.SetActive(true);
                                    else rightHandKeyboard.SetActive(true);
                                    TogglePointer(true);
                                    newPrompt = "";
                                    for (int i = 0; i < 6; i++)
                                    {
                                        newPrompt += letters[UnityEngine.Random.Range(0, letters.Count - 1)];
                                    }
                                    keyboard.promptText = newPrompt;
                                    break;
                                case 5:
                                    metrics.e2TC5 = timer.time;
                                    if (dominantRight) leftHandKeyboard.SetActive(false);
                                    else rightHandKeyboard.SetActive(false);
                                    TogglePointer(false);
                                    headNumberPad.SetActive(true);
                                    keyboard.promptText = UnityEngine.Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    break;
                            }
                        }
                    }
                }
                break;
            case 5: // Movement
                if (!testStarted)
                {
                    if (!dialog.activeSelf)
                    {
                        ToggleDialog(true);
                        StartCoroutine(FadeAmbience(true));
                        questionsDialog.text = "You've done well!  Just one more test.  Follow the directions and complete the movements, and you'll be done soon.";
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
                                    UpdateProgress(3);
                                    secondScreen = true;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    }
                    else if (secondScreen)
                    {
                        secondScreen = false;
                        questionsDialog.text = "<b>Experiment #2</b>\nThis final experiment involves movement, using one of two movement techniques.";
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
                                    techniqueIdx = 0;
                                    secondScreen = false;
                                },
                                background = actionColor,
                                requireAnswer = false
                            }
                        });
                    } else
                    {
                        switch (techniqueIdx)
                        {
                            case 0:
                                techniqueIdx = -1;
                                questionsDialog.text = "<b>Experiment #2</b>\nBefore we get started with the experiment, you'll get to try the two movement techniques.";
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
                                            techniqueIdx = 1;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 1:
                                techniqueIdx = -1;
                                teleport.enabled = true;
                                Teleport.instance.CancelTeleportHint();
                                questionsDialog.text = "<b>Technique #1: Teleport</b>\nPress down on the left thumbpad to open the teleport interface.  Release the button to teleport.\nTry it!";
                                questionsDialog.question = false;
                                actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Next",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            joystick.transform.position = Vector3.zero;
                                            joystick.transform.eulerAngles = Vector3.zero;
                                            teleport.enabled = false;
                                            techniqueIdx = 3;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 2:
                                techniqueIdx = -1;
                                teleport.blur = true;
                                questionsDialog.text = "<b>Technique #2: Fast Travel</b>\nAgain, press down on the left thumbpad to open the interface.  Release the button to fast travel.\nAfter you've tried it a few times, come back and press 'Next'.";
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
                                            teleport.blur = false;
                                            techniqueIdx = 1;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    },
                                    new VRDialogActionValues()
                                    {
                                        text = "Next",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            teleport.enabled = false;
                                            teleport.blur = false;
                                            techniqueIdx = 4;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 3:
                                techniqueIdx = -1;
                                joystick.enabled = true;
                                questionsDialog.text = "<b>Technique #2: Thumbpad Joystick</b>\nTouch the thumbpad on your left controller and use it like a joysick to control your current position.\nTry it!";
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
                                            joystick.enabled = false;
                                            teleport.enabled = true;
                                            techniqueIdx = 1;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    },
                                    new VRDialogActionValues()
                                    {
                                        text = "Next",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            joystick.enabled = false;
                                            joystick.transform.position = Vector3.zero;
                                            joystick.transform.eulerAngles = Vector3.zero;
                                            techniqueIdx = 4;
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                            case 4:
                                techniqueIdx = -1;
                                questionsDialog.text = "Experiment #2 will now begin.  Use the prompted movement technique to complete 3 laps as quickly as possible.";
                                questionsDialog.question = false;
                                actionColor = Color.black;
                                ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
                                questionsDialog.SetActions(new List<VRDialogActionValues>()
                                {
                                    new VRDialogActionValues()
                                    {
                                        text = "Start",
                                        callback = answer => {
                                            questionsDialog.Reset();
                                            ToggleDialog(false);
                                            StartCoroutine(FadeAmbience(false));
                                            StartCoroutine(FadeMusic(true));
                                            testStarted = true;
                                            lapStuff.SetActive(true);
                                            joystick.transform.position = new Vector3(0f, joystick.transform.position.y, 9f);
                                            joystick.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                                            threeTwoOne.transform.position = new Vector3(2.5f, 1.5f, 9f);
                                            threeTwoOne.transform.eulerAngles = new Vector3(0, 90f, 0f);
                                            StartCoroutine(ThreeTwoOne(() =>
                                            {
                                                techniqueIdx = rand.NextDouble() > 0.5 ? 0 : 1;
                                                if (techniqueIdx == 0)
                                                {
                                                    teleport.enabled = true;
                                                    teleport.blur = false;
                                                    techniqueText.text = "Teleport";
                                                } else
                                                {
                                                    joystickStuff.SetActive(true);
                                                    joystick.enabled = true;
                                                    tracker.joystick = true;
                                                    techniqueText.text = "Joystick";
                                                }
                                                tracker.track = true;
                                            }));
                                        },
                                        background = actionColor,
                                        requireAnswer = false
                                    }
                                });
                                break;
                        }
                    }
                }
                else
                {
                    switch (techniqueIdx)
                    {
                        case 0:
                            if (tracker.completed)
                            {
                                teleport.enabled = false;
                                tracker.completed = false;
                                if (oneDone)
                                {
                                    joystick.transform.position = Vector3.zero;
                                    joystick.transform.eulerAngles = Vector3.zero;
                                    lapStuff.SetActive(false);
                                    SetSceneIdx(6);
                                    StartCoroutine(FadeMusic(false));
                                    StartCoroutine(FadeAmbience(true));
                                } else
                                {
                                    oneDone = true;
                                    joystick.transform.position = new Vector3(0f, joystick.transform.position.y, 9f);
                                    joystick.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                                    techniqueIdx = 1;
                                    joystickStuff.SetActive(true);
                                    StartCoroutine(ThreeTwoOne(() =>
                                    {
                                        joystick.enabled = true;
                                        tracker.joystick = true;
                                        techniqueText.text = "Joystick";
                                    }));
                                }
                            }
                            break;
                        case 2:
                            if (tracker.completed)
                            {
                                techniqueIdx = 2;
                                tracker.completed = false;
                                teleport.enabled = false;
                                teleport.blur = false;
                                joystickStuff.SetActive(true);
                                StartCoroutine(WaitThenExecute(0.5f, () => {
                                    joystick.transform.position = new Vector3(0f, joystick.transform.position.y, 9f);
                                    joystick.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                                    StartCoroutine(ThreeTwoOne(() =>
                                    {
                                        joystick.enabled = true;
                                        tracker.joystick = true;
                                        techniqueText.text = "Joystick";
                                    }));
                                }));
                            }
                            break;
                        case 1:
                            if (tracker.completed)
                            {
                                joystick.enabled = false;
                                joystickStuff.SetActive(false);
                                tracker.completed = false;
                                if (oneDone)
                                {
                                    joystick.transform.position = Vector3.zero;
                                    joystick.transform.eulerAngles = Vector3.zero;
                                    lapStuff.SetActive(false);
                                    SetSceneIdx(6);
                                    StartCoroutine(FadeMusic(false));
                                    StartCoroutine(FadeAmbience(true));
                                }
                                else
                                {
                                    oneDone = true;
                                    techniqueIdx = 0;
                                    joystick.transform.position = new Vector3(0f, joystick.transform.position.y, 9f);
                                    joystick.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                                    StartCoroutine(ThreeTwoOne(() =>
                                    {
                                        teleport.enabled = true;
                                        teleport.blur = false;
                                        techniqueText.text = "Teleport";
                                    }));
                                }
                            }
                            break;
                    }
                }
                break;
            case 6:
                if (reloadTime > 10f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                reloadTime += Time.deltaTime;
                break;
        }
        
    }

    private void TogglePointer(bool show)
    {
        rightPointer.active = show && dominantRight;
        leftPointer.active = show && !dominantRight;
    }

    private IEnumerator FadeAmbience(bool on)
    {
        while ((on && ambience.volume < 0.2f) || (!on && ambience.volume > 0f))
        {
            ambience.volume += on ? 0.01f : -0.01f;
            yield return null;
        }
    }

    private IEnumerator FadeMusic(bool on)
    {
        if (on) music.Play();
        while ((on && music.volume < 0.2f) || (!on && music.volume > 0f))
        {
            music.volume += on ? 0.01f : -0.01f;
            yield return null;
        }
        if (!on) music.Stop();
    }

    public void SetSceneIdx (int idx)
    {
        scenes[sceneIdx].SetActive(false);
        sceneIdx = idx;
        scenes[sceneIdx].SetActive(true);
    }

    private void ToggleDialog(bool show)
    {
        progressBar.SetActive(show);
        dialog.SetActive(show);
        TogglePointer(show);
    }

    private void ToggleNumberPad(bool show)
    {
        numberPad.SetActive(show);
        TogglePointer(show);
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
        threeTwoOneSound.Stop();
        threeTwoOneSound.Play();
        threeTwoOne.GetComponentInChildren<Text>().text = "3";
        yield return new WaitForSeconds(1f);
        threeTwoOneSound.Stop();
        threeTwoOneSound.Play();
        threeTwoOne.GetComponentInChildren<Text>().text = "2";
        yield return new WaitForSeconds(1f);
        threeTwoOneSound.Stop();
        threeTwoOneSound.Play();
        threeTwoOne.GetComponentInChildren<Text>().text = "1";
        yield return new WaitForSeconds(1f);
        threeTwoOne.SetActive(false);
        if (callable != null) callable();
    }

    public void LeftRotated()
    {
        Color actionColor = Color.black;
        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
        questionsDialog.SetActions(new List<VRDialogActionValues>()
        {
            new VRDialogActionValues()
            {
                text = "Okay",
                callback = answer => {
                    questionsDialog.Reset();
                    joystick.transform.eulerAngles = Vector3.zero;
                    ControllerButtonHints.HideAllButtonHints(rightPointer.GetComponent<Hand>());
                    questionIdx = 6;
                },
                background = actionColor,
                requireAnswer = false
            }
        });
    }

    public void RightRotated()
    {
        Color actionColor = Color.black;
        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
        questionsDialog.SetActions(new List<VRDialogActionValues>()
        {
            new VRDialogActionValues()
            {
                text = "Okay",
                callback = answer => {
                    questionsDialog.Reset();
                    joystick.transform.eulerAngles = Vector3.zero;
                    ControllerButtonHints.HideAllButtonHints(rightPointer.GetComponent<Hand>());
                    questionIdx = 7;
                },
                background = actionColor,
                requireAnswer = false
            }
        });
    }

    private void UpdateProgress(int idx)
    {
        Color actionColor = Color.black;
        ColorUtility.TryParseHtmlString("#46ACC2", out actionColor);
        for (int i = 0; i <= idx; i++)
        {
            progressBar.transform.GetChild(i).GetComponent<ProceduralImage>().color = actionColor;
        }
    }
}
