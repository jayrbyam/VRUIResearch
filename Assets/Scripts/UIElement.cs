﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: UIElement that responds to VR hands and generates UnityEvents
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Valve.VR.InteractionSystem;

//-------------------------------------------------------------------------
[RequireComponent(typeof(Interactable))]
public class UIElement : MonoBehaviour
{
    public CustomEvents.UnityEventHand onHandClick;

    protected Hand currentHand;

    //-------------------------------------------------
    protected virtual void Awake()
    {
        Button button = GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }


    //-------------------------------------------------
    protected virtual void OnHandHoverBegin(Hand hand)
    {
        currentHand = hand;
        InputModule.instance.HoverBegin(gameObject);
        ControllerButtonHints.ShowButtonHint(hand, hand.uiInteractAction);
    }


    //-------------------------------------------------
    protected virtual void OnHandHoverEnd(Hand hand)
    {
        InputModule.instance.HoverEnd(gameObject);
        ControllerButtonHints.HideButtonHint(hand, hand.uiInteractAction);
        currentHand = null;
    }


    //-------------------------------------------------
    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (hand.uiInteractAction != null && hand.uiInteractAction.GetStateDown(hand.handType))
        {
            InputModule.instance.Submit(gameObject);
            ControllerButtonHints.HideButtonHint(hand, hand.uiInteractAction);
        }
    }


    //-------------------------------------------------
    protected virtual void OnButtonClick()
    {
        onHandClick.Invoke(currentHand);
    }

    public void onHoverBegin(Hand hand)
    {
        currentHand = hand;
        InputModule.instance.HoverBegin(gameObject);
        ControllerButtonHints.ShowButtonHint(hand, hand.uiInteractAction);
    }

    public void onHoverEnd(Hand hand)
    {
        InputModule.instance.HoverEnd(gameObject);
        ControllerButtonHints.HideButtonHint(hand, hand.uiInteractAction);
        currentHand = null;
    }

    public void onButtonClick()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}
