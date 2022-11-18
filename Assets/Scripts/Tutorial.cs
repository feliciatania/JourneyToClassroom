using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject ButtonClose;
    public GameObject ButtonSkip;

    // Start is called before the first frame update
    void Start()
    {
        GameInstance.onHowToPlay += onTutorial;
        GameInstance.onPlayGame += onPlayGame;

        ButtonClose.SetActive(false);
        ButtonSkip.SetActive(false);
    }

    public void onTutorial()
    {
        //Debug.Log("masuk tutorial");
        ButtonClose.SetActive(true);
        ButtonSkip.SetActive(false);
    }

    public void onPlayGame()
    {
        //Debug.Log("masuk play game");
        ButtonClose.SetActive(false);
        ButtonSkip.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
