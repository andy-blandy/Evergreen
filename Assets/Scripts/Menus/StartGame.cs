using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void SG()
    {
        SceneManager.LoadScene("Dungeon_1");
    }

    public void QG()
    {
        Application.Quit();
    }
}
