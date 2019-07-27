using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LaserUIElement : UIElement
{

    public AudioSource hoverSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnter(Hand hand)
    {
        base.OnHandHoverBegin(hand);
        //if (hoverSound != null && !hoverSound.isPlaying) hoverSound.Play();
    }

    public void OnExit(Hand hand)
    {
        base.OnHandHoverEnd(hand);
    }

    public void HoverUpdate(Hand hand)
    {
        base.HandHoverUpdate(hand);
    }

}
