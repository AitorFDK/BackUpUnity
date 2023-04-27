using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherController : MonoBehaviour
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
        
    }

    public void ActivateRoll() {
        StartCoroutine(AutomaticRoll());
    }

    IEnumerator AutomaticRoll() {

        yield return new WaitForSeconds(.3f);
        
        int rolls = Random.Range(1, 4);

        for (int i = 0; i < rolls; i++) {
            gameController.OtherRoll();

            yield return new WaitForSeconds(Random.Range(0, .4f));
        }
    }
}
