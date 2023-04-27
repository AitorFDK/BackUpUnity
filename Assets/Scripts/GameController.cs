using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int nPlayers = 1;

    public DieController[] dice;
    public PlayerController playerController;
    public OtherController otherController;
    public GameObject rollTheDiceButton;
    public GameObject otherRollTheDiceButton;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public GameObject Player1WinCanvas;
    public GameObject Player2WinCanvas;
    public List<Transform> numbers;
    public float numberLookingPlayerRot;
    public float numberLookingOtherRot;

    public enum Turn { Player, Other }
    public enum Phase { Initial, Rolling, Moving, Ending, Win, GameOver };

    [SerializeField]
    private Turn currentTurn;
    [SerializeField]
    private Phase currentPhase;

    private List<TokenController> movementsDone = new List<TokenController>();

    void Start()
    {
        currentTurn = Turn.Player;
        currentPhase = Phase.Initial;

        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);
    }


    void Update()
    {

        rollTheDiceButton.SetActive((currentPhase == Phase.Initial || currentPhase == Phase.Rolling) && currentTurn == Turn.Player);
        otherRollTheDiceButton.SetActive(nPlayers == 2 && (currentPhase == Phase.Initial || currentPhase == Phase.Rolling) && currentTurn == Turn.Other);

        SetNumbersRotation();

        switch (currentPhase)
        {

            case Phase.Initial:
                if (currentTurn == Turn.Other && nPlayers == 1)
                {
                    otherController.ActivateRoll();
                }
                break;

            case Phase.Rolling:

                bool rolling = false;
                foreach (var die in dice)
                    if (die.IsRolling())
                        rolling = true;

                if (!rolling)
                {
                    Debug.Log("End rolling.. number:");
                    foreach (var die in dice)
                        Debug.Log(die.GetCurrentFace());

                    int num1 = dice[0].GetCurrentFace();
                    int num2 = dice[1].GetCurrentFace();

                    movements.Clear();
                    movements.Add(num1);
                    movements.Add(num2);

                    currentPhase = Phase.Moving;

                    ActivateMovements();
                }

                break;

            case Phase.Ending:
                ChangeTurn();
                break;

            case Phase.Win:
                if (nPlayers == 1)
                    WinCanvas.SetActive(true);
                else
                    Player1WinCanvas.SetActive(true);
                break;

            case Phase.GameOver:
                if (nPlayers == 1)
                    LoseCanvas.SetActive(true);
                else
                    Player2WinCanvas.SetActive(true);
                break;
        }
    }

    public void OtherRoll()
    {
        if (currentTurn == Turn.Other)
        {
            if (currentPhase == Phase.Initial || currentPhase == Phase.Rolling)
            {
                foreach (var die in dice) die.Roll();
                currentPhase = Phase.Rolling;
            }
        }
    }

    public void PlayerRoll()
    {
        if (currentTurn == Turn.Player)
        {
            if (currentPhase == Phase.Initial || currentPhase == Phase.Rolling)
            {
                foreach (var die in dice) die.Roll();
                currentPhase = Phase.Rolling;
            }
        }
    }

    private List<int> movements = new List<int>();

    void MovementExecuted(int num, TokenController controller)
    {
        movementsDone.Add(controller);

        playerController.token1.Desactivate();
        playerController.token2.Desactivate();
        playerController.token3.Desactivate();
        playerController.token4.Desactivate();
        playerController.token5.Desactivate();
        playerController.token6.Desactivate();

        otherController.token1.Desactivate();
        otherController.token2.Desactivate();
        otherController.token3.Desactivate();
        otherController.token4.Desactivate();
        otherController.token5.Desactivate();
        otherController.token6.Desactivate();

        if (movements.Contains(num))
        {
            movements.Remove(num);
        }
        else if (movements.Sum() == num)
        {
            movements.Clear();
        }

        if (!CheckWinState())
            ActivateMovements();

    }

    void ActivateMovements()
    {

        int sum = movements.Sum();

        if (currentTurn == Turn.Player)
        {
            int possibleMoves = 0;

            if (movements.Contains(1))
            {
                if (!movementsDone.Contains(playerController.token1))
                    possibleMoves += playerController.token1.ActivatePositive(() => MovementExecuted(1, playerController.token1)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token1))
                    possibleMoves += otherController.token1.ActivateNegative(() => MovementExecuted(1, otherController.token1)) ? 1 : 0;
            }
            if (movements.Contains(2) || sum == 2)
            {
                if (!movementsDone.Contains(playerController.token2))
                    possibleMoves += playerController.token2.ActivatePositive(() => MovementExecuted(2, playerController.token2)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token2))
                    possibleMoves += otherController.token2.ActivateNegative(() => MovementExecuted(2, otherController.token2)) ? 1 : 0;
            }
            if (movements.Contains(3) || sum == 3)
            {
                if (!movementsDone.Contains(playerController.token3))
                    possibleMoves += playerController.token3.ActivatePositive(() => MovementExecuted(3, playerController.token3)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token3))
                    possibleMoves += otherController.token3.ActivateNegative(() => MovementExecuted(3, otherController.token3)) ? 1 : 0;
            }
            if (movements.Contains(4) || sum == 4)
            {
                if (!movementsDone.Contains(playerController.token4))
                    possibleMoves += playerController.token4.ActivatePositive(() => MovementExecuted(4, playerController.token4)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token4))
                    possibleMoves += otherController.token4.ActivateNegative(() => MovementExecuted(4, otherController.token4)) ? 1 : 0;
            }
            if (movements.Contains(5) || sum == 5)
            {
                if (!movementsDone.Contains(playerController.token5))
                    possibleMoves += playerController.token5.ActivatePositive(() => MovementExecuted(5, playerController.token5)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token5))
                    possibleMoves += otherController.token5.ActivateNegative(() => MovementExecuted(5, otherController.token5)) ? 1 : 0;
            }
            if (movements.Contains(6) || sum == 6)
            {
                if (!movementsDone.Contains(playerController.token6))
                    possibleMoves += playerController.token6.ActivatePositive(() => MovementExecuted(6, playerController.token6)) ? 1 : 0;
                if (!movementsDone.Contains(otherController.token6))
                    possibleMoves += otherController.token6.ActivateNegative(() => MovementExecuted(6, otherController.token6)) ? 1 : 0;
            }

            if (possibleMoves == 0)
                currentPhase = Phase.Ending;

        }
        else
        {

            List<TokenController> goodMoves = new List<TokenController>();
            List<TokenController> badMoves = new List<TokenController>();

            if (movements.Contains(1))
            {
                if (!movementsDone.Contains(playerController.token1) && playerController.token1.ActivateNegative(() => MovementExecuted(1, playerController.token1)))
                    goodMoves.Add(playerController.token1);
                if (!movementsDone.Contains(otherController.token1) && otherController.token1.ActivatePositive(() => MovementExecuted(1, otherController.token1)))
                {
                    if (otherController.token1.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token1);
                    else
                        goodMoves.Add(otherController.token1);
                }
            }
            if (movements.Contains(2) || sum == 2)
            {
                if (!movementsDone.Contains(playerController.token2) && playerController.token2.ActivateNegative(() => MovementExecuted(2, playerController.token2)))
                    goodMoves.Add(playerController.token2);
                if (!movementsDone.Contains(otherController.token2) && otherController.token2.ActivatePositive(() => MovementExecuted(2, otherController.token2)))
                {
                    if (otherController.token2.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token2);
                    else
                        goodMoves.Add(otherController.token2);
                }
            }
            if (movements.Contains(3) || sum == 3)
            {
                if (!movementsDone.Contains(playerController.token3) && playerController.token3.ActivateNegative(() => MovementExecuted(3, playerController.token3)))
                    goodMoves.Add(playerController.token3);
                if (!movementsDone.Contains(otherController.token3) && otherController.token3.ActivatePositive(() => MovementExecuted(3, otherController.token3)))
                {
                    if (otherController.token3.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token3);
                    else
                        goodMoves.Add(otherController.token3);
                }
            }
            if (movements.Contains(4) || sum == 4)
            {
                if (!movementsDone.Contains(playerController.token4) && playerController.token4.ActivateNegative(() => MovementExecuted(4, playerController.token4)))
                    goodMoves.Add(playerController.token4);
                if (!movementsDone.Contains(otherController.token4) && otherController.token4.ActivatePositive(() => MovementExecuted(4, otherController.token4)))
                {
                    if (otherController.token4.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token4);
                    else
                        goodMoves.Add(otherController.token4);
                }
            }
            if (movements.Contains(5) || sum == 5)
            {
                if (!movementsDone.Contains(playerController.token5) && playerController.token5.ActivateNegative(() => MovementExecuted(5, playerController.token5)))
                    goodMoves.Add(playerController.token5);
                if (!movementsDone.Contains(otherController.token5) && otherController.token5.ActivatePositive(() => MovementExecuted(5, otherController.token5)))
                {
                    if (otherController.token5.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token5);
                    else
                        goodMoves.Add(otherController.token5);
                }
            }
            if (movements.Contains(6) || sum == 6)
            {
                if (!movementsDone.Contains(playerController.token6) && playerController.token6.ActivateNegative(() => MovementExecuted(6, playerController.token6)))
                    goodMoves.Add(playerController.token6);
                if (!movementsDone.Contains(otherController.token6) && otherController.token6.ActivatePositive(() => MovementExecuted(6, otherController.token6)))
                {
                    if (otherController.token6.state == TokenController.TokenState.Safe)
                        badMoves.Add(otherController.token6);
                    else
                        goodMoves.Add(otherController.token6);
                }
            }

            if (nPlayers == 1)
            {
                StartCoroutine(DoAIMovement(.7f, goodMoves, badMoves));
            }
            else
            {
                if (goodMoves.Count == 0 && badMoves.Count == 0)
                    currentPhase = Phase.Ending;
            }

        }


    }

    IEnumerator DoAIMovement(float delay, List<TokenController> goodMoves, List<TokenController> badMoves)
    {

        yield return new WaitForSeconds(delay);

        if (goodMoves.Count > 0)
        {
            goodMoves[Random.Range(0, goodMoves.Count)]
                .GetComponentsInChildren<Clickable>().First(p => p.enabled)?.onClick.Invoke();
        }
        else if (badMoves.Count > 0)
        {
            badMoves[Random.Range(0, badMoves.Count)]
                .GetComponentsInChildren<Clickable>().First(p => p.enabled)?.onClick.Invoke();
        }
        else
        {
            currentPhase = Phase.Ending;
        }
    }

    void ChangeTurn()
    {
        movementsDone.Clear();

        if (CheckWinState()) return;

        if (currentTurn == Turn.Player)
            currentTurn = Turn.Other;
        else
            currentTurn = Turn.Player;

        //StartCoroutine(SetNumbersRotation());
        rotationTime = 0;

        currentPhase = Phase.Initial;
    }

    private float rotationTime = 3f;
    void SetNumbersRotation()
    {
        float targetRotation = nPlayers == 1 ? numberLookingPlayerRot : (currentTurn == Turn.Player ? numberLookingPlayerRot : numberLookingOtherRot);

        if (rotationTime > 1.5f)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                Vector3 v = numbers[i].rotation.eulerAngles;
                numbers[i].rotation = Quaternion.Euler(v.x, targetRotation, v.z);
            }
        }
        else
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                Vector3 v = numbers[i].rotation.eulerAngles;
                numbers[i].rotation = Quaternion.Euler(v.x, Mathf.SmoothStep(v.y, targetRotation, rotationTime / 1.5f), v.z);

            }
        }
        
        rotationTime += Time.deltaTime;
    }

    bool CheckWinState()
    {
        if (
            playerController.token1.state == TokenController.TokenState.Safe &&
            playerController.token2.state == TokenController.TokenState.Safe &&
            playerController.token3.state == TokenController.TokenState.Safe &&
            playerController.token4.state == TokenController.TokenState.Safe &&
            playerController.token5.state == TokenController.TokenState.Safe &&
            playerController.token6.state == TokenController.TokenState.Safe
        )
        {
            currentPhase = Phase.Win;
            return true;
        }

        if (
            otherController.token1.state == TokenController.TokenState.Safe &&
            otherController.token2.state == TokenController.TokenState.Safe &&
            otherController.token3.state == TokenController.TokenState.Safe &&
            otherController.token4.state == TokenController.TokenState.Safe &&
            otherController.token5.state == TokenController.TokenState.Safe &&
            otherController.token6.state == TokenController.TokenState.Safe
        )
        {
            currentPhase = Phase.GameOver;
            return true;
        }

        return false;
    }

    public void Exit()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
