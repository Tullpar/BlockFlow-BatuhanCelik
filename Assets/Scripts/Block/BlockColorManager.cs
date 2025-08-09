using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColorManager : MonoBehaviour
{
    #region Singleton
    public static BlockColorManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public ColorData ColorData;

}
