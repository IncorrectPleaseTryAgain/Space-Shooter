using System;
using UnityEngine;

class PositionIndicatorLogic : MonoBehaviour
{
    private GameObject followTarget;
    private Vector2 camSize;
    private Vector2 camPos;

    private void Start() { camSize = new Vector2(Camera.main.orthographicSize * 2f * Camera.main.aspect, Camera.main.orthographicSize * 2f); }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            camPos = Camera.main.transform.position;
            float camLeftEdge = camPos.x - 8.75f;
            float camRightEdge = camPos.x + 8.75f;
            float camTopEdge = camPos.y + 4.87f;
            float camBottomEdge = camPos.y - 4.87f;

            //TOP
            if (followTarget.transform.position.y > (camPos.y + camSize.y / 2))
            {
                //TOP LEFT
                if(followTarget.transform.position.x < (camPos.x - camSize.x / 2))
                {
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                    transform.position = new Vector2(camPos.x - 8.75f, camPos.y + 4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, 45);
                }
                //TOP RIGHT
                else if (followTarget.transform.position.x > (camPos.x + camSize.x / 2))
                {
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                    transform.position = new Vector2(camPos.x + 8.75f, camPos.y + 4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, -45);
                }
                else
                {
                    //Debug.Log("Top");
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    transform.position = new Vector2(x, camPos.y + 4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            // Bottom
            else if (followTarget.transform.position.y < (camPos.y - camSize.y / 2))
            {

                //BOTTOM LEFT
                if (followTarget.transform.position.x < (camPos.x - camSize.x / 2))
                {
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                    transform.position = new Vector2(camPos.x - 8.75f, camPos.y - 4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, 135);
                }
                //BOTTOM RIGHT
                else if (followTarget.transform.position.x > (camPos.x + camSize.x / 2))
                {
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                    transform.position = new Vector2(camPos.x + 8.75f, camPos.y +-4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, -135);
                }
                else
                {
                    //Debug.Log("Bottom");
                    float x = Math.Clamp(followTarget.transform.position.x, camLeftEdge, camRightEdge);
                    transform.position = new Vector2(x, camPos.y - 4.87f);
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }
            // Left
            else if (followTarget.transform.position.x < (camPos.x - camSize.x / 2))
            {
                //Debug.Log("Left");
                float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                transform.position = new Vector2(camPos.x - 8.75f, y);
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            // Right
            else if (followTarget.transform.position.x > (camPos.x + camSize.x / 2))
            {
                //Debug.Log("Right");
                float y = Math.Clamp(followTarget.transform.position.y, camBottomEdge, camTopEdge);
                transform.position = new Vector2(camPos.x + 8.75f, y);
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }

    public void SetActive(bool active)
    {
        if (active) { this.gameObject.SetActive(true); }
        else { this.gameObject.SetActive(false); }
    }

    public void SetFollowTarget(GameObject target) { followTarget = target; }
}