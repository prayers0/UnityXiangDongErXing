
using UnityEngine;

public interface IGlobalManager
{
    void Init();
}

public class GlobaManagerBase<T> : MonoBehaviour, IGlobalManager where T : GlobaManagerBase<T>
{
    public  static T Instance { get; private set; }
    public virtual void Init()
    {
        Instance = (T)this;
    }
}
