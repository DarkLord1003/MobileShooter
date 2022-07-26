using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventProvider : MonoBehaviour
{
    public void AddAmmo()
    {
        EventManager.TriggerEvent("AddAmmo");
    }
}
