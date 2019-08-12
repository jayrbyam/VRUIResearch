using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackPlayer : MonoBehaviour
{
    public bool track = false;
    public bool completed = false;
    public Text lapCounter;
    public bool joystick = false;
    private int lap = 0;
    private bool firstCorner = false;
    private bool secondCorner = false;
    private bool thirdCorner = false;
    private bool lastCorner = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (track && !completed)
        {
            if (!firstCorner && transform.position.x > 6 && transform.position.z < 6f) firstCorner = true;
            if (firstCorner && !secondCorner && transform.position.x < 6f && transform.position.z < -6f) secondCorner = true;
            if (firstCorner && secondCorner && !thirdCorner && transform.position.x < -6f && transform.position.z > -6f) thirdCorner = true;
            if (firstCorner && secondCorner && thirdCorner && !lastCorner && transform.position.x > -6f && transform.position.z > 6f) lastCorner = true;
            if (firstCorner && secondCorner && thirdCorner && lastCorner && transform.position.x > 0f)
            {
                lap++;
                if (lap == 1)
                {
                    if (!joystick) MainController.Instance.secondLapStuff.SetActive(true);
                    else
                    {
                        MainController.Instance.joystickSecondLapStuff.SetActive(true);
                        MainController.Instance.joystick.second = true;
                    }
                    
                }
                else if (lap == 2)
                {
                    if (!joystick) MainController.Instance.thirdLapStuff.SetActive(true);
                    else
                    {
                        MainController.Instance.joystickThirdLapStuff.SetActive(true);
                        MainController.Instance.joystick.third = true;
                    }
                }
                firstCorner = false;
                secondCorner = false;
                thirdCorner = false;
                lastCorner = false;
                if (lap == 3)
                {
                    completed = true;
                    lap = 0;
                    if (!joystick)
                    {
                        MainController.Instance.secondLapStuff.SetActive(false);
                        MainController.Instance.thirdLapStuff.SetActive(false);
                    } else
                    {
                        MainController.Instance.joystick.second = false;
                        MainController.Instance.joystick.third = false;
                        MainController.Instance.joystickSecondLapStuff.SetActive(false);
                        MainController.Instance.joystickThirdLapStuff.SetActive(false);
                    }
                }
            }
            lapCounter.text = "Lap " + (lap + 1).ToString();
        }
    }
}
