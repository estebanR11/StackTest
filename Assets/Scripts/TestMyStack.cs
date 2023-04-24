using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMyStack : MonoBehaviour
{

    [SerializeField]StackBehaviour actualStack;

    public void ChangeStack(StackBehaviour pStack)
    {
        actualStack = pStack;

    }

    public void removeGlass()
    {
        actualStack.RemoveGlassBlocks();
    }
}
