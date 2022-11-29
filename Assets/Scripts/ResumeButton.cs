using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void onClick()
    {
        ResumeGame();
    }

    void ResumeGame()
    {
        GameInstance.onResume?.Invoke();
        Time.timeScale = 1;
    }
}
