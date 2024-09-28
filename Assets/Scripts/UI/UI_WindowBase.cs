using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_WindowBase : MonoBehaviour
{
    public virtual void OnShow() { }
    public virtual void OnClose() { }
}
