using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    private RectTransform creditsTransform;

    // Scrolling
    [SerializeField] private float START_SCROLL_SPEED = 1f;
    [SerializeField] private float SCROLL_ACCELERATION= 0.1f;
    [SerializeField] private float MAX_SCROLL_SPEED = 5f;
    [SerializeField] private float MIN_SCROLL_SPEED = -5f;
    private const float MIN_POS_CREDITS_OUT_OF_VIEW = -40;
    private const float MAX_POS_CREDITS_OUT_OF_VIEW = 170f;
    private float currentScrollSpeed;


    private void Awake()
    {
        creditsTransform = credits.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Scroll credits, then change scene once out of view
        creditsTransform.position += Vector3.up * (Time.deltaTime * currentScrollSpeed);
        creditsTransform.position = new Vector3(0f, Mathf.Clamp(creditsTransform.position.y, MIN_POS_CREDITS_OUT_OF_VIEW, MAX_POS_CREDITS_OUT_OF_VIEW), 0f);
        if(creditsTransform.position.y > MAX_POS_CREDITS_OUT_OF_VIEW) { StateManager.instance.UpdateGameState(GameStates.SceneMainMenu); }


        // Exit credits
        if (Input.GetKeyDown(KeyCode.Escape)) { StateManager.instance.UpdateGameState(GameStates.SceneMainMenu); }

        // Speed up scrolling
        if (Input.GetKey(KeyCode.UpArrow)) { currentScrollSpeed += SCROLL_ACCELERATION; }
        // Slow down scrolling
        else if (Input.GetKey(KeyCode.DownArrow)) { currentScrollSpeed -= SCROLL_ACCELERATION; }
        // Go back to original scrolling speed when no button is pressed
        else { currentScrollSpeed = START_SCROLL_SPEED; }

        // Clamp Scroll Speed
        currentScrollSpeed = Mathf.Clamp(currentScrollSpeed, MIN_SCROLL_SPEED, MAX_SCROLL_SPEED);
    }
}
