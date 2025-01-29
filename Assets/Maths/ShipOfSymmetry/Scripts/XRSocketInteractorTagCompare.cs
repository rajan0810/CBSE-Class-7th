using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Collections;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Diagnostics;
using NUnit.Framework;



public class XRSocketInteractorTagCompare : XRSocketInteractor
{
    public string compareTag;
    public bool isComplete = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args){
            base.OnSelectEntered(args);
            if (args.interactableObject is XRGrabInteractable grabInteractable){
                StartSocketSnapping(grabInteractable);
                CompareTagOnScelect(grabInteractable.gameObject.tag);
            }
    }
    public void CompareTagOnScelect(string objectTag){
        if(objectTag == compareTag){
            isComplete = true;
        }
        else{
            isComplete = false;
        }
    }
}
