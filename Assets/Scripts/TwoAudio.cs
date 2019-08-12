using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoAudio : MonoBehaviour
{
    public List<AudioClip> sounds;
    public AudioSource left;
    public AudioSource right;
    private bool leftEmitting = false;
    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmitRandom()
    {
        leftEmitting = rand.NextDouble() > 0.5;
        if (leftEmitting)
        {
            int idx = Random.Range(0, sounds.Count - 1);
            left.clip = sounds[idx];
            sounds.RemoveAt(idx);
            left.Play();
        }
        else
        {
            int idx = Random.Range(0, sounds.Count - 1);
            right.clip = sounds[idx];
            sounds.RemoveAt(idx);
            right.Play();
        }
    }

    public void LeftSelected()
    {
        left.Stop();
        right.Stop();
        if (leftEmitting) MainController.Instance.metrics.e1aCA++;
        MainController.Instance.audioSourcesSelected++;
    }

    public void RightSelected()
    {
        left.Stop();
        right.Stop();
        if (!leftEmitting) MainController.Instance.metrics.e1aCA++;
        MainController.Instance.audioSourcesSelected++;
    }
}
