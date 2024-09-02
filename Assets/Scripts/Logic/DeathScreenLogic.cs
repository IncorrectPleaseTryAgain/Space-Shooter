using UnityEngine;

public class DeathScreenLogic : MonoBehaviour
{
    [SerializeField] private const float SCALE_UP_RATE = 0.005f;
    private bool hasBeenActivated = false;

    private void Awake() { transform.localScale = Vector3.zero; }
    private void Update()
    {
        if (!hasBeenActivated)
        {
            if (this.isActiveAndEnabled)
            {
                hasBeenActivated = true;
            }
        }
        else
        {
            if (transform.localScale.x < 1)
            {
                transform.localScale += new Vector3(SCALE_UP_RATE, SCALE_UP_RATE, 0f); 
            }
        }
    }
}
