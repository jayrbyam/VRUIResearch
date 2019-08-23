using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RotatePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_DPadLeft[SteamVR_Input_Sources.RightHand].stateDown)
        {
            transform.eulerAngles += new Vector3(0f, -40f, 0f);
            if (MainController.Instance.waitingForLeftRotate)
            {
                MainController.Instance.waitingForLeftRotate = false;
                MainController.Instance.LeftRotated();
            }
        }
        if (SteamVR_Actions.default_DPadRight[SteamVR_Input_Sources.RightHand].stateDown)
        {
            transform.eulerAngles += new Vector3(0f, 40f, 0f);
            if (MainController.Instance.waitingForRightRotate)
            {
                MainController.Instance.waitingForRightRotate = false;
                MainController.Instance.RightRotated();
            }
        }
    }
}
