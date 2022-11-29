using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KotakUlarTangga;
using static DosenMovement;
using Unity.Mathematics;

public class MahasiswaMovement : MonoBehaviour
{
    int curPosMhs;
    public GameObject go_mhs;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    float xPos = 0.5f;
    float yPos = -0.1f;
    float jumpHeight = -10f;
    int curPosDos;
    float x;
    float y;
    float h;
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
        gameover = false;
        
        curPosMhs = 0;
        go_mhs.transform.localScale = new Vector3(10, 10, 1);
        //curPosMhs = 4;
        //go_mhs.transform.localScale = new Vector3(8.5f, 8.5f, 1);
        //curPosMhs = 8;
        //go_mhs.transform.localScale = new Vector3(7.5f, 7.5f, 1);

        go_mhs.transform.position = new Vector2(kotak[curPosMhs].transform.position.x - 2.75f, kotak[curPosMhs].transform.position.y + yPos);

        this.Wait(1.15f, () =>
        {
            StartCoroutine(JalanPintuMasuk());
        });
        
    }
    void onNormal()
    {
        go_mhs.GetComponent<Animator>().Play("MC_IdleNormal");
    }
    void onPanik()
    {
        go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
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
                            GameInstance.onGiliranDosen?.Invoke();
                        });
                    });
                });
                
            }
            else
            {
                this.Wait(0.25f, () =>
                {
                    go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");
                    GameInstance.onGiliranDosen?.Invoke();
                });
            }
        }
    }

    IEnumerator JalanPintuMasuk()
    {
        Vector2 startPosMhs;
        Vector2 EndPosMhs;
        float percent;
        float timeFactor;
        float duration;

        go_mhs.GetComponent<Animator>().Play("MC_Walk");

        duration = 1.5f;
        x = xPos;
        y = yPos;

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

        go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");
    }

    IEnumerator JumpMahasiswa(int n)
    {
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
            if (curPosMhs >= 1 && curPosMhs <= 3)
            {
                x = - 0.75f;
                y = yPos;
                h = jumpHeight;
            }
            else if (curPosMhs >= 5 && curPosMhs <= 7)
            {
                x = 0.7f;
                y = yPos - 0.1f;
                h = jumpHeight / 2;
            }
            else if (curPosMhs >= 9)
            {
                x = - 0.45f;
                y = yPos;
                h = jumpHeight / 1.5f;
            }
            else if (curPosMhs == 4)
            {
                x = -xPos + 0.05f;
                y = yPos - 0.1f;
                h = jumpHeight;
            }
            else if(curPosMhs == 8)
            {
                x = xPos - 0.05f;
                y = yPos;
                h = jumpHeight;
            }

            Vector2 p0 = go_mhs.transform.position;
            Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * h);
            if (curPosMhs == 4 | curPosMhs == 8)
            {
                //p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 1.5f);
    }
            Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

            Vector3 targetScale;
            duration = 10f;

            go_mhs.GetComponent<Animator>().Play("MC_Jump");
            GameInstance.onJump?.Invoke();
            //go_shadow.SetActive(false);

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

            GameInstance.onLand?.Invoke();
            go_mhs.GetComponent<Animator>().Play("MC_IdleHappy");
            //go_shadow.SetActive(true);

            yield return new WaitForSeconds(0.25f);

            tParam = 0;

            if(curPosMhs == 11)
            {
                GameInstance.onAnimasiPintuBiru?.Invoke();
            }
              

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
                    y = yPos;
                    go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                p0 = go_mhs.transform.position;
                p1 = new Vector2(go_mhs.transform.position.x - x / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 2);
                p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

                GameInstance.onJump?.Invoke();
                //go_shadow.SetActive(false);
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
                            targetScale = new Vector3(8.5f, 8.5f, 1);
                        else
                            targetScale = new Vector3(7.5f, 7.5f, 1);
                        go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                    }

                    yield return new WaitForEndOfFrame();

                }
                go_mhs.GetComponent<Animator>().Play("MC_IdleNormal");
                GameInstance.onLand?.Invoke();
                //go_shadow.SetActive(true);

                yield return new WaitForSeconds(0.375f);
                //speedModifier = 1f;
                tParam = 0;
                i++;
            }
            
            // JALAN
            else
            {
                if((curPosMhs == 4 && i <= n) | (curPosMhs == 8 && i <= n))
                {

                }
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
                        x = -xPos + 0.075f;
                        y = yPos - 0.1f;
                    }
                    else if (curPosMhs >= 8)
                    {
                        duration = 1.15f;
                        x = xPos - 0.05f;
                        y = yPos;
                    }

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

            go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");

            if (curPosMhs == 11)
            {
                break;
            }

        }

        
        if (curPosMhs == 11)
        {
            go_mhs.GetComponent<Animator>().Play("MC_IdleHappy");
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
                    x = -0.75f;
                    y = yPos;
                }
                else if (curPosMhs >= 5 && curPosMhs <= 7)
                {
                    x = 0.7f;
                    y = yPos - 0.1f;
                }
                else if (curPosMhs >= 9)
                {
                    x = -0.45f;
                    y = yPos;
                }
                else if (curPosMhs == 4)
                {
                    x = -xPos + 0.075f;
                    y = yPos - 0.1f;
                }
                else if (curPosMhs == 8)
                {
                    x = xPos - 0.05f;
                    y = yPos;
                }

                Vector2 p0 = go_mhs.transform.position;
                Vector2 p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * -10f);
                if (curPosMhs == 4 | curPosMhs == 8)
                {
                    //p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 1.5f);
                }
                Vector2 p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

                Vector3 targetScale;
                duration = 10f;

                go_mhs.GetComponent<Animator>().Play("MC_Jump");
                GameInstance.onJump?.Invoke();
                //go_shadow.SetActive(false);

                while (tParam < 1)
                {
                    tParam += Time.deltaTime * speedModifier;

                    MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                        2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                    transform.position = MahasiswaPosition;

                    if (curPosMhs == 4 || curPosMhs == 8)
                    {
                        if (curPosMhs == 4)
                            targetScale = new Vector3(8.5f, 8.5f, 1);
                        else
                            targetScale = new Vector3(7.5f, 7.5f, 1);
                        go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                    }


                    yield return new WaitForEndOfFrame();

                }

                GameInstance.onLand?.Invoke();
                go_mhs.GetComponent<Animator>().Play("MC_IdleHappy");
                //go_shadow.SetActive(true);

                yield return new WaitForSeconds(0.25f);

                tParam = 0;


                // LOMPAT (KOTAK 3->4 & KOTAK 7->8)
                if ((curPosMhs == 3 && i < n) | (curPosMhs == 7 && i < n))
                {
                    Debug.Log("masuk lompat");
                    go_mhs.GetComponent<Animator>().Play("MC_Jump");
                    Debug.Log("masuk if");
                    curPosMhs++;
                    kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();

                    if (curPosMhs == 4)
                    {
                        x = -xPos + 0.075f;
                        y = yPos - 0.1f;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                    }
                    else
                    {
                        x = xPos - 0.05f;
                        y = yPos;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                    p0 = go_mhs.transform.position;
                    p1 = new Vector2(go_mhs.transform.position.x - x / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 2);
                    p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

                    GameInstance.onJump?.Invoke();
                    go_mhs.GetComponent<Animator>().Play("MC_Jump");
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
                                targetScale = new Vector3(8.5f, 8.5f, 1);
                            else
                                targetScale = new Vector3(7.5f, 7.5f, 1);
                            go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                        }

                        yield return new WaitForEndOfFrame();

                    }
                    go_mhs.GetComponent<Animator>().Play("MC_IdleNormal");
                    GameInstance.onLand?.Invoke();
                    //go_shadow.SetActive(true);

                    yield return new WaitForSeconds(0.375f);
                    //speedModifier = 1f;
                    tParam = 0;
                    i++;
                }

                // JALAN
                else
                {
                    if (curPosMhs == 4| curPosMhs == 8)
                    {

                    }
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
                            x = -xPos + 0.075f;
                            y = yPos - 0.1f;
                        }
                        else if (curPosMhs >= 8)
                        {
                            duration = 1.15f;
                            x = xPos - 0.05f;
                            y = yPos;
                        }

                        startPosMhs = go_mhs.transform.position;
                        percent = 0;
                        timeFactor = 1 / duration;

                        go_mhs.GetComponent<Animator>().Play("MC_Walk");

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
                  
                go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");

            }

        }
        else if (n < 0)
        {
            go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
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
                Vector2 startPosMhs;
                Vector2 EndPosMhs;
                Vector3 targetScale;
                float percent;
                float timeFactor;
                float duration;

                Vector2 p0;
                Vector2 p1;
                Vector2 p2;

                if ((curPosMhs == 3 && i <= n) | (curPosMhs == 7 && i <= n))
                {
                    Debug.Log("masuk if");
                    curPosMhs++;
                    kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();

                    if (curPosMhs == 3)
                    {
                        x = xPos;
                        y = yPos;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                    }
                    else
                    {
                        x = - xPos + 0.05f;
                        y = yPos - 0.1f;
                        go_mhs.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                    p0 = go_mhs.transform.position;
                    p1 = new Vector2(go_mhs.transform.position.x - x / 2, (go_mhs.transform.position.y + kotak[curPosMhs].transform.position.y + y) / 2);
                    p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);

                    GameInstance.onJump?.Invoke();
                    go_mhs.GetComponent<Animator>().Play("MC_JumpPanic");
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
                                targetScale = new Vector3(8.5f, 8.5f, 1);
                            else
                                targetScale = new Vector3(7.5f, 7.5f, 1);
                            go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                        }

                        yield return new WaitForEndOfFrame();

                    }
                    go_mhs.GetComponent<Animator>().Play("MC_IdleNormal");
                    GameInstance.onLand?.Invoke();
                    //go_shadow.SetActive(true);

                    yield return new WaitForSeconds(0.375f);
                    //speedModifier = 1f;
                    tParam = 0;
                    i++;
                }
                else
                {
                    //JALAN
                    if (curPosMhs == 3 | curPosMhs == 7)
                    {

                    }
                    else
                    {
                        duration = 1.25f;

                        if (curPosMhs >= 1 && curPosMhs <= 3)
                        {
                            x = 0.75f;
                            y = yPos;
                        }
                        else if (curPosMhs >= 5 && curPosMhs <= 7)
                        {
                            x = -0.7f;
                            y = yPos - 0.1f;
                        }
                        else if (curPosMhs >= 8)
                        {
                            x = 0.45f;
                            y = yPos;
                        }

                        startPosMhs = go_mhs.transform.position;
                        percent = 0;
                        timeFactor = 1 / duration;

                        go_mhs.GetComponent<Animator>().Play("MC_WalkPanic");
                        while (percent < 1)
                        {
                            percent += Time.deltaTime * timeFactor;

                            EndPosMhs = new Vector2(kotak[curPosMhs + 1].transform.position.x - x, kotak[curPosMhs + 1].transform.position.y + y); //ditambah dengan posisi tile target

                            go_mhs.transform.position = Vector2.Lerp(startPosMhs, EndPosMhs, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                               //time += Time.deltaTime;
                            yield return null;
                        }
                        go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
                    }


                    //LOMPAT
                    kotakRT = kotak[curPosMhs].GetComponent<RectTransform>();
                    if (curPosMhs >= 1 && curPosMhs <= 3)
                    {
                        x = xPos;
                        y = yPos;
                    }
                    else if (curPosMhs >= 5 && curPosMhs <= 7)
                    {
                        x = -xPos + 0.075f;
                        y = yPos - 0.1f;
                    }
                    else if (curPosMhs >= 9)
                    {
                        x = xPos - 0.05f;
                        y = yPos;
                    }
                    else if (curPosMhs == 4)
                    {
                        x = -xPos + 0.075f;
                        y = yPos - 0.1f;
                    }
                    else if (curPosMhs == 8)
                    {
                        x = xPos - 0.05f;
                        y = yPos;
                    }

                    p0 = go_mhs.transform.position;
                    p1 = new Vector2((go_mhs.transform.position.x + kotak[curPosMhs].transform.position.x + x) / 2, go_mhs.transform.position.y + y * -10f);
                    p2 = new Vector2(kotak[curPosMhs].transform.position.x + x, kotak[curPosMhs].transform.position.y + y);
                    if (curPosMhs == 3 || curPosMhs == 7)
                    {
                        //p1 = new Vector2(go_mhs.transform.position.x + x * 2, go_mhs.transform.position.y - y / 2);
                        //p2 = new Vector2(go_mhs.transform.position.x + x, go_mhs.transform.position.y - y);
                    }

                    duration = 10f;

                    go_mhs.GetComponent<Animator>().Play("MC_JumpPanic");
                    GameInstance.onJump?.Invoke();
                   
                    while (tParam < 1)
                    {
                        tParam += Time.deltaTime * speedModifier;

                        MahasiswaPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                            2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

                        transform.position = MahasiswaPosition;

                        if (curPosMhs == 3 || curPosMhs == 7)
                        {
                            if (curPosMhs == 3)
                                targetScale = new Vector3(10, 10, 1);
                            else
                                targetScale = new Vector3(8.5f, 8.5f, 1);
                            go_mhs.transform.localScale = Vector2.Lerp(go_mhs.transform.localScale, targetScale, tParam / duration);
                        }


                        yield return new WaitForEndOfFrame();

                    }
                    go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
                    GameInstance.onLand?.Invoke();

                    yield return new WaitForSeconds(0.25f);

                    tParam = 0;
                }                  

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
            go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");

        curPosDos = go_dosen.GetComponent<DosenMovement>().getCurPosDosen();
        if (curPosDos == curPosMhs)
        {
            this.Wait(0.25f, () =>
            {
                go_mhs.GetComponent<Animator>().Play("MC_IdlePanic");
                go_dosen.GetComponent<Animator>().Play("PakGery_IdleKesal");
                GameInstance.onDosenMarah?.Invoke(true);
                this.Wait(1.5f, () =>
                {
                    go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");
                    go_dosen.GetComponent<Animator>().Play("PakGery_IdleNormal");
                    GameInstance.onDosenMarah?.Invoke(false);
                    this.Wait(0.5f, () =>
                    {
                        GameInstance.onGiliranDosen?.Invoke();
                    });
                });
            });
               
        }
        else
        {
            go_mhs.GetComponent<Animator>().Play("MC_IdleFlat");
            this.Wait(0.25f, () =>
            {
                GameInstance.onGiliranDosen?.Invoke();
            });
        }
    }
}
