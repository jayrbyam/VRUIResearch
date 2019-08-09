using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Joystick : MonoBehaviour
{

    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_DPad[SteamVR_Input_Sources.LeftHand].active)
        {
            Vector2 axis = SteamVR_Actions.default_DPad.GetAxis(SteamVR_Input_Sources.LeftHand) * 0.1f;
            transform.position += new Vector3(head.forward.x * axis.y, 0f, head.forward.z * axis.y);
            transform.position += new Vector3(head.right.x * axis.x, 0f, head.right.z * axis.x);
        }
    }
}
