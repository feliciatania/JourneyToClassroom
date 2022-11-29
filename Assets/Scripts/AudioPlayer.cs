using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource bgm_cover;
    public AudioSource bgm_gameplay;
    public AudioSource sfx_click;
    public AudioSource sfx_hover;
    public AudioSource sfx_right;
    public AudioSource sfx_wrong;
    public AudioSource sfx_jump;
    public AudioSource sfx_land;
    public AudioSource sfx_win;
    public AudioSource sfx_lose;
    public AudioSource sfx_UIWindowOpen;
    public AudioSource sfx_SomethingCleared;
    public AudioSource sfx_diceRoll;
    public AudioSource sfx_diceDrop;

    void Awake()
    {
        GameInstance.onCoverGame += onBgmCover;
        GameInstance.onGameStart += onBgmGameplay;
        GameInstance.onHowToPlay += onSfxUIWindowOpen;
        GameInstance.onPlayGame += onSfxUIWindowOpen;
        GameInstance.onGiliranMahasiswa += onSfxUIWindowOpen;
        GameInstance.onGiliranDosen += onSfxUIWindowOpen;
        GameInstance.onKartuNegatif += onSfxUIWindowOpen;
        GameInstance.onKartuPositif += onSfxUIWindowOpen;
        GameInstance.onJawabanBenar += onSfxRight;
        GameInstance.onJawabanSalah += onSfxWrong;
        GameInstance.onJump += onJump;
        GameInstance.onLand += onLand;
        GameInstance.onDiceRoll += onDiceRoll;
        GameInstance.onGameOver += onSfxEnding;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onBgmCover()
    {
        bgm_cover.Play();
        bgm_gameplay.Pause();
    }

    void onBgmGameplay()
    {
        bgm_cover.Pause();
        bgm_gameplay.Play();
    }

    void onSfxUIWindowOpen()
    {
        sfx_UIWindowOpen.Play();
    }

    void onSfxUIWindowOpen(int n)
    {
        sfx_UIWindowOpen.Play();
    }

    void onSfxRight()
    {
        sfx_right.Play();
    }

    void onSfxWrong()
    {
        sfx_wrong.Play();
    }

    void onJump()
    {
        sfx_jump.Play();
    }

    void onLand()
    {
        sfx_land.Play();
    }

    void onDiceRoll(bool b)
    {
        if(b)
            sfx_diceRoll.Play();
        else
        {
            sfx_diceRoll.Pause();
            sfx_diceDrop.Play();
        }
    }

    void onSfxEnding(bool win)
    {
        if (win)
        {
            sfx_win.Play();
        }
        else
        {
            sfx_lose.Play();
        }
    }
}
