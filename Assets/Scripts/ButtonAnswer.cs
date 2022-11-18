using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnswer : MonoBehaviour
{
    public GameObject canvasQuiz;
    public void onClick(int index)
    {
        Debug.Log("answering..");
        //canvasQuiz.GetComponent<CanvasQuiz>().Answer(index);
        GameInstance.onQuizAnswered?.Invoke(index);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
