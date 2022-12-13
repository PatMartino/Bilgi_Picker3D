using System.Collections.Generic;
using Data.UnityObjects;
using Data.ValueObjects;
using Signals;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Keys;
namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        #endregion

        #region Private Variables

         private InputData _data;

         private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity; //ref Type
        private float3 _moveVector; //ref Type
        private Vector2? _mousePosition; //ref Type

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").Data;
        }

        #region Event Subscriptions

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onPlay += OnPlay;
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onPlay -= OnPlay;
        }
        private void OnEnableInput()
        {
            _isAvailableForTouch = true;
        }
        private void OnDisableInput()
        {
            _isAvailableForTouch = false;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void Update()
        {
            if (!_isAvailableForTouch) return;

            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
            {
                _isTouching = false;

                InputSignals.Instance.onInputReleased?.Invoke();
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                if (!_isFirstTimeTouchTaken)
                {
                    _isFirstTimeTouchTaken = true;
                    InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                }

                _mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                if (_isTouching)
                {
                    if (_mousePosition != null)
                    {
                        Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;


                        if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                            _moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                        else if (mouseDeltaPos.x < -_data.HorizontalInputSpeed)
                            _moveVector.x = -_data.HorizontalInputSpeed / 10f * -mouseDeltaPos.x;
                        else
                            _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity,
                                _data.ClampSpeed);

                        _mousePosition = Input.mousePosition;

                        InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                        {
                            HorizontalInputValue = _moveVector.x,
                            HorizontalInputClampNegativeSide = _data.ClampValues.x,
                            HorizontalInputClampPositiveSide = _data.ClampValues.y
                        });
                    }
                }
            }
        }

        private void OnPlay()
        {
            _isAvailableForTouch = true;
        }


        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        private void OnReset()
        {
            _isTouching = false;
            _isAvailableForTouch = false;
        }
    }
}

