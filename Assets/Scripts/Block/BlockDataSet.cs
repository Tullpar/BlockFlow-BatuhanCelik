using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Block Data Set")]
public class BlockDataSet : ScriptableObject
{
    public List<GameObject> BlockPrefabs = new List<GameObject>();
}
