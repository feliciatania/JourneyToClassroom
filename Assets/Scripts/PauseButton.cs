using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void onClick()
    {
        PauseGame();
    }

    void PauseGame()
    {
        GameInstance.onPause?.Invoke();
        Time.timeScale = 0;
    }

    
}
