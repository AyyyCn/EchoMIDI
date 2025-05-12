using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN
        Screen.SetResolution(585, 1266, false);
#endif
    }
}
