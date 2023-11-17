using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    public static DataMgr Instance;

    public GameData GameData;
    public HumanData HumanData;

    private void Awake()
    {
        Instance = this;
    }
}
