using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum EInputType
{
    None = 0,
    Value,
    Press,
    Release,
    Tap,
    Hold,
    ExitHold,
}

public struct InteractionParam
{
    // Hold时间
    public double HoldDuration;
    // 从Hold到cancel的时间
    public double ExitHoldDuration;
}

public abstract class BaseMap
{
    protected InputActionMap InputActionMap;
    private Dictionary<Guid, bool> _holdState = new();
    private Dictionary<Guid, double> _holdStartTime = new();

    private Dictionary<Guid, Dictionary<EInputType, Action<InputAction.CallbackContext, InteractionParam>>> _actionTrigger = new();


    protected BaseMap(InputActionMap inputMap)
    {
        InputActionMap = inputMap;
    }

    public virtual void Destroy()
    {
        InputActionMap = null;
        _actionTrigger = null;
    }

    public void AddTrigger(InputAction action, EInputType inputType, Action<InputAction.CallbackContext, InteractionParam> trigger)
    {
        if (!_actionTrigger.TryGetValue(action.id, out var triggerDic))
        {
            triggerDic = new Dictionary<EInputType, Action<InputAction.CallbackContext, InteractionParam>>();
            _actionTrigger.Add(action.id, triggerDic);
        }
        triggerDic.TryAdd(inputType, null);
        triggerDic[inputType] += trigger;
    }

    public void RemoveTrigger(InputAction action, EInputType interactionType, Action<InputAction.CallbackContext, InteractionParam> trigger)
    {
        if (_actionTrigger.TryGetValue(action.id, out var triggerDic))
        {
            if (triggerDic.ContainsKey(interactionType))
            {
                triggerDic[interactionType] -= trigger;
            }
        }
    }

    public void Active()
    {
        InputActionMap?.Enable();
    }

    public void DeActive()
    {
        InputActionMap?.Disable();
        _holdState.Clear();
        _holdStartTime.Clear();
    }

    protected void RegisterAction(InputAction action)
    {
        if (action != null)
        {
            action.started += HandleInput;
            action.performed += HandleInput;
            action.canceled += HandleInput;
        }
        else
        {
            throw new NullReferenceException($"InputAction is null");
        }
    }

    protected void UnRegisterAction(InputAction action)
    {
        if (action != null)
        {
            action.started -= HandleInput;
            action.performed -= HandleInput;
            action.canceled -= HandleInput;
        }
    }

    private void HandleInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.type == InputActionType.Value)
        {
            HandleValue(ctx);
        }
        else if (ctx.action.type == InputActionType.Button)
        {
            if (ctx.interaction is PressInteraction)
            {
                HandlePress(ctx);
            }
            else if (ctx.interaction is TapInteraction)
            {
                HandleTap(ctx);
            }
            else if (ctx.interaction is HoldInteraction hold)
            {
                HandleHold(ctx, hold);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"不支持的类型");
            }
        }
    }

    private void HandleValue(InputAction.CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {
            InvokeTrigger(EInputType.Value, ctx);
        }
    }

    private void HandlePress(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            return;
        }
        if (ctx.control.IsPressed())
        {
            InvokeTrigger(EInputType.Press, ctx);
        }
        else
        {
            InvokeTrigger(EInputType.Release, ctx);
        }
    }

    private void HandleTap(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            return;
        }
        InvokeTrigger(EInputType.Tap, ctx);
    }

    private void HandleHold(InputAction.CallbackContext ctx, HoldInteraction hold)
    {
        _holdState.TryAdd(ctx.action.id, false);
        if (ctx.started)
        {
            _holdState[ctx.action.id] = false;
        }
        else if (ctx.performed)
        {
            _holdState[ctx.action.id] = true;
            _holdStartTime[ctx.action.id] = ctx.time;
            InvokeTrigger(EInputType.Hold, ctx, new InteractionParam()
            {
                HoldDuration = hold.duration
            });
        }
        else if (ctx.canceled)
        {
            if (_holdState[ctx.action.id])
            {
                var duration = ctx.time - _holdStartTime[ctx.action.id];
                InvokeTrigger(EInputType.ExitHold, ctx, new InteractionParam()
                {
                    ExitHoldDuration = duration
                });
            }
            _holdState[ctx.action.id] = false;
        }
    }

    private void InvokeTrigger(EInputType type, InputAction.CallbackContext ctx, InteractionParam param = default)
    {
        if (_actionTrigger.TryGetValue(ctx.action.id, out var triggerDic))
        {
            if (triggerDic.TryGetValue(type, out var trigger))
            {
                trigger?.Invoke(ctx, param);
            }
        }
    }
}