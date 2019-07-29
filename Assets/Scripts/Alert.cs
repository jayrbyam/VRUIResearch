using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public bool yesCorrect = false;
    public bool answered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.name != "VR Camera") transform.LookAt(GameObject.Find("VR Camera").transform.position);
    }

    public void YesSelected()
    {
        // Record whether answer was correct or not
        answered = true;
    }

    public void NoSelected()
    {
        // Record whether answer was correct or not
        answered = true;
    }
}
