using UnityEngine;

public class CameraConstantWidth : MonoBehaviour
{
  [SerializeField]  private Vector2 _defaultResolution = new Vector2(720, 1280);
    [SerializeField,Range(0f, 1f)] private float _widthOrHeight = 0;

    private Camera _componentCamera;

    private float _initialSize;
    private float _targetAspect;

    private float _initialFov;
    private float _horizontalFov = 120f;
    public static CameraConstantWidth instance;
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Singleton();
        _componentCamera = GetComponent<Camera>();
        _initialSize = _componentCamera.orthographicSize;

        _targetAspect = _defaultResolution.x / _defaultResolution.y;

        _initialFov = _componentCamera.fieldOfView;
        _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);


        if (_componentCamera.orthographic)
        {
            float constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
            _componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _widthOrHeight);
        }
        else
        {
            float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.aspect);
            _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, _widthOrHeight);
        }
    }
    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }
    public float GetWidthOrHeight()
    {
        return _widthOrHeight;
    }
    public float GetTargetAspect()
    {
        return _targetAspect;
    }
}