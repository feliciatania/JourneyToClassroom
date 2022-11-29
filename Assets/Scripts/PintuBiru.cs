using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintuBiru : MonoBehaviour
{
    public GameObject go_pintuBiru;

    void Awake()
    {
        GameInstance.onGameStart += onReset;
        GameInstance.onAnimasiPintuBiru += onPintuBuka;
    }

    void onReset()
    {
        go_pintuBiru.GetComponent<Animator>().Play("PintuBiru_Tutup");
    }

    void onPintuBuka()
    {
        go_pintuBiru.GetComponent<Animator>().Play("PintuBiru_Transisi");
        this.Wait(1.10f, () =>
        {
            go_pintuBiru.GetComponent<Animator>().Play("PintuBiru_Buka");
        });
    }
}
