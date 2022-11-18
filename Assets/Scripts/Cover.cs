using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public void OnHowToPlay()
    {
        GameInstance.onHowToPlay?.Invoke();
    }

    public void onPlayGame()
    {
        GameInstance.onPlayGame?.Invoke();
    }
}
