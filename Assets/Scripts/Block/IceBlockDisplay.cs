using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IceBlockDisplay : MonoBehaviour
{
    public TextMeshPro text;

    public void SetCounter(int count)
    {
        text.enabled = true;
        text.text = count.ToString();
    }

    public void DisableCounter()
    {
        text.enabled = false;
    }
}
