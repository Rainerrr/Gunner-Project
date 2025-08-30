using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTargetObjective : Objective
{
    [Header("toggle to press")]
    [SerializeField] private BoxCollider Collidercheck;   // child on this prefab
    [SerializeField] private Toggle toggle;
    [SerializeField] private TargetBankSO TargetBankSO;

    // prevent accidental double subscribe
    private bool _subscribedToBank = false;

    private void OnEnable()
    {
        // Cache the child collider once (include inactive just in case)
        if (Collidercheck == null)
            Collidercheck = GetComponentInChildren<BoxCollider>(true);

        // Listen to UI toggle changes
        if (toggle != null)
            toggle.onValueChanged.AddListener(ListentoLaser);

        // Align subscription with current toggle state immediately
        if (toggle != null)
            ListentoLaser(toggle.isOn);
    }

    public override void Activate()
    {
        // Nothing special needed here anymore; OnEnable takes care of wiring.
        // If you prefer, keep Activate() empty.
    }

    private void OnDisable()
    {
        // Always detach from everything on disable
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(ListentoLaser);

        TryUnsubscribeFromBank();
    }

    private void OnDestroy()
    {
        // Belt & suspenders – in case Destroy happens without OnDisable
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(ListentoLaser);

        TryUnsubscribeFromBank();
    }

    private void ListentoLaser(bool isON)
    {
        if (isON)
        {
            TrySubscribeToBank();
        }
        else
        {
            TryUnsubscribeFromBank();
        }
    }

    private void TrySubscribeToBank()
    {
        if (!_subscribedToBank && TargetBankSO != null)
        {
            TargetBankSO.OnTargetAdded += CheckTargetInColider;
            _subscribedToBank = true;
        }
    }

    private void TryUnsubscribeFromBank()
    {
        if (_subscribedToBank && TargetBankSO != null)
        {
            TargetBankSO.OnTargetAdded -= CheckTargetInColider;
            _subscribedToBank = false;
        }
    }

    private void CheckTargetInColider(Target target)
    {
        // If this component (or its GO) was destroyed, do nothing
        if (this == null) return;

        if (Collidercheck == null)
        {
            // Attempt to find again once (prefab might have been re-enabled)
            Collidercheck = GetComponentInChildren<BoxCollider>(true);
            if (Collidercheck == null) return;
        }

        if (target == null) return;

        var targetCollider = target.GetComponent<SphereCollider>();
        if (targetCollider == null) return;

        var neededType = toggle != null
            ? toggle.GetComponent<TargetTypeToggleInfo>()?.targetType
            : null;

        // If no toggle/type info, we can’t validate type; fail fast
        if (neededType == null) return;

        if (Collidercheck.bounds.Intersects(targetCollider.bounds)
            && neededType == target.type)
        {
            CompleteObjective();
        }
        else
        {
            Debug.Log("target added - but not in colider");
        }
    }

    private void CompleteObjective()
    {
        // Stop listening to everything first
        TryUnsubscribeFromBank();
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(ListentoLaser);

        // Disable our logic and notify manager
        enabled = false;
        Complete();

        // IMPORTANT: Do NOT destroy here. Let ObjectiveManager disable/move on.
    }
}
