using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private GameStates states;

    #endregion

    #endregion
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnChangeGameState += OnChangeGameState;
    }
    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.OnChangeGameState -= OnChangeGameState;
    }
    private void OnDisable()
    {

    }
    
    private void OnChangeGameState(GameStates state)
    {
        states = state;
    }
}
