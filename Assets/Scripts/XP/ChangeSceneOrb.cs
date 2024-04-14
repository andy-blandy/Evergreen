using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOrb : MonoBehaviour
{
    public string sceneName;

    public AudioSource orbSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            orbSFX.Play();
            SceneManager.LoadScene(sceneName);
            gameObject.SetActive(false);
        }
    }
}
