using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{

    public UnityEvent onClick = new UnityEvent();

    private void Start() {
        
    }

    private void OnMouseDown() {
        if (this.enabled) {
            onClick.Invoke();
        }
    }
}
