using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasSpawner : MonoBehaviour
{
    public GameObject GO_Cover;
    public GameObject GO_HowToPlay;
    public GameObject GO_PauseMenu;
    public GameObject GO_Papan;
    public GameObject GO_GilMahasiswa;
    public GameObject GO_KartuPositif;
    public GameObject GO_KartuNegatif;
    public GameObject GO_DosenMarah;
    public GameObject GO_Quiz;
    public GameObject GO_JawabanBenar;
    public GameObject GO_JawabanSalah;
    public GameObject GO_WaktuHabis;
    public GameObject GO_GilDosen;
    public GameObject GO_Win;
    public GameObject GO_Lose;
    public GameObject GO_ButtonPause;

    void Start()
    {
        GameInstance.onCover += onCover;
        GameInstance.onHowToPlay += onHowToPlay;
        GameInstance.onPlayGame += onPlayGame;
        GameInstance.onGameStart += onStart;
        GameInstance.onPause += onPause;
        GameInstance.onResume += onResume;
        GameInstance.onGiliranMahasiswa += onGiliranMahasiswa;
        GameInstance.onKartuPositif += OnKartuPositif;
        GameInstance.onKartuNegatif += OnKartuNegatif;
        GameInstance.onDosenMarah += OnDosenMarah;
        GameInstance.onQuizStart += onQuizStart;
        GameInstance.onJawabanBenar += onJawabanBenar;
        GameInstance.onJawabanSalah += onJawabanSalah;
        GameInstance.onLemparDadu += onLemparDadu;
        GameInstance.onTimeout += onTimeout;
        GameInstance.onGiliranDosen += onGiliranDosen;
        GameInstance.onGameOver += onGameOver;
        GameInstance.onMahasiswaMoveOnKartu += onMahasiswaMoveOnKartu;
        GameInstance.onReplay += onReplay;

    }

    private void onReplay()
    {
        GO_Win.SetActive(false);
        GO_Lose.SetActive(false);
        GO_Cover.SetActive(true);
        GameInstance.onCoverGame?.Invoke();
    }
    private void onCover()
    {
        GO_Cover.SetActive(true);

        GO_DosenMarah.SetActive(false);
        GO_GilDosen.SetActive(false);
        GO_GilMahasiswa.SetActive(false);
        GO_HowToPlay.SetActive(false);
        GO_JawabanBenar.SetActive(false);
        GO_JawabanSalah.SetActive(false);
        GO_KartuNegatif.SetActive(false);
        GO_KartuPositif.SetActive(false);
        GO_Lose.SetActive(false);
        GO_Papan.SetActive(false);
        GO_Quiz.SetActive(false);
        GO_WaktuHabis.SetActive(false);
        GO_Win.SetActive(false);
        GO_PauseMenu.SetActive(false);
    }
    private void onHowToPlay()
    {
        GO_Cover.SetActive(false);
        GO_HowToPlay.SetActive(true);
    }
    private void onPlayGame()
    {
        GO_Cover.SetActive(false);
        GO_HowToPlay.SetActive(true);
        GO_Papan.SetActive(true);
    }
    private void onStart()
    {
        GO_HowToPlay.SetActive(false);
        GO_Papan.SetActive(true);
        this.Wait(4.5f, () =>
        {
            GameInstance.onGiliranMahasiswa?.Invoke();
        });
    }
    private void onPause()
    {
        GO_PauseMenu.SetActive(true);
        GO_ButtonPause.SetActive(false);
    }

    private void onResume()
    {
        GO_PauseMenu.SetActive(false);
        GO_ButtonPause.SetActive(true);
    }

    private void onGiliranMahasiswa()
    {
        GO_GilMahasiswa.SetActive(true);
        this.Wait(1.5f, () =>
        {
            GO_GilMahasiswa.SetActive(false);
            GameInstance.onQuizStart?.Invoke();
        });
    }
    private void onGiliranDosen()
    {
        GO_GilDosen.SetActive(true);
        this.Wait(1.5f, () =>
        {
            GO_GilDosen.SetActive(false);
            GameInstance.onDosenMove?.Invoke();
            GO_ButtonPause.SetActive(true);
        });
    }
    private void OnKartuPositif(int x)
    {
        GO_KartuPositif.SetActive(true);
        GameObject.Find("Kartu Positif").GetComponent<KartuPositif>().OnKartuPositif(x);
    }
    private void OnKartuNegatif(int x)
    {
        GO_KartuNegatif.SetActive(true);
        GameObject.Find("Kartu Negatif").GetComponent<KartuNegatif>().onKartuNegatif(x);
    }
    private void OnDosenMarah(bool marah)
    {
        if(marah)
            GO_DosenMarah.SetActive(true);
        else
            GO_DosenMarah.SetActive(false);
    }
    private void onQuizStart()
    {
        GO_Quiz.SetActive(true);
        GO_ButtonPause.SetActive(false);
    }
    private void onJawabanBenar()
    {
        GO_Quiz.SetActive(false);
        GO_JawabanBenar.SetActive(true);
    }
    private void onJawabanSalah()
    {
        GO_Quiz.SetActive(false);
        GO_JawabanSalah.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_JawabanSalah.SetActive(false);
            GameInstance.onGiliranDosen?.Invoke();
        });
    }
    private void onTimeout()
    {
        GO_Quiz.SetActive(false);
        GO_WaktuHabis.SetActive(true);
        this.Wait(2f, () =>
        {
            GO_WaktuHabis.SetActive(false);
            GameInstance.onGiliranDosen?.Invoke();
        });
    }
    private void onLemparDadu()
    {
        GO_JawabanBenar.SetActive(false);
        GO_ButtonPause.SetActive(true);
    }
    public void onMahasiswaMoveOnKartu(int x)
    {
        GO_KartuNegatif.SetActive(false);
        GO_KartuPositif.SetActive(false);
    }
    private void onGameOver(bool win)
    {
        if (win)
            GO_Win.SetActive(true);
        else
            GO_Lose.SetActive(true);
    }
    void Update()
    {

    }
}
