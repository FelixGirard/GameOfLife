using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float from = 0;
    public float to = 0;
    public float duration = 0;
    public bool Animate = false;

    private float currentAnimateTime = 0;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        Animate = false;
        camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Animate)
        {
            currentAnimateTime += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(from, to, currentAnimateTime / duration);
        }
        else
            currentAnimateTime = 0;
    }
}
