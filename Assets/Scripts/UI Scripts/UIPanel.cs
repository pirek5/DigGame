using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class UIPanel<T> : UIPanel where T : UIPanel<T>
//{
//    private static T instance;
//    public static T Instance {get {return instance; }}

//    protected virtual void Awake()
//    {
//        if (instance != null)
//        {
//            //Destroy(gameObject);
//        }
//        else
//        {
//            instance = (T)this;
//        }
//    }

//    public static void Open()
//    {
//        if (UIPanelManager.Instance != null && Instance != null)
//        {
//            UIPanelManager.Instance.OpenPanel(Instance);
//        }
//    }

//    public override void Init() { }
//    public override void SetDependency(PlayerInput playerInput, BuildManager buildManager) { }
//}


[RequireComponent(typeof(Canvas))]
public class UIPanel : MonoBehaviour
{
    public virtual void Init() { }
    public virtual void SetDependency(PlayerInput playerInput, BuildManager buildManager) { }
}
