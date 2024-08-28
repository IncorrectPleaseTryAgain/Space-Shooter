using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    float oriScrollSpeed = 1f;
    float curScrollSpeed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.Button, transform, 1.5f);
            StateManager.instance.UpdateGameState(GameStates.MainMenu);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            curScrollSpeed += 0.1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            curScrollSpeed -= 0.1f;
        }
        else
        {
            curScrollSpeed = oriScrollSpeed;
        }

        curScrollSpeed = Mathf.Clamp(curScrollSpeed, -5f, 5f);
    }

    public float GetCurScrollSpeed() { return curScrollSpeed; }



}
