using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer audioMixer;
    public AudioMixer soundMixer;
    public GameObject panel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetSound(float volume)
    {
        soundMixer.SetFloat("blup", volume);
    }

    public void Back(GameObject menu)
    {
        panel.SetActive(false);
        menu.SetActive(true);
    }

    public void BacktoGame()
    {
        panel.SetActive(false);
    }
    public void Reset()
    {
        //LevelSelect.delete = true;
    }
}
