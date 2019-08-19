using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCues : MonoBehaviour
{
    public bool show = false;
    public GameObject left;
    public GameObject right;
    public Transform target;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);

        if (show)
        {
            if (angle > 0)
            {
                left.SetActive(true);
                right.SetActive(false);
            }
            else
            {
                left.SetActive(false);
                right.SetActive(true);
            }

            if (Mathf.Abs(angle) < 20)
            {
                left.SetActive(false);
                right.SetActive(false);
            }
        } else
        {
            left.SetActive(false);
            right.SetActive(false);
        }
    }
}
