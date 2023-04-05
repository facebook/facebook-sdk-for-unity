using UnityEngine;

public class InputSystemWarning : MonoBehaviour
{
    void Awake()
    {
        #if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        Debug.LogWarning("You are using the new Input System, but this example uses Unity Engine OLD input system. Please, set your input configuration to Old or Both.");
        #endif
    }
}
