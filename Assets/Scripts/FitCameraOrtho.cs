using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitCameraOrtho : MonoBehaviour
{

    public Collider boundingBox;

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        
        var vertical = boundingBox.bounds.size.y * cam.pixelHeight / cam.pixelWidth;
        // var horizontal = boundingBox.bounds.size.x * cam.pixelHeight / cam.pixelWidth;
        var horizontal = boundingBox.bounds.size.x;

        var size = Mathf.Max(horizontal, vertical) * .5f;

        Vector3 bs = boundingBox.bounds.size;
        var diagonal = Mathf.Sqrt((bs.x * bs.x) + (bs.y * bs.y) + (bs.z * bs.z));

        cam.orthographicSize = diagonal / 2;
    }
}
