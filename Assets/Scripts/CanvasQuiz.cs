using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static CanvasQuiz;

public class CanvasQuiz : MonoBehaviour
{
    public int currentIndex = -1;
    public TextMeshProUGUI TMP_Answer1;
    public TextMeshProUGUI TMP_Answer2;
    public TextMeshProUGUI TMP_Answer3;
    public TextMeshProUGUI TMP_Pertanyaan;
    public TextMeshProUGUI TMP_Timer;

    public List<QuestionAndAnswers> QnA = new List<QuestionAndAnswers>();

    private bool answered = false;
    private bool timeout = false;

    private float currentDuration = -1f;
    public float durationAnswer = 15f;
    private bool startTimer = false;
    public void StartTimer()
    {
        currentDuration = durationAnswer;
        startTimer = true;
    }

    private List<QuestionAndAnswers> tempQnA = new List<QuestionAndAnswers>();

    void Start()
    {
        //Debug.Log("start Quizzz");
        GameInstance.onQuizAnswered += Answer;
        GameInstance.onQuizStart += onQuizStart;
        GameInstance.onGameStart += onGameStart;
    }

    public void onGameStart()
    {
        for (int i = 0; i < QnA.Count; i++)
        {
            tempQnA.Add(QnA[i]);
        }
    }

    public void onQuizStart()
    {
        nextQuiz();
        StartTimer();
    }

    public void nextQuiz()
    {
        answered = false;
        timeout = false;
        currentIndex = UnityEngine.Random.Range(0, tempQnA.Count);
        
        var currentQuiz = tempQnA[currentIndex];
        Debug.Log("jawaban :" + tempQnA[currentIndex].CorrectAnswers);

        TMP_Answer1.text = currentQuiz.Answers[0];
        TMP_Answer2.text = currentQuiz.Answers[1];
        TMP_Answer3.text = currentQuiz.Answers[2];
        TMP_Pertanyaan.text = currentQuiz.Question;
    }

    public void Answer(int index)
    {
        if (timeout) return;
        answered = true;
        if (index == tempQnA[currentIndex].CorrectAnswers)
        {
            GameInstance.onJawabanBenar?.Invoke();
        }
        else
        {
            GameInstance.onJawabanSalah?.Invoke();
        }
        tempQnA.RemoveAt(currentIndex);
    }

    void Update()
    {
        if (!startTimer) return;
        if (answered) return;
        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0)
        {
            timeout = true;
            GameInstance.onTimeout?.Invoke();
            startTimer = false;
        }
        TMP_Timer.text = Convert.ToInt32(currentDuration).ToString();
    }
}
