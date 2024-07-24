using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ManagersB : MonoBehaviour
{
    public static ManagersB Instance;

    public UIManager _ui;
    public static UIManager ui { get { return Instance._ui; } }

    private void Awake()
    {
        Instance = this;
    }
}
