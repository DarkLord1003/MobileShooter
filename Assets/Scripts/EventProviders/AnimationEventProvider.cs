using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    public void OnAddAmmo()
    {
        EventManager.TriggerEvent("AddAmmo");
    }

    public void OnPlayPullingOutClipSound()
    {
        EventManager.TriggerEvent("PlayPullingOutClipSound");
    }

    public void OnPlayInsertingClipSound()
    {
        EventManager.TriggerEvent("PlayInsertingClipSound");
    }

    public void OnPlayShutterDistortionSound()
    {
        EventManager.TriggerEvent("PlayShutterDistortionSound");
    }

    public void OnSpawnCasing()
    {
        EventManager.TriggerEvent("SpawnCasing");
    }
}
