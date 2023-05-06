using System.Collections.Generic;
using UnityEngine;

public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    private SwitchState currentState;

    private void OnEnable()
    {
        EventService.Instance.LightSwitchToggleEvent.AddListener(onLightsToggled);
        EventService.Instance.LightsOffByGhostEvent.AddListener(onLightsOffByGhostEvent);
    }

    private void OnDisable()
    {
        EventService.Instance.LightSwitchToggleEvent.RemoveListener(onLightsToggled);
        EventService.Instance.LightsOffByGhostEvent.RemoveListener(onLightsOffByGhostEvent);
    }

    private void Start()
    {
        currentState = SwitchState.Off;
    }
    public void Interact()
    {
        GameService.Instance.GetInstructionView().HideInstruction();
        EventService.Instance.LightSwitchToggleEvent.InvokeEvent();
    }
    private void toggleLights()
    {
        bool lights = false;

        switch (currentState)
        {
            case SwitchState.On:
                currentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                currentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }

    private void setLights(bool lights)
    {
        if (lights)
            currentState = SwitchState.On;
        else
            currentState = SwitchState.Off;

        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }
    private void onLightsOffByGhostEvent()
    {
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
        setLights(false);
    }
    private void onLightsToggled()
    {
        toggleLights();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
    }
}
