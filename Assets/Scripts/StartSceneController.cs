using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    
    public GameObject playerIAButton;
    public GameObject playerVsPlayerButton;
    public GameObject playButton;
    public GameObject howToPlayButton;

    bool playClicked = false;

    private void Start() {
        Application.targetFrameRate = 60;    
    }

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OnPlayClick() {
        playClicked = true;

        playButton.SetActive(false);
        howToPlayButton.SetActive(false);
        playerIAButton.SetActive(true);
        playerVsPlayerButton.SetActive(true);

    }

    public void StartGameOnePlayer() {
        GameController.nPlayers = 1;
        ChangeScene("PlayScene");
    }

    public void StartGameTwoPlayers() {
        GameController.nPlayers = 2;
        ChangeScene("PlayScene");
    }

    public void OnClickExit() {

        if (playClicked) {
            playClicked = false;

            
            playButton.SetActive(true);
            howToPlayButton.SetActive(true);
            playerIAButton.SetActive(false);
            playerVsPlayerButton.SetActive(false);
        } else {
            Application.Quit();
        }
    }
}
