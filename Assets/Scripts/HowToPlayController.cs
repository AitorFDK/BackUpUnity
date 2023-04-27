using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayController : MonoBehaviour
{

    int howToPlaySteps = 1;
    int currentStep = 0;

    

    public void Next() {
        currentStep++;
        if (currentStep >= howToPlaySteps)
            Exit();
    }

    public void Exit() {
        SceneManager.LoadScene("StartScene");
    }

}
