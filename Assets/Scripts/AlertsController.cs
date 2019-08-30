using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsController : MonoBehaviour
{
    public List<Alert> alerts;
    public bool completed = false;
    private bool started = false;
    private int alertIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Mathf.Abs(MainController.Instance.visualCues.angle) < 30 && timeForAlert() == -1) setTimeForAlertView();

        if (started && alerts[alertIdx].answered)
        {
            //setTimeForAlertAction();
            alertIdx++;
            MainController.Instance.visualCues.show = alertIdx == 2 || alertIdx == 4;
            if (alertIdx == alerts.Count)
            {
                started = false;
                completed = true;
            } else
            {
                MainController.Instance.visualCues.target = alerts[alertIdx].transform;
                alerts[alertIdx - 1].gameObject.SetActive(false);
                alerts[alertIdx].gameObject.SetActive(true);
            }
        }
    }

    public void Begin()
    {
        started = true;
        alerts[alertIdx].gameObject.SetActive(true);
    }

    //private float timeForAlert()
    //{
    //    if (alertIdx == 0) return MainController.Instance.metrics.e1cTV1;
    //    if (alertIdx == 1) return MainController.Instance.metrics.e1cTV2;
    //    if (alertIdx == 2) return MainController.Instance.metrics.e1cTV3;
    //    if (alertIdx == 3) return MainController.Instance.metrics.e1cTV4;
    //    if (alertIdx == 4) return MainController.Instance.metrics.e1cTV5;
    //    if (alertIdx == 5) return MainController.Instance.metrics.e1cTV6;
    //    if (alertIdx == 6) return MainController.Instance.metrics.e1cTV7;
    //    if (alertIdx == 7) return MainController.Instance.metrics.e1cTV8;
    //    if (alertIdx == 8) return MainController.Instance.metrics.e1cTV9;
    //    return MainController.Instance.metrics.e1cTV10;
    //}

    //private void setTimeForAlertView()
    //{
    //    if (alertIdx == 0) MainController.Instance.metrics.e1cTV1 = MainController.Instance.timer.time;
    //    else if (alertIdx == 1) MainController.Instance.metrics.e1cTV2 = MainController.Instance.timer.time;
    //    else if (alertIdx == 2) MainController.Instance.metrics.e1cTV3 = MainController.Instance.timer.time;
    //    else if (alertIdx == 3) MainController.Instance.metrics.e1cTV4 = MainController.Instance.timer.time;
    //    else if (alertIdx == 4) MainController.Instance.metrics.e1cTV5 = MainController.Instance.timer.time;
    //    else if (alertIdx == 5) MainController.Instance.metrics.e1cTV6 = MainController.Instance.timer.time;
    //    else if (alertIdx == 6) MainController.Instance.metrics.e1cTV7 = MainController.Instance.timer.time;
    //    else if (alertIdx == 7) MainController.Instance.metrics.e1cTV8 = MainController.Instance.timer.time;
    //    else if (alertIdx == 8) MainController.Instance.metrics.e1cTV9 = MainController.Instance.timer.time;
    //    else MainController.Instance.metrics.e1cTV10 = MainController.Instance.timer.time;
    //}

    //private void setTimeForAlertAction()
    //{
    //    if (alertIdx == 0) MainController.Instance.metrics.e1cTA1 = MainController.Instance.timer.time;
    //    else if (alertIdx == 1) MainController.Instance.metrics.e1cTA2 = MainController.Instance.timer.time;
    //    else if (alertIdx == 2) MainController.Instance.metrics.e1cTA3 = MainController.Instance.timer.time;
    //    else if (alertIdx == 3) MainController.Instance.metrics.e1cTA4 = MainController.Instance.timer.time;
    //    else if (alertIdx == 4) MainController.Instance.metrics.e1cTA5 = MainController.Instance.timer.time;
    //    else if (alertIdx == 5) MainController.Instance.metrics.e1cTA6 = MainController.Instance.timer.time;
    //    else if (alertIdx == 6) MainController.Instance.metrics.e1cTA7 = MainController.Instance.timer.time;
    //    else if (alertIdx == 7) MainController.Instance.metrics.e1cTA8 = MainController.Instance.timer.time;
    //    else if (alertIdx == 8) MainController.Instance.metrics.e1cTA9 = MainController.Instance.timer.time;
    //    else MainController.Instance.metrics.e1cTA10 = MainController.Instance.timer.time;
    //}
}
