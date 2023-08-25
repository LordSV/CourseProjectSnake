using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField] public Material[] Materials;

    public int Length { get { return Materials.Length; } }
    public Material GetMaterial(int index)
    {
        int checkedIndex = index;
        while(checkedIndex >= Materials.Length)
        {
            checkedIndex -= Materials.Length;  
        }
        return Materials[checkedIndex];
        
    }
}
