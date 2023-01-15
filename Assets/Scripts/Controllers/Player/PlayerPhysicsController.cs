using Controllers.Pool;
using DG.Tweening;
using Managers;
using Signals;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Data.UnityObjects;
using Data.ValueObjects;
using TMPro;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;
        private bool powerUp = false;
        private float fuel;
        private bool fuelStage = false;
        private int score;
        private int counter = 0;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StageArea"))
            {
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeStageResult(manager.StageValue);
                    if (result)
                    {
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                        InputSignals.Instance.onEnableInput?.Invoke();
                    }
                    else CoreGameSignals.Instance.onLevelFailed?.Invoke();
                });
                return;
            }

            if (other.CompareTag("Finish"))
            {
                GameObject.FindWithTag("I1").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("I2").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("I3").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(2).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(4).gameObject.SetActive(false);
                
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
                return;
            }

            if (other.CompareTag("MiniGame"))
            {
                score = GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point;
                Debug.Log("MiniGame");
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(1).gameObject.SetActive(true);
                powerUp = true;
                
                DOVirtual.DelayedCall(5, () =>
                {
                   
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                    fuel = GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(1).GetComponent<Image>().fillAmount;
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(2).gameObject.SetActive(true);
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).gameObject.SetActive(true);
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(4).gameObject.SetActive(true);
                    powerUp = false;
                    fuelStage = true;
                    GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).GetComponent<Image>().fillAmount = fuel;


                });


            }
            if (other.CompareTag("2x"))
            {
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point = score * 2+(counter*50) ;
                Debug.Log(GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point);
            }
            if (other.CompareTag("4x"))
            {
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point = score * 4 + (counter * 50);
                Debug.Log(GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point);
            }
            if (other.CompareTag("6x"))
            {
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point = score * 6 + (counter * 50);
                Debug.Log(GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point);
            }
            if (other.CompareTag("8x"))
            {
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point = score * 8 + (counter * 50);
                Debug.Log(GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point);
            }
            if (other.CompareTag("10x"))
            {
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point = score * 10 + (counter * 50);
                Debug.Log(GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point);
            }
            if (other.CompareTag("Score"))
            {
                counter++;
                GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point += 50;
                Destroy(other.gameObject);


            }
            if (other.CompareTag("Image1"))
            {
                GameObject.FindWithTag("I1").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);
                
            }
            if (other.CompareTag("Image2"))
            {
                GameObject.FindWithTag("I2").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);

            }
            if (other.CompareTag("Image3"))
            {
                GameObject.FindWithTag("I3").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);

            }
        }
        private void Update()
        {
            if (powerUp)
            {
                Power();
            }
            if (fuelStage)
            {
                FuelStage();

            }
            Score();
        }
        private void Score()
        {
            GameObject.FindWithTag("Bum").GetComponent<TextMeshProUGUI>().text = "Score= " + GameObject.FindWithTag("Bum").GetComponent<PoolScore>().point.ToString();
        }
        void Power()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(1).GetComponent<Image>().fillAmount += 0.075f;
            }
            GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(1).GetComponent<Image>().fillAmount -= 0.25f * Time.deltaTime;
        }
        void FuelStage()
        {
            
            GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).GetComponent<Image>().fillAmount -= 0.07f * Time.deltaTime;
            if (GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).GetComponent<Image>().fillAmount<=0)
            {
                GameObject.FindWithTag("I1").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("I2").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("I3").GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).GetComponent<Image>().fillAmount = 0.3f;
                fuelStage = false;
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(2).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(3).gameObject.SetActive(false);
                GameObject.FindWithTag("PowerBar").GetComponent<RectTransform>().GetChild(4).gameObject.SetActive(false);
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform1 = manager.transform;
            var position = transform1.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y - 1.2f, position.z + 1f), 1.65f);
        }

        internal void OnReset()
        {
        }
    }
}