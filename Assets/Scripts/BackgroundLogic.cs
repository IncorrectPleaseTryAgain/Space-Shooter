using UnityEngine;
using System;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    SpriteRenderer sprt;

    Vector2 startPos;
    Vector2 objSize;
    Vector2 camSize;

    private void Awake()
    {
        sprt = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPos = transform.position;
        objSize = new Vector2(sprt.bounds.size.x, sprt.bounds.size.y);
        camSize = new Vector2(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2f);
    }

    void Update()
    {
        Vector2 objPos = transform.position;
        Vector2 camPos = cam.transform.position;

        // Right of camera
        if (objPos.x > camPos.x)
        {
            // Check obj left edge > cam right edge
            // => obj is out of camera view from the right
            if((objPos.x - (objSize.x / 2f)) > (camPos.x + (camSize.x / 2f)))
            {
                // Shift obj left
                transform.position = new Vector3(objPos.x - (objSize.x * 3f), transform.position.y, transform.position.z);
                transform.Rotate(UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 4) * 90f, Space.Self);
            }
        }
        // Left of camera
        else if (objPos.x < camPos.x)
        {
            // Check obj right edge < cam left edge
            // => obj is out of camera view from the left
            if ((objPos.x + (objSize.x / 2f)) < (camPos.x - (camSize.x / 2f)))
            {
                // Shift obj right
                transform.position = new Vector3(objPos.x + (objSize.x * 3f), transform.position.y, transform.position.z);
                transform.Rotate(UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 4) * 90f, Space.Self);
            }
        }

        // Above camera
        if(objPos.y > camPos.y)
        {
            // Cehck obj bottom edge > cam top edge
            // => obj is out of camera view from the top
            if ((objPos.y - (objSize.y / 2f)) > (camPos.y + (camSize.y / 2f)))
            {
                // Shift obj down
                transform.position = new Vector3(transform.position.x, objPos.y - (objSize.y * 2f), transform.position.z);
                transform.Rotate(UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 4) * 90f, Space.Self);
            }
        }
        // Below camera
        else if (objPos.y < camPos.y)
        {
            // Cehck obj top edge < cam bottom edge
            // => obj is out of camera view from the bottom
            if ((objPos.y + (objSize.y / 2f)) < (camPos.y - (camSize.y / 2f)))
            {
                // Shift obj up
                transform.position = new Vector3(transform.position.x, objPos.y + (objSize.y * 2f), transform.position.z);
                transform.Rotate(UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 2) * 180f, UnityEngine.Random.Range(0, 4) * 90f, Space.Self);
            }
        }
    }
}
