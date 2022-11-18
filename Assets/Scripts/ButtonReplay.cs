using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonReplay : MonoBehaviour
{
    // Start is called before the first frame update
    public void onButtonClick()
    {
        GameInstance.onReplay?.Invoke();
    }
}
