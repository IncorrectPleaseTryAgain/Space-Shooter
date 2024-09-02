using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField] private AudioClip pointerEnterSFX;
    [SerializeField] private GameStates onClickState;
    public void PlayPointerEnterSFX() { AudioManager.instance.PlaySoundQueue(pointerEnterSFX); }
    public void OnClickSetState() { StateManager.instance.UpdateGameState(onClickState); }
}
