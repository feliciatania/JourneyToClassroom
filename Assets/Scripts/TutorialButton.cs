using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void onSkip()
    {
        GameInstance.onGameStart?.Invoke();
    }

    public void onBack()
    {
        GameInstance.onCover?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
