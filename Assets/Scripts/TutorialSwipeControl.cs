using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSwipeControl : MonoBehaviour
{
    public GameObject scrollBar;
    float scroll_pos = 0;
    float[] pos;
    int posisi = 0;
    public GameObject buttonNext;
    public GameObject buttonPrev;
    public GameObject buttonHome;
    public Image[] circle;
    public Sprite[] circleSprite;
   
    void Awake()
    {
        GameInstance.onHowToPlay += onTutorial;
        GameInstance.onPlayGame += onPlayGame;
    }

    public void restartPosisi()
    {
        while(posisi > 0)
        {
            posisi -= 1;
            scroll_pos = pos[posisi];
        }
        posisi = 0;
    }

    public void onTutorial()
    {
        restartPosisi();
    }

    public void onPlayGame()
    {
        restartPosisi();
    }

    public void next()
    {
        if (posisi < pos.Length - 1)
        {
            posisi += 1;
            scroll_pos = pos[posisi];
        }
    }

    public void prev()
    {
        if (posisi > 0)
        {
            posisi -= 1;
            scroll_pos = pos[posisi];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (posisi == 0)
        {
            buttonPrev.SetActive(false);
            buttonNext.SetActive(true);
            circle[0].sprite = circleSprite[1];
            circle[1].sprite = circleSprite[0];
            circle[2].sprite = circleSprite[0];
            //buttonHome.SetActive(false);
        }
        else if (posisi == 1)
        {
            buttonNext.SetActive(true);
            buttonPrev.SetActive(true);
            circle[0].sprite = circleSprite[0];
            circle[1].sprite = circleSprite[1];
            circle[2].sprite = circleSprite[0];
            //buttonHome.SetActive(false);
        }
        else
        {
            buttonPrev.SetActive(true);
            buttonNext.SetActive(false);
            circle[0].sprite = circleSprite[0];
            circle[1].sprite = circleSprite[0];
            circle[2].sprite = circleSprite[1];
            //if(isTutorial == true)
            //{
            //    buttonHome.SetActive(true);
            //}
            //else
            //{
            //    buttonHome.SetActive(false);
            //}
        }
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if(Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.15f);
                    posisi = i;
                }
                    
            }
        }
    }
}
