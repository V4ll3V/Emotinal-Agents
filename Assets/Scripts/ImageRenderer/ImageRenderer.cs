using UnityEngine;
using System.Collections;
using MessageBus;
using System;
using Assets.Scripts.Emotions;
using UnityEngine.UI;
using Assets.Scripts.Agents;
using System.Collections.Generic;

public class ImageRenderer : MonoBehaviour, ISubscriber<ShowPinMessage>, ISubscriber<ShowHpBarMessage>, ISubscriber<DestroyHpBarMessage>
{
    public GameObject PinPrefab;
    public Image HpBarPrefab;
    public Camera UICamera;
    public Vector3 offset;
    public RectTransform UI;

    private NavMeshAgent _agent;
    private GameObject _pin;

    private Dictionary<Agent, Image> HpBars;

    void Start()
    {
        GlobalMessageBus.Instance.Subscribe(this);
        HpBars = new Dictionary<Agent, Image>();
    }
    public void OnEvent(ShowPinMessage evt)
    {
        _agent = evt.AgentMesh;
        if (_pin != null)
            DestroyPin();

        CreatePin(evt.Type);
    }
    public void OnEvent(ShowHpBarMessage evt)
    {
        CreateHpBar(evt.Agent);
    }
    public void OnEvent(DestroyHpBarMessage evt)
    {
        DestroyBar(evt.Agent);
    }
    void LateUpdate()
    {
        if (_pin != null && _agent != null)
            UpdatePinPosition();
        UpdateHpBars();
    }

    public void UpdatePinPosition()
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

        StartCoroutine(DestroyPin());
    }

    public void CreateHpBar(Agent agent)
    {
        Image bar = Image.Instantiate(HpBarPrefab);
        bar.transform.SetParent(this.transform, false);

        float Hp = agent.HP / 10.0f;
        bar.fillAmount = Hp;
        HpBars.Add(agent, bar);
    }

    public void UpdateHpBars()
    {
        foreach (Agent agent in HpBars.Keys)
        {
            if (agent.HP >= 0)
            {
                Image hpBar = null;

                if (HpBars.TryGetValue(agent, out hpBar))
                {
                    if (hpBar != null)
                    {
                        float Hp = agent.HP / 10.0f;
                        hpBar.fillAmount = Hp;

                        NavMeshAgent navAgent = agent.AgentModule.agent;
                        if (navAgent != null)
                        {
                            Vector3 ViewportPosition = UICamera.WorldToScreenPoint(navAgent.transform.position) + offset;
                            hpBar.GetComponent<RectTransform>().position = ViewportPosition;
                        }
                    }
                }
            }
            else DestroyBar(agent);
        }
    }

    public void DestroyBar(Agent agent)
    {
        Image hpBar = null;

        if (HpBars.TryGetValue(agent, out hpBar))
            Destroy(hpBar);
    }

    public IEnumerator DestroyPin()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(_pin);
        _agent = null;
    }
}
