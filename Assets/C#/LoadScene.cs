using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour {
    public static bool isNewGame;
    public void Loaded(bool newGame)
    {
        SceneManager.LoadScene("Playing");
        isNewGame = newGame;
    }
}
