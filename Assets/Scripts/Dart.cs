using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{

    private bool grabbed = false;
    private bool thrown = false;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += DartGrabbed;
        //GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += DartThrown;
    }

    // Update is called once per frame
    void Update()
    {
        //Rigidbody rigidbody = GetComponent<Rigidbody>();
        //VRTK_InteractableObject dart = GetComponent<VRTK_InteractableObject>();

        //if (grabbed && thrown)
        //{
        //    transform.LookAt(transform.position + rigidbody.velocity);
        //}
    }

    //private void DartGrabbed(object sender, InteractableObjectEventArgs e)
    //{
    //    grabbed = true;
    //}

    //private void DartThrown(object sender, InteractableObjectEventArgs e)
    //{
    //    thrown = true;
    //}
}
