using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    //Mobile Controls
    private static MobileControls _instance;

    //Canvas
    private Canvas _canvas;

    //List of buttons
    private List<ClickTracer> _buttons = new List<ClickTracer>();

    //Properties
    public static MobileControls Instance => _instance;
    public Canvas Canvas => _canvas;

    private void Awake()
    {
        GetReferences();
    }


    public void AddButton(ClickTracer button)
    {
        _buttons.Add(button);
    }

    public bool GetButton(string name)
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            if(_buttons[i].NameButton == name)
            {
                return _buttons[i].GetHoldingStatus();
            }
        }

        return false;
    }

    public bool GetButtonDown(string name)
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            if(_buttons[i].NameButton == name)
            {
                return _buttons[i].GetClickedStatus();
            }
        }

        return false;
    }

    public Vector2 GetJoystick(string name)
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            if(_buttons[i].NameButton == name)
            {
                return _buttons[i].GetInputAxis();
            }
        }

        return Vector2.zero;
    }

    public float GetDistanceStick(string name)
    {
        for(int i = 0; i < _buttons.Count; i++)
        {
            if(_buttons[i].NameButton == name)
            {
                return _buttons[i].GetDistanceStickToArea();
            }
        }

        return 0f;
    }

    #region - GetReferences -
    private void GetReferences()
    {
        _instance = this;
        _canvas = GetComponent<Canvas>();
    }

    #endregion


}
