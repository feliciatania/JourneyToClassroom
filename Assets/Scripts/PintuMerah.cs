using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintuMerah : MonoBehaviour
{
    public GameObject go_pintuMerah;
    public GameObject mask;

    void Start()
    {
        GameInstance.onPlayGame += onReset;
        GameInstance.onGameStart += onStart;
    }

    void Update()
    {

    }

    void onReset()
    {
        mask.SetActive(false);
        go_pintuMerah.GetComponent<Animator>().Play("PintuMerah_Tutup");
    }

    void onStart()
    {
        mask.SetActive(true);
        go_pintuMerah.GetComponent<Animator>().Play("PintuMerah_Transisi");
        this.Wait(1.10f, () =>
        {
            go_pintuMerah.GetComponent<Animator>().Play("PintuMerah_Buka");
        });
    }
}
