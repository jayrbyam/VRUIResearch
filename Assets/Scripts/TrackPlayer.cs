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
                    if (!joystick)
                    {
                        MainController.Instance.secondLapStuff.SetActive(true);
                        MainController.Instance.metrics.e2TTL1 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
                    }
                    else
                    {
                        MainController.Instance.joystickSecondLapStuff.SetActive(true);
                        MainController.Instance.joystick.second = true;
                        MainController.Instance.metrics.e2JTL1 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
                    }
                    
                }
                else if (lap == 2)
                {
                    if (!joystick)
                    {
                        MainController.Instance.thirdLapStuff.SetActive(true);
                        MainController.Instance.metrics.e2TTL2 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
                    }
                    else
                    {
                        MainController.Instance.joystickThirdLapStuff.SetActive(true);
                        MainController.Instance.joystick.third = true;
                        MainController.Instance.metrics.e2JTL2 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
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
                        MainController.Instance.metrics.e2TTL3 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
                    } else
                    {
                        MainController.Instance.joystick.second = false;
                        MainController.Instance.joystick.third = false;
                        MainController.Instance.joystickSecondLapStuff.SetActive(false);
                        MainController.Instance.joystickThirdLapStuff.SetActive(false);
                        MainController.Instance.metrics.e2JTL3 = MainController.Instance.timer.time;
                        MainController.Instance.timer.time = 0f;
                    }
                }
            }
            lapCounter.text = "Lap " + (lap + 1).ToString();
        }
    }
}
