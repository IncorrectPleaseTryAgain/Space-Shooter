using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CreditsLogic : MonoBehaviour
{
    [SerializeField]
    CreditsManager manager;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(rectTransform.localPosition.y <= 150)
        {
            rectTransform.localPosition += Vector3.up * (Time.deltaTime * manager.creditsScrollSpeed);
        }
    }
}
