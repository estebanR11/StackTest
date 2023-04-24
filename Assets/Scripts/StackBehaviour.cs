using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBehaviour : MonoBehaviour
{
    [SerializeField]List<GameObject> blocksInStack;
    private void Start()
    {
        blocksInStack = new List<GameObject>();
    }
    public void LoadBlocks(GameObject block)
    {
        blocksInStack.Add(block);
    }

    public void RemoveGlassBlocks()
    {
        for(int i =0;i<blocksInStack.Count;i++)
        {
            BlockBehaviour actualBlock = blocksInStack[i].GetComponent<BlockBehaviour>();
            actualBlock.ActivatePhysics();
            actualBlock.DestroyBlock();
   

        }
    }
}
