using UnityEngine;
using UnityEngine.Events;
using Enums;

public class CoreGameSignals : MonoBehaviour
{
    #region Singleton

    public static CoreGameSignals Instance;

    private void Awake()
    {
        if(Instance!=null && Instance!= this)
        {
        Destroy(gameObject);
         return;
        }
        Instance = this;
        
    }


    #endregion

    public UnityAction<GameStates> OnChangeGameState = delegate { };
    public UnityAction<int> onLevelInitilialize = delegate { };
    public UnityAction onClearActiveLevel = delegate { };
    public UnityAction onLevelFailed = delegate { };
    public UnityAction onLevelSuccesful = delegate { };
    public UnityAction onNextLevel = delegate { };
    public UnityAction onRestartLevel = delegate { };
    public UnityAction onReset = delegate { };
    public UnityAction onPlay = delegate { };
}

