using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedScroll;
    [SerializeField]
    private float _sensivity;

    private PlayerInput _input;
    private InputAction _moveAction;
    [SerializeField]
    private float _angleX;
    [SerializeField]
    private float _angleY;

    private void Start()
    {
        _input = new();
        _input.Player.Enable();
        _input.Player.Look.performed += Look_performed;


#if UNITY_EDITOR
        _moveAction = _input.Player.MoveEdit;
#elif UNITY_STANDALONE_WIN
        _moveAction = _input.Player.Move;
#endif
        _input.Player.Zoom.performed += Zoom_performed;
        _input.Player.EscButton.performed += EscButton_performed;
    }

    private void EscButton_performed(InputAction.CallbackContext obj)
    {
        LogManager.Instance?.SaveLog("Esc KeyDown");
        UIManager.Instance.ExitUIMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void Zoom_performed(InputAction.CallbackContext obj)
    {
        transform.localPosition += transform.TransformVector(new Vector3(0,0,obj.ReadValue<float>())* _speedScroll * Time.deltaTime);
    }

    private void Update()
    {
        Move(_moveAction);
    }
    private void Move(InputAction obj)
    {
        var move = obj.ReadValue<Vector2>();
        transform.localPosition += transform.TransformVector(new Vector3(move.x, move.y) * _speed * Time.deltaTime);
    }

    private void Look_performed(InputAction.CallbackContext obj)
    {
        if (Mouse.current.rightButton.isPressed)
        {
            var input = obj.ReadValue<Vector2>();
            var look = new Vector3(-input.y, input.x);

            _angleX += look.x * _sensivity * Time.deltaTime;
            _angleX = Mathf.Clamp(_angleX, -89, 89);
            _angleY += look.y * _sensivity * Time.deltaTime;
            if (_angleY < -360)
                _angleY += 360;
            if (_angleY > 360)
                _angleY -= 360;
            look = new Vector3(_angleX, _angleY);
            transform.localRotation = Quaternion.Euler(look);
        }
    }
}
