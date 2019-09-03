using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Joystick : MonoBehaviour
{

    public Transform head;
    public List<BoxCollider> objects;
    public bool second = false;
    public List<BoxCollider> secondObjects;
    public bool third = false;
    public List<BoxCollider> thirdObjects;
    public bool firstAction = false;
    private bool setTimeTilAction = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_DPad[SteamVR_Input_Sources.LeftHand].axis != Vector2.zero)
        {
            if (MainController.Instance.sceneIdx == 5 && MainController.Instance.testStarted && MainController.Instance.techniqueIdx == 1 && setTimeTilAction)
            {
                setTimeTilAction = false;
                firstAction = true;
            }
            Vector2 axis = SteamVR_Actions.default_DPad.GetAxis(SteamVR_Input_Sources.LeftHand) * 0.1f;
            Vector3 oldPosition = transform.position;
            transform.position += new Vector3(head.forward.x * axis.y, 0f, 0f);
            if (Overlaps()) transform.position = oldPosition;
            else oldPosition = transform.position;
            transform.position += new Vector3(0f, 0f, head.forward.z * axis.y);
            if (Overlaps()) transform.position = oldPosition;
            else oldPosition = transform.position;
            transform.position += new Vector3(head.right.x * axis.x, 0f, 0f);
            if (Overlaps()) transform.position = oldPosition;
            else oldPosition = transform.position;
            transform.position += new Vector3(0f, 0f, head.right.z * axis.x);
            if (Overlaps()) transform.position = oldPosition;

            if (transform.position.x < -11.25) {
                transform.position = new Vector3(-11.25f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 11.25) {
                transform.position = new Vector3(11.25f, transform.position.y, transform.position.z);
            }
            if (transform.position.z < -11.25) {
                transform.position = new Vector3(transform.position.x, transform.position.y, -11.25f);
            }
            if (transform.position.z > 11.25)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 11.25f);
            }
        }
    }

    private bool Overlaps()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].bounds.Contains(transform.position)) return true;
        }
        if (second)
        {
            for (int i = 0; i < secondObjects.Count; i++)
            {
                if (secondObjects[i].bounds.Contains(transform.position)) return true;
            }
        }
        if (third)
        {
            for (int i = 0; i < thirdObjects.Count; i++)
            {
                if (thirdObjects[i].bounds.Contains(transform.position)) return true;
            }
        }
        return false;
    }
}
