using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class Dart : MonoBehaviour
{
    public static Dart focused;
    public GameObject textHint;
    public GameObject highlight;
    private bool grabbed = false;
    private bool thrown = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().onAttachedToHand += onAttach;
        GetComponent<Interactable>().onDetachedFromHand += onDetach;
    }

    private void onDetach(Hand hand)
    {
        grabbed = false;
        thrown = true;
    }

    private void onAttach(Hand hand)
    {
        grabbed = true;
        thrown = false;
        if (focused != null)
        {
            focused.highlight.SetActive(false);
            focused.textHint.SetActive(false);
        }
        focused = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (thrown)
        {
            transform.up = -GetComponent<Rigidbody>().velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            thrown = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            if (collision.gameObject.tag == "Dartboard")
            {
                // Radius of dartboard is 0.35
                float toMiddle = Vector3.Distance(collision.transform.position, collision.contacts[0].point);
                float score = (1 - toMiddle / 0.35f) * 100;
                if (score < 1)
                {
                    textHint.GetComponentInChildren<Text>().text = "0";
                    Color red = Color.red;
                    ColorUtility.TryParseHtmlString("#E84118", out red);
                    textHint.GetComponentInChildren<ProceduralImage>().color = red;
                } else
                {
                    textHint.GetComponentInChildren<Text>().text = ((int)score).ToString();
                    Color green = Color.green;
                    ColorUtility.TryParseHtmlString("#4CD137", out green);
                    textHint.GetComponentInChildren<ProceduralImage>().color = green;
                }
            } else {
                textHint.GetComponentInChildren<Text>().text = "0";
                Color red = Color.red;
                ColorUtility.TryParseHtmlString("#E84118", out red);
                textHint.GetComponentInChildren<ProceduralImage>().color = red;
            }
            
            textHint.SetActive(true);
            textHint.transform.position = new Vector3(textHint.transform.position.x, textHint.transform.position.y + 0.1f, textHint.transform.position.z);
            highlight.SetActive(true);
        }
    }
}
