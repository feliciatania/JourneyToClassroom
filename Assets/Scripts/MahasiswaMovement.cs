using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KotakUlarTangga;
using static DosenMovement;
using Unity.Mathematics;

public class MahasiswaMovement : MonoBehaviour
{
    public int curPosMhs;
    public GameObject go_mhs;
    //public Image MahasiswaImg;
    //public Sprite[] MahasiswaSprites;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    float xPos = 0.35f;
    float yPos = 0;
    int curPosDos;
    float x;
    float y;
    public GameObject go_dosen;

    private float tParam;
    private Vector2 MahasiswaPosition;
    private float speedModifier;

    void Awake()
    {
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameStart += onStart;

        GameInstance.onMahasiswaMove += onMahasiswaMove;
        GameInstance.onMahasiswaMoveOnKartu += onMahasiswaMoveOnKartu;
        GameInstance.onGameOver += onGameOver;
        GameInstance.onGameStart += onStart;
        GameInstance.onJawabanSalah += onPanik;
        GameInstance.onTimeout += onPanik;
        GameInstance.onGiliranMahasiswa += onNormal;
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

    public int getCurPosMhs()
    {
        return curPosMhs;
    }
     
    public void onGameOver(bool b)
    {
        gameover = true;
    }

    public void onStart()
    {
        Debug.Log("masuk mhs");
       
        x = xPos;
        x *= Screen.width / 1024f;
        y = yPos;
        y *= Screen.height / 576f;
        curPosMhs = 0;
        go_mhs.transform.localScale = new Vector3(10, 10, 10);
        //curPosMhs = 4;
        //go_mhs.transform.localScale = new Vector3(0.85f, 0.85f, 1);
        //curPosMhs = 8;
        //go_mhs.transform.localScale = new Vector3(0.75f, 0.75f, 1);
        go_mhs.transform.position = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
        //checkPositifOrNegatif();

    }
    void onFlat()
    {
        //MahasiswaImg.sprite = MahasiswaSprites[0];
    }
    void onNormal()
    {
        //MahasiswaImg.sprite = MahasiswaSprites[1];
    }
    void onHappy()
    {
        //MahasiswaImg.sprite = MahasiswaSprites[2];
    }
    void onPanik()
    {
        //MahasiswaImg.sprite = MahasiswaSprites[3];
    }

    

    void onMahasiswaMove(int n)
    {
        if(!gameover)
        {
            this.Wait(0.5f, () =>
            {
                StartCoroutine(JumpMahasiswa(n));
            });
        }
    }

    void onMahasiswaMoveOnKartu(int n)
    {
        this.Wait(0.5f, () =>
        {
            StartCoroutine(JumpMahasiswaKartuOnPoisitifNegatif(n));
        });

    }

    void checkPositifOrNegatif()
    {
        
        if (curPosMhs == 2 | curPosMhs == 7)
        {
            GameInstance.onKartuPositif?.Invoke(curPosMhs);
        }
        else if (curPosMhs == 4 | curPosMhs == 9)
        {
           GameInstance.onKartuNegatif?.Invoke(curPosMhs);
        }
        else
        {
            
            curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
            if (curPosDos == curPosMhs)
            {
                //MahasiswaImg.sprite = MahasiswaSprites[3];
                GameInstance.onDosenMarah?.Invoke(true);
                this.Wait(1.5f, () =>
                {
                    //MahasiswaImg.sprite = MahasiswaSprites[0];
                    GameInstance.onDosenMarah?.Invoke(false);
                    this.Wait(0.5f, () =>
                    {
                        GameInstance.onGiliranDosen?.Invoke();
                    });
                });
            }
            else
            {
                this.Wait(0.25f, () =>
                {
                    //MahasiswaImg.sprite = MahasiswaSprites[0];
                    GameInstance.onGiliranDosen?.Invoke();
                });
            }
        }
    }

    IEnumerator JumpMahasiswa(int n)
    {
        //MahasiswaImg.sprite = MahasiswaSprites[2];
        go_mhs.GetComponent<Animator>().Play("MC_IdleHappy");
        for (int i = 1; i <= n; i++)
        {
            curPosMhs++;
           
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }


            RectTransform kotakRT;
            RectTransform MhsRT = go_mhs.GetComponent<RectTransform>();
            Vector2 startPosMhs;
            Vector2 EndPosMhs;
            float percent;
            float timeFactor;
            float duration;

            //LOMPAT
            kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();
            if (curPosMhs >= 1 && curPosMhs <= 3)
            {
                x = - kotakRT.rect.width / 2;
                y = yPos;
            }
            else if (curPosMhs >= 5 && curPosMhs <= 7)
            {
                x = kotakRT.rect.width / 2;
                y = yPos - 0.1f;
            }
            else if (curPosMhs >= 9)
            {
                x = - kotakRT.rect.width / 2;
                y = yPos - 0.025f;
            }
            else if (curPosMhs == 4)
            {
                x = -xPos + 0.05f;
                y = yPos - 0.1f;
            }
            else if(curPosMhs == 8)
            {
                x = xPos - 0.05f;
                y = yPos - 0.025f;
            }
            x *= Screen.width / 1024f;
            y *= Screen.height / 576f;

            Vector2 p0 = go_mhs.transform.position;
            Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * 1.5f);
            if (curPosMhs == 4 | curPosMhs == 8)
            {
                p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 1.5f);
    }
            Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

