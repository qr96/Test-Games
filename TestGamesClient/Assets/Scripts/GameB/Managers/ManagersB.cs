using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ManagersB : MonoBehaviour
{
    public static ManagersB Instance;

    public UIManager _ui;
    public DataManagerB _data;

    public static UIManager ui { get { return Instance._ui; } }
    public static DataManagerB data { get { return Instance._data; } }

    private void Awake()
    {
        Instance = this;
    }
}
