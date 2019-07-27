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
    public CapsuleCollider collider;
    private bool grabbed = false;
    private bool thrown = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().onAttachedToHand += onAttach;
        GetComponent<Interactable>().onDetachedFromHand += onDetach;
        textHint.SetActive(false);
    }

    private void onDetach(Hand hand)
    {
        collider.enabled = true;
        grabbed = false;
        thrown = true;
    }

    private void onAttach(Hand hand)
    {
        collider.enabled = false;
        grabbed = true;
        thrown = false;
        if (focused != null)
        {
            Destroy(focused.gameObject);
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
            GetComponent<AudioSource>().Play();
            thrown = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            if (collision.gameObject.tag == "Dartboard")
            {
                textHint.GetComponentInChildren<Text>().text = "+1";
                Color green = Color.green;
                ColorUtility.TryParseHtmlString("#4CD137", out green);
                textHint.GetComponentInChildren<ProceduralImage>().color = green;
                MainController.Instance.dartScore++;
            } else {
                textHint.GetComponentInChildren<Text>().text = "Miss";
                Color red = Color.red;
                ColorUtility.TryParseHtmlString("#E84118", out red);
                textHint.GetComponentInChildren<ProceduralImage>().color = red;
            }
            
            textHint.SetActive(true);
            textHint.transform.position = new Vector3(textHint.transform.position.x, textHint.transform.position.y + 0.1f, textHint.transform.position.z);
            highlight.SetActive(true);

            MainController.dartsThrown++;
            if (MainController.dartsThrown == 5) Invoke("DestroyThis", 3f);
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