            Vector3 targetScale;
            duration = 10f;

            go_mhs.GetComponent<Animator>().Play("MC_Jump");
            while (tParam < 1)
            {
                tParam += Time.deltaTime * speedModifier;

                MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                    2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                transform.position = MahasiswaPosition;

                if (curPosMhs == 4 || curPosMhs == 8)
                {
                    if(curPosMhs == 4)
                        targetScale = new Vector3(8.5f, 8.5f, 1);
                    else
                        targetScale = new Vector3(7.5f,7.5f, 1);
                    go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                }
                

                yield return new WaitForEndOfFrame();

            }

            go_mhs.GetComponent<Animator>().Play("MC_IdleHappy");
            yield return new WaitForSeconds(0.25f);

            tParam = 0;
           

            // LOMPAT (KOTAK 3->4 & KOTAK 7->8)
            if ((curPosMhs == 3 && i < n) | (curPosMhs == 7 && i < n))
            {
                go_mhs.GetComponent<Animator>().Play("MC_Jump");
                Debug.Log("masuk if");
                curPosMhs++;
                kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();

                if (curPosMhs == 4)
                {
                    x = -xPos + 0.05f;
                    y = yPos - 0.1f;
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    x = xPos - 0.05f;
                    y = yPos - 0.025f;
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                x *= Screen.width / 1024f;
                y *= Screen.height / 576f;

                p0 = go_mhs.transform.position;
                p1 = new Vector2(go_mhs.transform.position.x - x / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 2);
                p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                
                duration = 10f;
                //speedModifier = 1f;

                while (tParam < 1)
                {
                    tParam += Time.deltaTime * speedModifier;

                    MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                        2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                    transform.position = MahasiswaPosition;

                    if (curPosMhs == 4 || curPosMhs == 8)
                    {
                        if (curPosMhs == 4)
                            targetScale = new Vector3(0.85f, 0.85f, 1);
                        else
                            targetScale = new Vector3(0.75f, 0.75f, 1);
                        go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                    }

                    yield return new WaitForEndOfFrame();

                }
                yield return new WaitForSeconds(0.375f);
                //speedModifier = 1f;
                tParam = 0;
                i++;
            }
            
            // JALAN
            else
            {
                go_mhs.GetComponent<Animator>().Play("MC_Walk");
                if (curPosMhs >= 1 && curPosMhs <= 3)
                {
                    duration = 1.35f;
                    x = xPos;
                    y = yPos;
                }
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    duration = 1.25f;
                    x = -xPos + 0.05f;
                    y = yPos - 0.1f;
                }
                else if (curPosMhs >= 8)
                {
                    duration = 1.15f;
                    x = xPos - 0.05f;
                    y = yPos - 0.025f;
                }
                x *= Screen.width / 1024f;
                y *= Screen.height / 576f;

                startPosMhs = go_mhs.transform.position;
                percent = 0;
                timeFactor = 1 / duration;

                while (percent < 1)
                {
                    percent += Time.deltaTime * timeFactor;

                    EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); //ditambah dengan posisi tile target
                    if (curPosMhs == 4 | curPosMhs == 8)
                    {
                        duration = 0.5f;
                        if (curPosMhs == 4)
                            x = -xPos + 5;
                        else
                            x = xPos - 5;
                        x *= Screen.width / 1024f;
                    }

                    go_mhs.transform.position = Vector2.Lerp(startPosMhs, EndPosMhs, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                       //time += Time.deltaTime;
                    yield return null;
                }
            }
            

