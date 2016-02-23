using UnityEngine;
using System.Collections;
using MessageBus;
using System;
using Assets.Scripts.Emotions;
using UnityEngine.UI;

public class PinRenderer : MonoBehaviour, ISubscriber<ShowPinMessage>
{
    public GameObject PinPrefab;
    public Camera UICamera;
    public Vector3 offset;
    public RectTransform UI;

    private NavMeshAgent _agent;
    private GameObject _pin;

    void Start()
    {
        GlobalMessageBus.Instance.Subscribe(this);
    }
    void LateUpdate()
    {
        if (_pin != null && _agent != null)
            UpdatePosition();
    }
    public void OnEvent(ShowPinMessage evt)
    {
        _agent = evt.AgentMesh;
        if (_pin != null)
            DestroyPin();

        CreatePin(evt.Type);
    }

    public void UpdatePosition()
    {
        Vector3 ViewportPosition = UICamera.WorldToScreenPoint( _agent.transform.position) + offset;
        _pin.GetComponent<RectTransform>().position= ViewportPosition;
    }

    public void CreatePin(EmotionType type)
    {
        _pin = GameObject.Instantiate(PinPrefab);
        _pin.transform.SetParent(this.transform, false);

        string ty = type.ToString();
        PinViewModel vm = _pin.GetComponent<PinViewModel>();
        vm.EmotionIcon.sprite = Resources.Load<Sprite>("Sprites/" + type.ToString());
    }
    public void DestroyPin()
    {
        Destroy(_pin);
        _agent = null;
    }
}
