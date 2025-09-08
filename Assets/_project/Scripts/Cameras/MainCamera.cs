using UnityEngine;

namespace DMM.Cameras
{
    [RequireComponent(typeof(Camera))]
    internal class MainCamera : MonoBehaviour
    {
        public static Camera Main { 
            get {

                if (_camera == null)
                {
                    _camera = FindFirstObjectByType<MainCamera>().GetComponent<Camera>();
                    if (_camera == null)
                    {
                        Debug.LogError("No Camera found in the scene. Please ensure there is a Camera component in the scene.");
                    }
                }
                return _camera;
            }
            private set => _camera = value; 
        }
        private static Camera _camera;

        private void Awake()
        {
            if (Main != null && Main != _camera)
            {
                Debug.LogWarning("Multiple MainCamera instances detected. Destroying duplicate.");
                Destroy(this);
                return;
            }
            _camera = GetComponent<Camera>();
        }

    }
}
