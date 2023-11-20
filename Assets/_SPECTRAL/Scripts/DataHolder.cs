using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance;

    public GameData GameData;
    public HumanData HumanData;

    private void Awake()
    {
        Instance = this;
    }
}
