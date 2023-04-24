using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockBehaviour : MonoBehaviour
{
    BlockData data;
    [SerializeField] TextMeshProUGUI infoText;
    bool glass = false;
    
    public void SetData(BlockData pData)
    {
        data = pData;
        infoText = GameObject.FindGameObjectWithTag("infoText").GetComponent<TextMeshProUGUI>();

        if(pData.mastery == 0)
        {
            glass = true;
        }
    }

    public void ShowData()
    {
        string text = "Grade: [" + data.grade + "] Domain: [" + data.domain + "]" + "\n" + "Cluster: [" + data.cluster + "] " + "\n"
            + "Standard ID: [" + data.standardid + "] Standard Description: [" + data.standarddescription + "]";
     
            infoText.text = text;
    }

    public bool isGlass()
    {
        return glass;
    }

    public void ActivatePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    public void DestroyBlock()
    {
        if(isGlass())
        {
            gameObject.SetActive(false);
        }
    }
}
