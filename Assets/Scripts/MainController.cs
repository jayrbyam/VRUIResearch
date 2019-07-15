using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Transform darts;
    public static int dartsThrown = 0;
    public GameObject leftRacket;
    public GameObject rightRacket;
    public GameObject tennisBall;
    public TennisShooter shooter;
    public static int numbersCompleted = 0;
    private int numbersHandled = 0;

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
                            questionsDialog.text = "Nice work! You will now proceed to the pre-experiment skill test.";
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
                                questionsDialog.text = "Our first test is a simple game of 'Virtually Throw the Virtual Darts at the Virtual Dartboard'. Use the trigger to grab and throw each dart. Remember, you will be timed and the score will be recorded.";
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
                            }
                            
                            if (dartsThrown == 3)
                            {
                                StartCoroutine(WaitThenExecute(3f, () =>
                                {
                                    skillTests[0].gameObject.SetActive(false);
                                    testIdx = 1;
                                    testStarted = false;
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
                                questionsDialog.text = "Now, lets test your reaction time.  You will be given a racket and tennis balls will be shot toward you.  Hit a ball, get a point.";
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

                                StartCoroutine(WaitThenExecute(1f, () =>
                                {
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
                                    shooter.active = true;
                                }));
                            }


                            if (shooter.ballsShot == 9)
                            {
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
                                questionsDialog.text = "Lastly, your speed with a menu will be tested.  When prompted, use the laser pointer to enter the displayed sequence on the number pad, as fast and as accuractely as you can.";
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
                            }
                            
                            if (numbersCompleted > 5)
                            {
                                sceneIdx = 3;
                            } else
                            {
                                if (numbersCompleted == numbersHandled)
                                {
                                    numberPad.GetComponent<VRNumberPad>().promptText = Random.Range(0, 999999).ToString().PadLeft(6, '0');
                                    numbersHandled++;
                                }
                            }
                        }
                        break;
                }
                break;
            case 3:

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
            float newX = tennisBall.transform.localPosition.x + Random.Range(-0.1f, 0.1f);
            float newY = tennisBall.transform.localPosition.y + Random.Range(-0.1f, 0.1f);
            float newZ = tennisBall.transform.localPosition.z + Random.Range(-0.1f, 0.1f);
            newBall.transform.localPosition = new Vector3(newX, newY, newZ);
            yield return null;
        }
    }
}
