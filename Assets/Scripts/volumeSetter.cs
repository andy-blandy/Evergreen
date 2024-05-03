using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class volumeSetter : MonoBehaviour
{
    public Canvas canvas;
    public bool openVolume = false;
    public Slider volumeSlider;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setVolume()
    {
        if(openVolume == false)
        {
            volumeSlider.gameObject.SetActive(true);
        }
        else if (openVolume == true)
        {
            volumeSlider.gameObject.SetActive(false);
        }
        openVolume = !openVolume;
    }
    public void openCanvas()
    {
        if (openVolume == false)
        {
            canvas.gameObject.SetActive(true);
        }
        else if (openVolume == true)
        {
            canvas.gameObject.SetActive(false);
        }
        openVolume = !openVolume;
    }
    public void changeVolume()
    {
        float noiseLevel = volumeSlider.value;
        mixer.SetFloat("MyExposedParam", Mathf.Log10(noiseLevel) * 20);
        mixer.SetFloat("MyExposedParam 1", Mathf.Log10(noiseLevel) * 20);
        mixer.SetFloat("MyExposedParam 2", Mathf.Log10(noiseLevel) * 20);

    }
}