            if (curPosMhs == 11)
            {
                break;
            }

        }

        go_mhs.GetComponent<Animator>().Play("MC_IdleNormal");
        if (curPosMhs == 11)
        {
            this.Wait(0.5f, () =>
            {
                GameInstance.onGameOver?.Invoke(true);
            });

        }
        else
        {
            checkPositifOrNegatif();
        }

    }

    IEnumerator JumpMahasiswaKartuOnPoisitifNegatif(int n)
    {
        int counter = math.abs(n);

        if (n > 0)
        {
            //onHappy();
            for (int i = 1; i <= counter; i++)
            {
                curPosMhs++;
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                RectTransform kotakRT;
                RectTransform MhsRT = go_mhs.GetComponent<RectTransform>();
                Vector2 startPosMhs;
                Vector2 EndPosMhs;
                float percent;
                float timeFactor;
                float duration;
                            
                //LOMPAT
                kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();
                if (curPosMhs >= 1 && curPosMhs <= 3)
                {
                    x = -kotakRT.rect.width / 2 + 25;
                    y = yPos;
                }
                if (curPosMhs >= 5 && curPosMhs <= 7)
                {
                    x = kotakRT.rect.width / 2 - 15;
                    y = yPos - 10;
                }
                else if (curPosMhs >= 9)
                {
                    x = -kotakRT.rect.width / 2 + 25;
                    y = yPos - 2.5f;
                }
                else if (curPosMhs == 4)
                {
                    x = -xPos + 5;
                    y = yPos - 10;
                }
                else if (curPosMhs == 8)
                {
                    x = xPos - 5;
                    y = yPos - 2.5f;
                }
                x *= Screen.width / 1024f;
                y *= Screen.height / 576f;

                Vector2 p0 = go_mhs.transform.position;
                Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * 1.5f);
                Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                if (curPosMhs == 4 | curPosMhs == 8)
                {
                    p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 1.5f);
                }

                Vector3 targetScale;
                duration = 10f;

                while (tParam < 1)
                {
                    tParam += Time.deltaTime * speedModifier;

                    MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                        2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                    transform.position = MahasiswaPosition;

                    if (curPosMhs == 4 || curPosMhs == 8)
                    {
                        if (curPosMhs == 4)
                            targetScale = new Vector3(0.85f, 0.85f, 1);
                        else
                            targetScale = new Vector3(0.75f, 0.75f, 1);
                        go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                    }


                    yield return new WaitForEndOfFrame();

                }

                yield return new WaitForSeconds(0.25f);

                tParam = 0;

                // LOMPAT (KOTAK 3->4 & KOTAK 7->8)
                if ((curPosMhs == 3 && i < counter) | (curPosMhs == 7 && i < counter))
                {
                    Debug.Log("masuk if KARTU POSITIF");
                    curPosMhs++;
                    Debug.Log("curposmhs : " + curPosMhs);
                    //kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();

                   if (curPosMhs == 4)
                    {
                        x = -xPos + 5;
                        y = yPos - 10;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                    }
                    else if (curPosMhs == 8)
                    {
                        x = xPos - 5;
                        y = yPos - 2.5f;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                    x *= Screen.width / 1024f;
                    y *= Screen.height / 576f;

                    p0 = go_mhs.transform.position;
                    p1 = new Vector2(go_mhs.transform.position.x - x / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 2);
                    p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

                    duration = 10f;
                    //speedModifier = 1f;

                    while (tParam < 1)
                    {
                        tParam += Time.deltaTime * speedModifier;

                        MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                            2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                        transform.position = MahasiswaPosition;

                        if (curPosMhs == 4 || curPosMhs == 8)
                        {
                            if (curPosMhs == 4)
                                targetScale = new Vector3(0.85f, 0.85f, 1);
                            else
                                targetScale = new Vector3(0.75f, 0.75f, 1);
                            go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                        }

                        yield return new WaitForEndOfFrame();

                    }
                    yield return new WaitForSeconds(0.375f);
                    //speedModifier = 1f;
                    tParam = 0;
                    i++;
                }

                // JALAN
                else
                {
                    if (curPosMhs >= 1 && curPosMhs <= 3)
                    {
                        duration = 1.35f;
                        x = xPos;
                        y = yPos;
                    }
                    if (curPosMhs >= 4 && curPosMhs <= 7)
                    {
                        duration = 1.25f;
                        x = -xPos + 5;
                        y = yPos - 10;
                    }
                    else if (curPosMhs >= 8)
                    {
                        duration = 1.15f;
                        x = xPos - 5;
                        y = yPos - 2.5f;
                    }
                    x *= Screen.width / 1024f;
                    y *= Screen.height / 576f;

                    startPosMhs = go_mhs.transform.position;
                    percent = 0;
                    timeFactor = 1 / duration;

                    while (percent < 1)
                    {
                        percent += Time.deltaTime * timeFactor;

                        EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); //ditambah dengan posisi tile target

                        go_mhs.transform.position = Vector2.Lerp(startPosMhs, EndPosMhs, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                           //time += Time.deltaTime;
                        yield return null;
                    }
                }

            }

        }
        else if (n < 0)
        {
            onPanik();
            for (int i = 1; i <= counter; i++)
            {
                curPosMhs--;

                if (curPosMhs == 6 | curPosMhs == 3)
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0f, 0);
                }
                else
                {
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }

                RectTransform kotakRT;
                RectTransform MhsRT = go_mhs.GetComponent<RectTransform>();
                Vector2 startPosMhs;
                Vector2 EndPosMhs;
                float percent;
                float timeFactor;
                float duration;

                //JALAN
                if (curPosMhs == 3 | curPosMhs == 7)
                {
                   
                }
                else
                {
                    Debug.Log("MASUK IF KARTU MASUK IF KARTU");
                    duration = 1.25f;
                    kotakRT = kotak[curPosMhs + 1].GetComponent<RectTransform>();

                    if (curPosMhs >= 1 && curPosMhs <= 2)
                    {
                        x = kotakRT.rect.width / 2 - 25;
                        y = yPos;
                    }
                    if (curPosMhs >= 4 && curPosMhs <= 6)
                    {
                        x = -kotakRT.rect.width / 2 + 15;
                        y = yPos - 10;
                    }
                    else if (curPosMhs >= 8)
                    {
                        x = kotakRT.rect.width / 2 - 25;
                        y = yPos - 2.5f;
                    }

                    x *= Screen.width / 1024f;
                    y *= Screen.height / 576f;

                    startPosMhs = go_mhs.transform.position;
                    percent = 0;
                    timeFactor = 1 / duration;

                    while (percent < 1)
                    {
                        percent += Time.deltaTime * timeFactor;

                        EndPosMhs = new Vector2(kotak[curPosMhs + 1].transform.position.x - x, kotak[curPosMhs + 1].transform.position.y + y); //ditambah dengan posisi tile target

                        go_mhs.transform.position = Vector2.Lerp(startPosMhs, EndPosMhs, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                           //time += Time.deltaTime;
                        yield return null;
                    }
                }
               

                //LOMPAT
                kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();
                if (curPosMhs >= 1 && curPosMhs <= 3)
                {
                    x = xPos;
                    y = yPos;
                }
                if (curPosMhs >= 4 && curPosMhs <= 7)
                {
                    x = - xPos + 5;
                    y = yPos - 10;
                }
                else if (curPosMhs >= 8)
                {
                    x = xPos - 5;
                    y = yPos - 2.5f;
                }
                x *= Screen.width / 1024f;
                y *= Screen.height / 576f;

                Vector2 p0 = go_mhs.transform.position;
                Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * 1.5f);
                Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                if (curPosMhs == 3 || curPosMhs == 7)
                {
                    //p1 = new Vector2(go_mhs.transform.position.x + x * 2, go_mhs.transform.position.y - y / 2);
                    //p2 = new Vector2(go_mhs.transform.position.x + x, go_mhs.transform.position.y - y);
                }

                Vector3 targetScale;
                duration = 10f;

                while (tParam < 1)
                {
                    tParam += Time.deltaTime * speedModifier;

                    MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                        2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                    transform.position = MahasiswaPosition;

                    if (curPosMhs == 3 || curPosMhs == 7)
                    {
                        if (curPosMhs == 3)
                            targetScale = new Vector3(1, 1, 1);
                        else
                            targetScale = new Vector3(0.85f, 0.85f, 1);
                        go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                    }


                    yield return new WaitForEndOfFrame();

                }

                yield return new WaitForSeconds(0.25f);

                tParam = 0;

                ////JALAN
                //if(curPosMhs == 3| curPosMhs == 7)
                //{
                //    duration = 1.25f;
                //    if (curPosMhs == 3)
                //    {
                //        x = xPos;
                //        y = yPos;
                //    }
                //    else if (curPosMhs == 7)
                //    {
                //        x = -xPos + 5;
                //        y = yPos - 10;
                //    }
                //    x *= Screen.width / 1024f;
                //    y *= Screen.height / 576f;

                //    startPosMhs = go_mhs.transform.position;
                //    percent = 0;
                //    timeFactor = 1 / duration;

                //    while (percent < 1)
                //    {
                //        percent += Time.deltaTime * timeFactor;

                //        EndPosMhs = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y); //ditambah dengan posisi tile target

                //        go_mhs.transform.position = Vector2.Lerp(startPosMhs, EndPosMhs, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                //                                                                                                           //time += Time.deltaTime;
                //        yield return null;
                //    }
                //}
                

            }
        }

        

        if (n < 0)
        {
            if (curPosMhs >= 4 && curPosMhs <= 7)
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (n == 0)
            //MahasiswaImg.sprite = MahasiswaSprites[3];

        curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
        if (curPosDos == curPosMhs)
        {
            //MahasiswaImg.sprite = MahasiswaSprites[3];
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                //MahasiswaImg.sprite = MahasiswaSprites[0];
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranDosen?.Invoke();
                });
            });
        }
        else
        {
            //MahasiswaImg.sprite = MahasiswaSprites[0];
            this.Wait(0.25f, () =>
            {
                GameInstance.onGiliranDosen?.Invoke();
            });
        }
    }
}
