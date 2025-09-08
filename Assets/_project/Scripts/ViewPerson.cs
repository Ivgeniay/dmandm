using DMM.Cameras;
using UnityEngine;

public class ViewPerson : MonoBehaviour
{
    [SerializeField] private Transform _transformView;
    [SerializeField] private SpriteRenderer[] m_View;
    [SerializeField] private SpriteRenderer _currentSprite;
    [SerializeField] private int lineLength = 3;

    void Update()
    {
        UseCase();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(MainCamera.Main.transform.position, MainCamera.Main.transform.position + (MainCamera.Main.transform.forward * lineLength));
        Gizmos.DrawLine(_transformView.position, _transformView.position + (_transformView.forward * lineLength));
    }

    private void UseCase()
    {
        var angle = GetSignedAngleBetweenCameraAndTransform();
        if (angle > -90 && angle < 90)
        {
            var relativeAngle = GetObjectViewAngleRelativeToCamera();
            SetSpriteByAngle(relativeAngle);
        }
        LookAtCamera();
    }

    private float GetSignedAngleBetweenCameraAndTransform()
    {
        Vector3 cameraForward = MainCamera.Main.transform.forward;
        Vector3 directionToTransform = (_transformView.position - MainCamera.Main.transform.position).normalized;

        float angle = Vector3.Angle(cameraForward, directionToTransform);

        Vector3 cross = Vector3.Cross(cameraForward, directionToTransform);
        if (cross.y < 0)
            angle = -angle;

        return angle;
    }

    private float GetObjectViewAngleRelativeToCamera()
    {
        Vector3 objectForward = _transformView.forward;
        Vector3 directionToCamera = (MainCamera.Main.transform.position - _transformView.position).normalized;

        float angle = Vector3.Angle(objectForward, directionToCamera);

        Vector3 cross = Vector3.Cross(objectForward, directionToCamera);
        if (cross.y < 0)
            angle = -angle;

        return angle;
    }

    private void SetSpriteByAngle(float objectViewAngle)
    {
        if (m_View == null || m_View.Length != 8)
            return;
        float normalizedAngle = objectViewAngle;
        if (normalizedAngle < 0)
            normalizedAngle += 360f;

        normalizedAngle = (normalizedAngle + 22.5f) % 360f;

        int spriteIndex = Mathf.FloorToInt(normalizedAngle / 45f);
        _currentSprite = m_View[spriteIndex];

        for (int i = 0; i < m_View.Length; i++)
        {
            m_View[i].enabled = (i == spriteIndex);
        }
    }

    private void LookAtCamera()
    {
        if (_currentSprite == null || MainCamera.Main == null)
            return;

        Vector3 directionToCamera = MainCamera.Main.transform.position - _currentSprite.transform.position;
        directionToCamera.y = 0f;

        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            //Debug.Log("Rotating object: " + _currentSprite.name);
            //Debug.Log("Before rotation: " + _currentSprite.transform.rotation.eulerAngles);
            _currentSprite.transform.rotation = targetRotation;
            //Debug.Log("After rotation: " + _currentSprite.transform.rotation.eulerAngles);
        }
    }
}
