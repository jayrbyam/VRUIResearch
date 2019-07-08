using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("VRCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - player.position);
    }
}
