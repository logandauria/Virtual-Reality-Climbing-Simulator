using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{
    // called when grab is entered
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        // init component
        if (interactor is XRDirectInteractor)
            Climber.climbingHand = interactor.GetComponent<XRController>();
    }

    // called when grab is exited
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);

        if(interactor is XRDirectInteractor)
        {
            if(Climber.climbingHand && Climber.climbingHand.name == interactor.name)
            {
                Climber.climbingHand = null;
            }
        }
    }
}
