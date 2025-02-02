﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Interactable_Sconce : Interactable 
{
    [SerializeField] private bool _StartOn = false;
    private Light2D _Light;
    private Animator _Animator;
    private bool _IsLit;

    private const float MAX_INTENSITY = 0.85f;
    private const float MIN_INTENSITY = 0.65f;
    private static float _FlickerFactor = 0.0f;
    private float _FlickerMax;
    private float _FlickerMin;

    protected override void Update()
    {
        base.Update();
        Flicker();
    }

    protected override void SetToDefault()
    {
        base.SetToDefault();
        _DefaultMessage = "[F] to light";
        _IsLit = _StartOn;
        if(_IsLit){
            _RewardIssued = true;
            LightSconce();
        }
        _FlickerMin = MIN_INTENSITY;
        _FlickerMax = MAX_INTENSITY;
    }

    protected override bool InputEnabled()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    protected override void Reward()
    {
        base.Reward();
        if (!CheckPlayerFuel())
        {
            UpdateMessage("Not enough fuel to light sconce.");
            return;
        }
        GameObject.Find("SconceCounter").GetComponent<SconceCounter>().AddSconce();
        RemoveVisualQue();
        LightSconce();
        TorchAudio();
    }

    private void LightSconce()
    {
        _Light = GetComponentInChildren<Light2D>();
        _Animator = GetComponent<Animator>();
        if (_Light == null) Debug.LogError("Sconce could not find Light2D component");
        if (_Animator == null) Debug.LogError("Sconce could not find Animator component");
        _Light.enabled = true;
        _IsLit = true;
        _Animator.SetTrigger("Light");
    }

    private void TorchAudio()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Interactables/Torch/Torch_Light", gameObject);
    }

    private void Flicker()
    {
        if (!_IsLit) return;
        _Light.intensity = Mathf.Lerp(_FlickerMin, _FlickerMax, _FlickerFactor);
        _FlickerFactor += Time.deltaTime;

        if (_FlickerFactor > 1.0f)
        {
            float tempMax = _FlickerMax;
            _FlickerMax = _FlickerMin;
            _FlickerMin = tempMax;
            _FlickerFactor = 0.0f;
        }
    }

    private bool CheckPlayerFuel()
    {
        var playerInventory = _Character.GetComponent<InventoryManager>();
        if (playerInventory == null) Debug.LogError("Player inventory not found.");
        if (playerInventory.GetQuantity(ItemType.Oil) >= 10)
        {
            playerInventory.RemoveFromInventory(ItemType.Oil, 5);
            GameObject.Find("Dosh_Tracker").GetComponent<TextPopupUI>().UpdateText("Used 5 Oil");
            // used 10 oil
            return true;
        }

        return false;
    }

    public void TurnOffLight(){
        _IsLit = false;
    }
}