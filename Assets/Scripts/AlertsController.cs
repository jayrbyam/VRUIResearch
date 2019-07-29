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
        if (started && alerts[alertIdx].answered)
        {
            alertIdx++;
            if (alertIdx == alerts.Count)
            {
                started = false;
                completed = true;
            } else
            {
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
}
