using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonLogic : MonoBehaviour
{
    public void PointerEnterHandler()
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.Button, transform, 1f);
    }
}
