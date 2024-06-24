using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour
{
    [HideInInspector]
    public Vector2 worldMousePosition;

    [HideInInspector]
    public float cameraHeight;

    [HideInInspector]
    public Vector2 pixelCenterOfScreen;
    [HideInInspector]
    public Vector2 worldCenterOfScreen;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        cameraHeight = 2f * cam.orthographicSize;

        pixelCenterOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        worldCenterOfScreen = pixelToWorldPosition(pixelCenterOfScreen);
    }

    public Vector3 pixelToWorldPosition(Vector3 position)
    {
        return ((position) / Screen.height) * cameraHeight;
    }

    public float pixelToWorldDistance(float distance)
    {
        return ((distance) / Screen.height) * cameraHeight;
    }
}
