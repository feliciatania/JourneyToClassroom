using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KartuPositif : MonoBehaviour
{
    public string[] kartuPositif = new string[3];
    public string[] kartuPositifPerintah = new string[3];
    public TextMeshProUGUI kalPosTxt;
    public TextMeshProUGUI kalPerintahPosTxt;

    int x;
    int majuOrMundur = 0;

    void Awake()
    {
        //kartuPositif[0] = "Kamu selalu hadir di kelas dan tidak pernah membolos";
        //kartuPositif[1] = "Wah...Kamu mendapat nilai 100 pada UTS dan UAS";
        //kartuPositif[2] = "Wow...Semester ini kamu rajin belajar jadi dapat IPS diatas 3.5";

        //kartuPositifPerintah[0] = "MAJU 1 KOTAK";
        //kartuPositifPerintah[1] = "MAJU 2 KOTAK";
        //kartuPositifPerintah[2] = "MAJU 3 KOTAK";

        //GameInstance.onKartuPositif += OnKartuPositif;
        GameInstance.onKartuOK += onButtonOK;
    }
    public void OnKartuPositif()
    {
        x = UnityEngine.Random.Range(0, 2);
        if (x == 0)
        {
            kalPosTxt.text = kartuPositif[0];
            kalPerintahPosTxt.text = kartuPositifPerintah[0];
            majuOrMundur = 1;
        }
        else
        {
            kalPosTxt.text = kartuPositif[2];
            kalPerintahPosTxt.text = kartuPositifPerintah[2];
            majuOrMundur = 3;
        }
        //majuOrMundur = 3;
    }

    public void onButtonOK(bool b)
    {
        if(b == true)
        {
            //Debug.Log("POSITIF : " + majuOrMundur);
            GameInstance.onMahasiswaMoveOnKartu?.Invoke(majuOrMundur);
        }
    }

    void Update()
    {
        
    }
}
