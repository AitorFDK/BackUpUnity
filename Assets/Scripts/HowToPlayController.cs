using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayController : MonoBehaviour
{

    int currentStep = 0;

    public List<GameObject> steps;
    public GameObject nextButton;
    
    private void Start() {
        currentStep = -1;
        Next();
    }

    public void Next() {
        currentStep++;
        
        if (currentStep == steps.Count - 1) {
            nextButton.SetActive(false);
        } else if (currentStep >= steps.Count) {
            Exit();
        } else {
            nextButton.SetActive(true);
        }

        for (int i = 0; i < steps.Count; i++) {
            steps[i].SetActive(i == currentStep);
        }
    }

    public void Exit() {
        SceneManager.LoadScene("StartScene");
    }

}
