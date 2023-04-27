using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TokenController : MonoBehaviour
{
    public enum TokenState { Initial, Mid, Safe };

    public TokenState state;

    [Header("Scene References")]
    public GameObject marker;
    public Transform initialPosition;
    public Transform midPosition;
    public Transform safePosition;

    [Header("Configuration")]
    public AnimationCurve transitionElevation;
    public float transitionDuration = 1f;
    public Material activeMaterial;
    public Material notActiveMaterial;

    public UnityEvent onChangeState = new UnityEvent();

    /* --------------------------------- private vars -------------------------------- */
    private TokenState prevState;
    private Coroutine currentMovement;

    void Start()
    {
        prevState = TokenState.Initial;
        state = TokenState.Initial;
        marker.transform.localPosition = initialPosition.localPosition;

        initialPosition.GetComponent<Clickable>().onClick.AddListener(() => state = TokenState.Initial);
        midPosition.GetComponent<Clickable>().onClick.AddListener(() => state = TokenState.Mid);
        safePosition.GetComponent<Clickable>().onClick.AddListener(() => state = TokenState.Safe);
    }

    void Update()
    {
        if (prevState != state)
        {
            prevState = state;
            OnChangeState();
        }
    }

    void OnChangeState()
    {
        Vector3 finalPosition = Vector3.zero;

        switch (state)
        {
            case TokenState.Initial:
                finalPosition = initialPosition.localPosition;
                break;

            case TokenState.Mid:
                finalPosition = midPosition.localPosition;
                break;

            case TokenState.Safe:
                finalPosition = safePosition.localPosition;
                break;
        }

        onChangeState.Invoke();

        if (currentMovement != null)
            StopCoroutine(currentMovement);

        currentMovement = StartCoroutine(MoveMarker(marker.transform.localPosition, finalPosition, transitionDuration));
    }

    public bool ActivatePositive(UnityAction onChangeCallback) {

        onChangeState.RemoveAllListeners();
        
        Clickable obj = null;
        switch(state) {
            case TokenState.Initial:
            case TokenState.Safe:
                obj = midPosition.GetComponent<Clickable>();
                break;
            case TokenState.Mid:
                obj = safePosition.GetComponent<Clickable>();
                break;
        }

        if (obj == null) return false;

        obj.enabled = true;
        obj.GetComponent<Renderer>().material = activeMaterial;
        onChangeState.AddListener(onChangeCallback);

        return true;
    }

    public bool ActivateNegative(UnityAction onChangeCallback) {
        onChangeState.RemoveAllListeners();
        
        Clickable obj = null;
        switch(state) {
            case TokenState.Mid:
                obj = initialPosition.GetComponent<Clickable>();
                break;
        }

        if (obj == null) return false;

        obj.enabled = true;
        obj.GetComponent<Renderer>().material = activeMaterial;
        onChangeState.AddListener(onChangeCallback);

        return true;
    }

    public void ActivateNext() {
        switch (state) {
            case TokenState.Initial:
                midPosition.GetComponent<Clickable>().enabled = true;
                break;
            
            case TokenState.Mid:
                safePosition.GetComponent<Clickable>().enabled = true;
                break;
        }
    }

    public void ActivatePrevious() {
        switch (state) {
            case TokenState.Safe:
                midPosition.GetComponent<Clickable>().enabled = true;
                break;
            
            case TokenState.Mid:
                initialPosition.GetComponent<Clickable>().enabled = true;
                break;
        }
    }

    public void Desactivate() {
        initialPosition.GetComponent<Clickable>().enabled = false;
        initialPosition.GetComponent<Renderer>().material = notActiveMaterial;
        midPosition.GetComponent<Clickable>().enabled = false;
        midPosition.GetComponent<Renderer>().material = notActiveMaterial;
        safePosition.GetComponent<Clickable>().enabled = false;
        safePosition.GetComponent<Renderer>().material = notActiveMaterial;
    }

    IEnumerator MoveMarker(Vector3 from, Vector3 to, float time)
    {
        float currentTime = 0;
        yield return null;

        while (marker.transform.localPosition != to)
        {
            float x = Mathf.SmoothStep(from.x, to.x, currentTime / time);
            float y = from.y + transitionElevation.Evaluate(currentTime / time);
            float z = Mathf.SmoothStep(from.z, to.z, currentTime / time);

            marker.transform.localPosition = new Vector3(x,y,z);
            
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
