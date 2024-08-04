using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ManagersC : MonoBehaviour
{
    public static ManagersC Instance;

    public UIManager _ui;
    public ObjectManagerC _obj;

    public static UIManager ui { get { return Instance._ui; } }
    public static ObjectManagerC obj { get { return Instance._obj; } }

    private void Awake()
    {
        Instance = this;
    }
}
