using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TwoIndicators : MonoBehaviour
{
    public Indicator left;
    public Indicator right;
    private bool leftActive = false;
    private System.Random rand;
    private bool activating = false;

    // Start is called before the first frame update
    void Start()
    {
        left.Activate(false);
        right.Activate(false);
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateRandom()
    {
        activating = true;
        StartCoroutine(WaitThenActivate());
        
    }

    public void LeftSelected()
    {
        if (activating) return;
        left.Activate(false);
        right.Activate(false);
        MainController.Instance.timer.StopTimer();
        if (leftActive) MainController.Instance.metrics.e1bCA++;
        MainController.Instance.indicatorsSelected++;
    }

    public void RightSelected()
    {
        if (activating) return;
        left.Activate(false);
        right.Activate(false);
        MainController.Instance.timer.StopTimer();
        if (!leftActive) MainController.Instance.metrics.e1bCA++;
        MainController.Instance.indicatorsSelected++;
    }

    private IEnumerator WaitThenActivate()
    {
        yield return new WaitForSeconds(1f);
        MainController.Instance.timer.StartTimer();
        activating = false;
        leftActive = rand.NextDouble() > 0.5;
        left.Activate(leftActive);
        right.Activate(!leftActive);
    }
}
