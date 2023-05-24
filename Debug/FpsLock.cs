using UnityEngine;

namespace Game
{
    public class FpsLock : MonoBehaviour
    {
        [SerializeField] private int _targetFps;
        
        private void Awake()
        {
            Application.targetFrameRate =  _targetFps;
            QualitySettings.vSyncCount = 1;
        }
    }
}