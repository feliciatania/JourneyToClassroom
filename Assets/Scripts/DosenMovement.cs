using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DosenMovement : MonoBehaviour
{
    int curPosDos;
    public GameObject go_dosen;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    float xPos = 0.5f;
    float yPos = -0.1f;
    float jumpHeight = -12f;
    int curPosMhs;
    float x;
    float y;
    float h;
    public GameObject go_mhs;

    private float tParam;
    private Vector2 dosenPosition;
    private float speedModifier;

    void Awake()
    {
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameStart += onStart;
        GameInstance.onDosenMove += onDosenMove;
        GameInstance.onGameOver += onGameOver;
        //go_shadow.SetActive(true);

    }
    void Start()
    {
        //LOMPAT
        tParam = 0;
        speedModifier = 1f;

    }

    void Update()
    {
        
    }

    public void onGameOver(bool b)
    {
        gameover = true;
    }

    public void onStart()
    {
        gameover = false;
        curPosDos = 0;
        go_dosen.transform.localRotation = Quaternion.Euler(0, 0f, 0);
       
        go_dosen.transform.localScale = new Vector3(9.25f, 9.25f, 1);

        go_dosen.transform.position = new Vector2(kotak[curPosDos].transform.position.x - 2.75f, kotak[curPosDos].transform.position.y + yPos);

        this.Wait(2.5f, () =>
        {
            StartCoroutine(JalanPintuMasuk());
        });
    }

    public void onDosenMove()
    {
        if(!gameover)
        {
            this.Wait(1f, () =>
            {
                StartCoroutine(WalkDosen());
            });
            
        }
    }

    public int getCurPosDosen()
    {
        return curPosDos;
    }

    IEnumerator JalanPintuMasuk()
    {
        Vector2 StartPosDosen;
        Vector2 EndPosDosen;
        float percent;
        float timeFactor;
        float duration;

        go_dosen.GetComponent<Animator>().Play("PakGery_Walk");

        duration = 1.25f;
        x = - xPos;
        y = yPos;

        StartPosDosen = go_dosen.transform.position;
        percent = 0;
        timeFactor = 1 / duration;

        while (percent < 1)
        {
            percent += Time.deltaTime * timeFactor;

            EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y); //ditambah dengan posisi tile target

            go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                               //time += Time.deltaTime;
            yield return null;
        }

        go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
    }

    IEnumerator WalkDosen()
    {
        Vector2 StartPosDosen;
        Vector2 EndPosDosen;
        float duration = 1.35f;

        StartPosDosen = go_dosen.transform.position;
        float percent = 0;
        float timeFactor = 1 / duration;

        //JALAN
        if(curPosDos != 3 && curPosDos != 7)
        {
            Debug.Log("MASUK IF MASUK IF");
            Debug.Log(curPosDos);
            if (curPosDos >= 0 && curPosDos <= 3)
            {
                x = 0.75f;
                y = yPos;
            }
            else if (curPosDos >= 4 && curPosDos <= 7)
            {
                x = -0.7f;
                y = yPos - 0.1f;
            }
            else if (curPosDos >= 8)
            {
                x = 0.45f;
                y = yPos;
            }

            go_dosen.GetComponent<Animator>().Play("PakGery_Walk");

            while (percent < 1)
            {
                percent += Time.deltaTime * timeFactor;

                EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y); //ditambah dengan posisi tile target

                go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                         //time += Time.deltaTime;
                yield return null;
            }

            go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
        }

        curPosDos++;

        //LOMPAT
        if (curPosDos >= 1 && curPosDos <= 3)
        {
            x = -xPos;
            y = yPos;
            h = jumpHeight;
        }
        if (curPosDos >= 4 && curPosDos <= 7)
        {
            x = xPos + 0.075f;
            y = yPos - 0.1f;
            h = jumpHeight / 2;
        }
        else if (curPosDos >= 8)
        {
            x = -xPos - 0.05f;
            y = yPos;
            h = jumpHeight / 1.5f;
        }

        Vector2 p0 = go_dosen.transform.position;
        Vector2 p1 = new Vector2((go_dosen.transform.position.x + kotak[curPosDos].transform.position.x) / 2, go_dosen.transform.position.y + y * h);
        Vector2 p2 = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y);
        if (curPosDos == 4 || curPosDos == 8)
        {
            p1 = new Vector2(go_dosen.transform.position.x - x / 2, (go_dosen.transform.position.y + kotak[curPosDos].transform.position.y + y) / 2);
        }

        Vector3 targetScale;
        duration = 10f;

        go_dosen.GetComponent<Animator>().Play("PakGery_Jump");
        //go_shadow.SetActive(false);
        GameInstance.onJump?.Invoke();

        while (tParam < 1)
        {

            tParam += Time.deltaTime * speedModifier;

            dosenPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

            transform.position = dosenPosition;

            if (curPosDos == 4 | curPosDos == 8)
            {
                if (curPosDos == 4)
                    targetScale = new Vector3(8, 8, 1);
                else
                    targetScale = new Vector3(7, 7, 1);
                go_dosen.transform.localScale = Vector2.Lerp(go_dosen.transform.localScale, targetScale, tParam / duration);
            }

            yield return new WaitForEndOfFrame();

        }
        tParam = 0f;
        go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
        //go_shadow.SetActive(true);
        GameInstance.onLand?.Invoke();

        if (curPosDos == 4)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(0.25f);


        if (curPosDos == 11)
        {
            GameInstance.onAnimasiPintuBiru?.Invoke();

            StartPosDosen = go_dosen.transform.position;
            x = xPos - 0.05f;
            y = yPos;
            EndPosDosen = new Vector2(x, y);

            duration = 1;
            percent = 0;
            timeFactor = 1 / duration;

            go_dosen.GetComponent<Animator>().Play("PakGery_Walk");

            while (percent < 1)
            {
                percent += Time.deltaTime * timeFactor;

                EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y); //ditambah dengan posisi tile target

                go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                         //time += Time.deltaTime;
                yield return null;
            }

            go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
        }


        curPosMhs = go_mhs.GetComponent<MahasiswaMovement>().getCurPosMhs();
        if (curPosDos == curPosMhs)
        {
            go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
            go_dosen.GetComponent<Animator>().Play("PakGery_IdleKesal");
            this.Wait(0.25f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(true);
                
                this.Wait(1.5f, () =>
                {
                    go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");
                    go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
                    GameInstance.onDosenMarah?.Invoke(false);
                    this.Wait(0.5f, () =>
                    {
                        GameInstance.onGiliranMahasiswa?.Invoke();
                    });
                });
            });
                
        }
        else
        {
            this.Wait(0.25f, () =>
            {
                Debug.Log(curPosDos);
                if (curPosDos == 11)
                {

                    GameInstance.onGameOver?.Invoke(false);
                }
                else
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                }
            });
        }
    }

}
