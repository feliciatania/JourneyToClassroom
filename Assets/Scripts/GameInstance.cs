using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CanvasQuiz;

public class GameInstance : MonoBehaviour
{
    public static Action onCover { get; set; }
    public static Action onHowToPlay { get; set; }
    public static Action onPlayGame{ get; set; }
    public static Action onGameStart { get; set; }
    public static Action onVersus { get; set; }
    public static Action onPapanUlarTangga { get; set; }
    public static Action onGiliranMahasiswa { get; set; }
    public static Action onQuizStart { get; set; }
    public static Action <int> onQuizAnswered { get; set; }
    public static Action onJawabanBenar { get; set; }
    public static Action onJawabanSalah { get; set; }
    public static Action onTimeout { get; set; }
    public static Action onLemparDadu { get; set; }
    public static Action <int> onMahasiswaMove { get; set; }
    public static Action <int> onMahasiswaMoveOnKartu { get; set; }
    public static Action <int> onKartuPositif { get; set; }
    public static Action <int> onKartuNegatif { get; set; }
    public static Action <int> onLoadKartuPositif { get; set; }
    public static Action <int> onLoadKartuNegatif { get; set; }
    public static Action <bool> onKartuOK { get; set; }
    public static Action <bool> onDosenMarah { get; set; }
    public static Action onGiliranDosen { get; set; }
    public static Action onDosenMove { get; set; }
    public static Action <bool> onGameOver { get; set; }
    public static Action onReplay { get; set; }
    public static Action onNext { get; set; }
}
