using UnityEngine;

public class DeathScreenLogic : MonoBehaviour
{
    bool hasBeenActivated = false;
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }


    float rate = 0.005f;
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
                transform.localScale += new Vector3(rate, rate, rate); 
            }
        }
    }
}
