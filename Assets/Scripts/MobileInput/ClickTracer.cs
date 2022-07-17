using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickTracer : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [Header("Button Settings")]
    [SerializeField] private string _nameButton = "";
    [SerializeField] private bool _isJoystick;
    [SerializeField] private float _movementLimit;
    [SerializeField] private float _movementThreashold;
    [SerializeField] private UnityEvent _action;

    //RectTransform and position
    private RectTransform _rectTransform;
    private Vector3 _origin;
    private Vector3 _positionInArea;
    private Vector3 _startPosition;
    private Vector2 _clickPosition;

    //InputAxis
    private Vector2 _inputAxis = Vector2.zero;

    //Hold and click
    private bool _holding;
    private bool _clicked;

    //Properties
    public string NameButton
    {
        get => _nameButton;
        set => _nameButton = value;
    }

    public float MovementLimit
    {
        get => _movementLimit;
        set => _movementLimit = value;
    }

    public float MovementThreashold
    {
        get => _movementThreashold;
        set=>_movementThreashold=value;
    }

    public bool IsJoystick
    {
        get => _isJoystick;
        set => _isJoystick = value;
    }

    public UnityEvent Action
    {
        get => _action;
        set => _action = value;
    }

    private void Awake()
    {
        GetReferences();
    }

    private void Start()
    {
        AddButton();
    }

    private void AddButton()
    {
        MobileControls.Instance.AddButton(this);
    }

    public bool GetClickedStatus()
    {
        return _clicked;
    }

    public bool GetHoldingStatus()
    {
        return _holding;
    }

    public Vector2 GetInputAxis()
    {
        return _inputAxis;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _holding = true;

        if (!_isJoystick)
        {
            _clicked = true;
            StartCoroutine(StopClick());
        }
        else
        {
            _clickPosition = eventData.position;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _holding = false;

        if (_isJoystick)
        {
            _rectTransform.anchoredPosition = _startPosition;
            _positionInArea = _startPosition;
            _inputAxis = Vector2.zero;
        }

    }

    public float GetDistanceStickToArea()
    {
        return Vector2.Distance(_origin,_positionInArea);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_isJoystick)
        {
            Vector3 movementVector = Vector3.ClampMagnitude((eventData.position - _clickPosition) /
                MobileControls.Instance.Canvas.scaleFactor, (_rectTransform.sizeDelta.x * _movementLimit) +
                (_rectTransform.sizeDelta.x * _movementThreashold));

            Vector3 pos = _startPosition + movementVector;
            _rectTransform.anchoredPosition = pos;

            float inputX = 0;
            float inputY = 0;

            if (Mathf.Abs(movementVector.x) > _rectTransform.sizeDelta.x * _movementThreashold)
            {
                inputX = (movementVector.x - (_rectTransform.sizeDelta.x * _movementThreashold *
                         (movementVector.x > 0 ? 1 : -1))) / (_rectTransform.sizeDelta.x * _movementLimit);
            }

            if (Mathf.Abs(movementVector.y) > _rectTransform.sizeDelta.x * _movementThreashold)
            {
                inputY = (movementVector.y - (_rectTransform.sizeDelta.x * _movementThreashold *
                         (movementVector.y > 0 ? 1 : -1))) / (_rectTransform.sizeDelta.x * _movementLimit);
            }

            _positionInArea = pos;
            _inputAxis = new Vector2(inputX, inputY);

        }

    }

    private void GetReferences()
    {
        _rectTransform = GetComponent<RectTransform>();

        _origin = _rectTransform.localPosition;
    }

    private IEnumerator StopClick()
    {
        yield return new WaitForEndOfFrame();

        _clicked = false;
    }

}
