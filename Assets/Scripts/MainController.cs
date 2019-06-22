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
    public GameObject dialog;

    // Indeces for "scenes" in the experience
    // 0 - Start screen
    // 1 - Pre-experience questions
    private int sceneIdx = 0;

    // Scene 0
    public FadePulse startText;

    // Scene 1
    

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject child in scenes)
        {
            child.SetActive(false);
        }
        scenes[sceneIdx].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (sceneIdx)
        {
            case 0:
                if (leftControllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick) || rightControllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick))
                {
                    startText.Hide = true;
                }
                break;
            case 1:
                if (!dialog.activeSelf)
                {
                    dialog.SetActive(true);
                    VRDialog questionsDialog = dialog.GetComponent<VRDialog>();
                    questionsDialog.text = "How would you categorize yourself as a VR user?";
                    questionsDialog.question = true;
                    Color disabledActionColor = Color.grey;
                    ColorUtility.TryParseHtmlString("#A9BCD0", out disabledActionColor);
                    Color enabledActionColor = Color.black;
                    ColorUtility.TryParseHtmlString("#46ACC2", out enabledActionColor);
                    questionsDialog.SetActions(new List<VRDialogActionValues>()
                    {
                        new VRDialogActionValues()
                        {
                            text = "Okay",
                            callback = () => { Debug.Log("Okay pressed!"); },
                            disabledBackground = disabledActionColor,
                            enabledBackground = enabledActionColor
                        }
                    });
                    questionsDialog.SetAnswers(new List<string>() { "A", "B", "C", "D" });
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

    public void WheresMyCookies()
    {
        Debug.Log("Okay....");
    }
}
