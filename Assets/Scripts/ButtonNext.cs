using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNext : MonoBehaviour
{

    public void onButtonClick()
    {
        GameInstance.onReplay?.Invoke();
    }
}
