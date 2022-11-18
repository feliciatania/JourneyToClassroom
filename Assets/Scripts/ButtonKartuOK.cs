using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonKartuOK : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onButtonClick (bool b)
    {
        if (b)
            GameInstance.onKartuOK?.Invoke(true);
        else
            GameInstance.onKartuOK?.Invoke(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
