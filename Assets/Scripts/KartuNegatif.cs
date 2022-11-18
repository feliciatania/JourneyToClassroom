using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KartuNegatif : MonoBehaviour
{
    public string[] kartuNegatif = new string[4];
    public string[] kartuNegatifPerintah = new string[4];
    public TextMeshProUGUI kalNegTxt;
    public TextMeshProUGUI kalPerintahNegTxt;

    int x;
    int majuOrMundur = 0;

    void Start()
    {
        //kartuNegatif[0] = "Kamu tidak memperhatikan dosen pada saat kelas";
        //kartuNegatif[1] = "Waduh...Kamu ketiduran jadi tidak masuk kelas";
        //kartuNegatif[2] = "Waduh...Kamu sibuk bermain game jadi lupa ngerjain tugas";
        //kartuNegatif[3] = "Astaga...Kamu lupa belajar padahal hari ini ada tes";

        //kartuNegatifPerintah[0] = "KAMU TIDAK MENDAPAT GILIRAN LEMPAR DADU";
        //kartuNegatifPerintah[1] = "MUNDUR 1 KOTAK";
        //kartuNegatifPerintah[2] = "MUNDUR 2 KOTAK";
        //kartuNegatifPerintah[3] = "MUNDUR 3 KOTAK";

        GameInstance.onLoadKartuNegatif += onKartuNegatif;
        GameInstance.onKartuOK += onButtonOK;
    }

    public void onKartuNegatif(int curPosMhs)
    {
        Debug.Log(curPosMhs);
        if (curPosMhs == 4)
        {
            x = UnityEngine.Random.Range(0, 3);
            if (x == 0)
            {
                kalNegTxt.text = kartuNegatif[0];
                kalPerintahNegTxt.text = kartuNegatifPerintah[0];
                majuOrMundur = 0;
            }
            else if (x == 1)
            {
                kalNegTxt.text = kartuNegatif[1];
                kalPerintahNegTxt.text = kartuNegatifPerintah[1];
                majuOrMundur = -1;
            }
            else
            {
                kalNegTxt.text = kartuNegatif[3];
                kalPerintahNegTxt.text = kartuNegatifPerintah[3];
                majuOrMundur = -3;
            }
        }
        else if (curPosMhs == 9)
        {
            x = UnityEngine.Random.Range(0, 3);
            if (x == 0)
            {
                kalNegTxt.text = kartuNegatif[0];
                kalPerintahNegTxt.text = kartuNegatifPerintah[0];
                majuOrMundur = 0;
            }
            else if (x == 1)
            {
                kalNegTxt.text = kartuNegatif[1];
                kalPerintahNegTxt.text = kartuNegatifPerintah[1];
                majuOrMundur = -1;
            }
            else
            {
                kalNegTxt.text = kartuNegatif[3];
                kalPerintahNegTxt.text = kartuNegatifPerintah[3];
                majuOrMundur = -3;
            }
        }
        //majuOrMundur = -3;
    }

    public void onButtonOK(bool b)
    {
        if (b == false)
        {
            //Debug.Log("NEGATIF : " + majuOrMundur);
            GameInstance.onMahasiswaMoveOnKartu?.Invoke(majuOrMundur);
        }
    }

    void Update()
    {
        
    }
}
