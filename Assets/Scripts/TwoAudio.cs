using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoAudio : MonoBehaviour
{
    public List<AudioClip> sounds;
    public AudioSource left;
    public AudioSource right;
    private bool leftEmitting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmitRandom()
    {
        leftEmitting = Random.Range(0, 2) == 0;
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
        // In future, record whether selection was correct or not
        MainController.Instance.audioSourcesSelected++;
    }

    public void RightSelected()
    {
        left.Stop();
        right.Stop();
        // In future, record whether selection was correct or not
        MainController.Instance.audioSourcesSelected++;
    }
}
