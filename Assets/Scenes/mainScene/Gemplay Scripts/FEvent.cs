using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FEvent : MonoBehaviour
{
    public string DialogPointName;
    public abstract void OnInteract(PlayerDialogManager playerDialogManager);
}

