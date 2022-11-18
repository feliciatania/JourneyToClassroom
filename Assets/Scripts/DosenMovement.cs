using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DosenMovement : MonoBehaviour
{
    [SerializeField]
    public int curPosDos;
    public GameObject go_dosen;
    public Image DosenImg;
    //public Sprite[] DosenSprites;
    private GameObject[] kotak;
    KotakUlarTangga KUT;
    bool gameover;
    float xPos = 0.35f;
    float yPos = 0.5f;
    int curPosMhs;
    float x;
    float y;
    public GameObject go_mhs;

    private float tParam;
    private Vector2 dosenPosition;
    private float speedModifier;

    void Awake()
    {
        KUT = GameObject.Find("Panel Papan").GetComponent<KotakUlarTangga>();
        kotak = KUT.kotak;
        GameInstance.onGameStart += onStart;
    }
    void Start()
    {
        GameInstance.onDosenMove += onDosenMove;
        GameInstance.onGameOver += onGameOver;
        GameInstance.onGameStart += onStart;

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
        Debug.Log("masuk dosen");
        curPosDos = 0;
        go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        x = xPos * Screen.width / 1024f;
        y = yPos * Screen.height / 576f;
        //x = (-xPos + 5) * Screen.width / 1024f;
        //y = (yPos  - 10) * Screen.height / 576f;
        go_dosen.transform.position = new Vector2(kotak[curPosDos].transform.position.x - x, kotak[curPosDos].transform.position.y + y);
        go_dosen.transform.localScale = new Vector3(1, 1, 1);
        //go_dosen.transform.localScale = new Vector3(0.85f, 0.85f, 1);
    }

    public void onDosenMove()
    {
        if(!gameover)
        {
            this.Wait(1f, () =>
            {
                StartCoroutine(MoveDosen());
            });
            
        }
    }

    public int getCurPosDosen()
    {
        return curPosDos;
    }

    IEnumerator MoveDosen()
    {
        Vector2 StartPosDosen;
        Vector2 EndPosDosen;
        float time = 0;
        float duration = 1.35f;

        StartPosDosen = go_dosen.transform.position;
        float percent = 0;
        float timeFactor = 1 / duration;

        RectTransform kotakRT = kotak[curPosDos].GetComponent<RectTransform>();
        RectTransform dosenRT = go_dosen.GetComponent<RectTransform>();

        //JALAN
        if(curPosDos != 3 && curPosDos != 7)
        {
            Debug.Log("MASUK IF MASUK IF");
            Debug.Log(curPosDos);
            if (curPosDos >= 0 && curPosDos <= 2)
            {
                x = kotakRT.rect.width / 2 - 30;
                y = yPos;
            }
            else if (curPosDos >= 4 && curPosDos <= 6)
            {
                x = -kotakRT.rect.width / 2 + 17.5f;
                y = yPos - 10;
            }
            else if (curPosDos >= 8)
            {
                x = kotakRT.rect.width / 2 - 25;
                y = yPos - 2.5f;
            }

            //if (curPosDos == 3)
            //{
            //    x = -30;
            //    y = kotakRT.rect.height - 10;
            //}
            //else if (curPosDos == 7)
            //{
            //    x = 30;
            //    y = kotakRT.rect.height;
            //}
            x *= Screen.width / 1024f;
            y *= Screen.height / 576f;

            while (percent < 1)
            {
                percent += Time.deltaTime * timeFactor;

                EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y); //ditambah dengan posisi tile target
                //if (curPosDos == 3 | curPosDos == 7)
                //{
                //    duration = 1.2f;
                //    EndPosDosen = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y);
                //}

                go_dosen.transform.position = Vector2.Lerp(StartPosDosen, EndPosDosen, Mathf.SmoothStep(0, 1, percent)); //diganti jadi world position karena masalah sort rendering
                                                                                                                         //time += Time.deltaTime;
                yield return null;
            }
        }
        

        StartCoroutine(JumpDosen());
    }

    IEnumerator JumpDosen()
    {
        curPosDos++;

        if (curPosDos >= 1 && curPosDos <= 3)
        {
            x = -xPos;
            y = yPos;
        }
        if (curPosDos >= 4 && curPosDos <= 7)
        {
            x = xPos - 5;
            y = yPos - 10;
        }
        else if (curPosDos >= 8)
        {
            x = -xPos + 5;
            y = yPos - 2.5f;
        }
        x *= Screen.width / 1024f;
        y *= Screen.height / 576f;
            
        Vector2 p0 = go_dosen.transform.position;
        Vector2 p1 = new Vector2((go_dosen.transform.position.x + kotak[curPosDos].transform.position.x + x) / 2, go_dosen.transform.position.y + y * 1.5f);
        Vector2 p2 = new Vector2(kotak[curPosDos].transform.position.x + x, kotak[curPosDos].transform.position.y + y);
        if (curPosDos == 4 || curPosDos == 8)
        {
            p1 = new Vector2(go_dosen.transform.position.x - x / 2, (go_dosen.transform.position.y + kotak[curPosDos].transform.position.y + y) / 2);
        }
       
        Vector3 targetScale;
        float duration = 10f;

        while (tParam < 1)
        {

            tParam += Time.deltaTime * speedModifier;

            dosenPosition = Mathf.Pow(1 - tParam, 2) * p0 +
                2 * (1 - tParam) * tParam * p1 + Mathf.Pow(tParam, 2) * p2;

            transform.position = dosenPosition;

            if (curPosDos == 4 | curPosDos == 8)
            {
                if (curPosDos == 4)
                    targetScale = new Vector3(0.85f, 0.85f, 1);
                else
                    targetScale = new Vector3(0.75f, 0.75f, 1);
                go_dosen.transform.localScale = Vector2.Lerp(go_dosen.transform.localScale, targetScale, tParam / duration);
            }

            yield return new WaitForEndOfFrame();

        }

        tParam = 0f;

        if (curPosDos == 4)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (curPosDos == 8)
        {
            go_dosen.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }
        yield return new WaitForSeconds(0.25f);

        curPosMhs = go_mhs.GetComponent<MahasiswaMovement>().getCurPosMhs();
        if (curPosDos == curPosMhs)
        {
            GameInstance.onDosenMarah?.Invoke(true);
            this.Wait(1.5f, () =>
            {
                GameInstance.onDosenMarah?.Invoke(false);
                this.Wait(0.5f, () =>
                {
                    GameInstance.onGiliranMahasiswa?.Invoke();
                });
            });
        }
        else
        {
            this.Wait(0.25f, () =>
            {
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
