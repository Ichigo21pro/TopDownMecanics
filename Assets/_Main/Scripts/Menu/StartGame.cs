using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void GoToGame(string name)
    {
            SceneManager.LoadScene(name);
    }
}
