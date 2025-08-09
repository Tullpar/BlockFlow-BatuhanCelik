using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BlockColorSelector : MonoBehaviour
{
    public Renderer Renderer;
    public int ColorMaterialIndex = 0;

    public GameObject VerticalArrow;
    public GameObject HorizontalArrow;

    public void SetColor(Block.BlockColor color)
    {
        Material[] materials = Renderer.materials;
        materials[ColorMaterialIndex] = BlockColorManager.Instance.ColorData.materials[(int)color];
        Renderer.materials = materials;
    }

    public void SetArrows(Block.MovementType movementType)
    {
        switch(movementType)
        {
            case Block.MovementType.Vertical:
                VerticalArrow.SetActive(true);
                HorizontalArrow.SetActive(false);
                break;
            case Block.MovementType.Horizontal:
                HorizontalArrow.SetActive(true);
                VerticalArrow.SetActive(false);
                break;
            default:
                VerticalArrow.SetActive(false);
                HorizontalArrow.SetActive(false);
                break;
        }
        
    }

    public void SetIce()
    {
        Material[] materials = Renderer.materials;
        materials[ColorMaterialIndex] = BlockColorManager.Instance.ColorData.iceMaterial;
        Renderer.materials = materials;
    }
}
