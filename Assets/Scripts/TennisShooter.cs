using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisShooter : MonoBehaviour
{
    public GameObject tennisBall;
    public Transform launchPoint;
    public bool active = false;
    public int ballsShot = 0;
    private float upperAngle = 185f;
    private float lowerAngle = 175f;
    private bool decreasingAngle = true;
    private List<float> timings = new List<float>() { 3f, 6f, 8f, 10f, 12f, 13f, 14f, 15f, 15.5f, 15.75f };
    private float testTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (decreasingAngle ? -0.1f : 0.1f), 0f);
            if (transform.localEulerAngles.y > upperAngle) decreasingAngle = true;
            if (transform.localEulerAngles.y < lowerAngle) decreasingAngle = false;

            testTime += Time.deltaTime;
            if (ballsShot < 10 && testTime > timings[ballsShot])
            {
                GameObject newBall = Instantiate(tennisBall);
                newBall.SetActive(true);
                newBall.transform.SetParent(tennisBall.transform.parent);
                newBall.transform.localScale = tennisBall.transform.localScale;
                newBall.transform.position = launchPoint.position;
                newBall.GetComponent<Rigidbody>().AddForce(launchPoint.forward * 1000f);
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().Play();
                ballsShot++;
            }
        }
    }
}
