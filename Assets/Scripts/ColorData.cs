using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Color Data")]
public class ColorData : ScriptableObject
{
    public List<Material> materials = new List<Material>();
    public Material iceMaterial;
}
