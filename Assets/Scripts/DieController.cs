using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour
{
    [Header("Configuration")]
    public float strength = 300f;
    public int maxContinuousRolls = 3;
    public float maxDeltaRoll = .5f;

    [Header("Faces")]
    public Transform face1;
    public Transform face2;
    public Transform face3;
    public Transform face4;
    public Transform face5;
    public Transform face6;

    private bool isRolling = false;
    private int nRoll = 0;
    private float deltaRoll = 0;

    void Update()
    {
        if (isRolling)
        {
            if (GetComponent<Rigidbody>().velocity == Vector3.zero && deltaRoll > 0.1f)
            {
                isRolling = false;
                deltaRoll = 0f;
                nRoll = 0;

            }

            deltaRoll += Time.deltaTime;
        }
        else
        {
            deltaRoll = 0f;
            nRoll = 0;
        }
    }

    public int GetCurrentFace() {
        float y = face1.position.y;
        int face = 1;

        if (face2.position.y > y){
            y = face2.position.y;
            face = 2;
        }

        if (face3.position.y > y){
            y = face3.position.y;
            face = 3;
        }

        if (face4.position.y > y){
            y = face4.position.y;
            face = 4;
        }

        if (face5.position.y > y){
            y = face5.position.y;
            face = 5;
        }

        if (face6.position.y > y){
            y = face6.position.y;
            face = 6;
        }

        return face;
    }
    
    public void Roll() => Roll(strength);
    public void Roll(float strength)
    {
        if (!isRolling || (nRoll < maxContinuousRolls && deltaRoll < maxDeltaRoll))
        {
            var rb = GetComponent<Rigidbody>();
            Vector3 torque = new Vector3(
                Random.Range(.7f, 1.2f) * Random.Range(0, 2) == 0 ? 1 : -1,
                Random.Range(.7f, 1.2f) * Random.Range(0, 2) == 0 ? 1 : -1,
                Random.Range(.7f, 1.2f) * Random.Range(0, 2) == 0 ? 1 : -1
            );
            rb.AddTorque(torque * 1300, ForceMode.Impulse);
            rb.AddForce(Vector2.up * strength * rb.mass);
            
            isRolling = true;
            nRoll++;
        }
    }

    public bool IsRolling() => isRolling;
}
