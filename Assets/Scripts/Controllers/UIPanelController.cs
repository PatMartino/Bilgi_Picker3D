using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private List<Transform> layers = new List<Transform>();

    #endregion

    #endregion

    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {

    }
    private void UnSubscribeEvents()
    {

    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void OnOpenPanel(UIPanelTypes type, int layerValue) 
    {
        Instantiate(Resources.Load<GameObject>($"Screens/{type}Panel"), layers[layerValue]);
    }
    private void OnClosePanel(int layerValue)
    {
        if (layers[layerValue].childCount > 0)
        {
            Destroy(layers[layerValue].GetChild(0).gameObject);
        }
    }
    private void OnCloseAllPanel(int layerValue)
    {
        for(int i = 0; i < layers.Count; i++)
        {
            if (layers[i].childCount > 0)
            {
                Destroy(layers[i].GetChild(0).gameObject);
        }
        }
        
    }
}
