using System.Numerics;
using System.Threading;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    public float creditsScrollSpeed = 1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            StateManager.instance.UpdateGameState(GameStates.MainMenu);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            creditsScrollSpeed += 0.1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            creditsScrollSpeed -= 0.1f;
        }

        creditsScrollSpeed = Mathf.Clamp(creditsScrollSpeed, -5f, 5f);
    }

}
