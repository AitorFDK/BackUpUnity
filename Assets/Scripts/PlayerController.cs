using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Tokens")]
    public TokenController token1;
    public TokenController token2;
    public TokenController token3;
    public TokenController token4;
    public TokenController token5;
    public TokenController token6;

    
    private GameController gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) gameController.PlayerRoll();
    }
}
