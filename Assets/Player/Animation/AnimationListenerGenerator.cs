using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationListenerGenerator : MonoBehaviour
{
    [SerializeField] protected string fileName;
    public void GenerateConstFile()
    {
        AnimationListenerHelper.GenerateConstFile(fileName, GetComponent<Animator>());
    }
}
