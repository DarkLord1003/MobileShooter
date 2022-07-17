using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Touch Field
    private static TouchField _instance;

    //Settings
    private Vector2 _pointerOldPosition;
    private Vector2 _touchDistance;
    private Vector2 _inputAxis;
    private int _pointerId;
    private bool _pressed;

    //Properties
    public Vector2 InputAxis => _inputAxis;
    public static TouchField Instance => _instance;

    private void Awake()
    {
        GetReference();
    }

    private void Update()
    {
        if (_pressed)
        {
            if(_pointerId >= 0 && _pointerId < Touchscreen.current.touches.Count)
            {
                _touchDistance = Touchscreen.current.touches[_pointerId].ReadValue().position - _pointerOldPosition;
                _pointerOldPosition = Touchscreen.current.touches[_pointerId].ReadValue().position;

                _inputAxis = _touchDistance;
            }
        }
        else
        {
            _touchDistance = Vector2.zero;
            _inputAxis = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        _pointerId = eventData.pointerId;
        _pointerOldPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }

    private void GetReference()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
