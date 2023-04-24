using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using System;

public class BlockSpawnManager : MonoBehaviour
{



    private BlockData[] blockDataList;

    [SerializeField] private GameObject glassBlock;
    [SerializeField] private GameObject woodBlock;
    [SerializeField] private GameObject stoneBlock;

    [SerializeField]private Transform grade6;
    [SerializeField]private Transform grade7;
    [SerializeField]private Transform grade8;

    [SerializeField] private StackBehaviour grade6Stack;
    [SerializeField] private StackBehaviour grade7Stack;
    [SerializeField] private StackBehaviour grade8Stack;
    Vector3 StartingNormalPosition = new Vector3(0, 0, 0);
    Vector3 StartingRotatedPosition = new Vector3(-0.4f, 0, 0.4f);
    Vector3 actualPosition = Vector3.zero;
    float yOffset;
    bool isRotated = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        string url = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch data: " + www.error);
        }
        else
        {
            Debug.Log("Data fetched successfully: " + www.downloadHandler.text);
            string responseText = www.downloadHandler.text;
            blockDataList = JsonConvert.DeserializeObject<BlockData[]>(responseText);
            LoadBlocks();
        }

    }


    void LoadBlocks()
    {
        Transform gradeStartingPosition = null;

        Quaternion rotation = Quaternion.identity;

        Vector3 actualPosition = Vector3.zero; ;
        int blockCount = 0;
        string currentGrade = "";

        List<BlockData> grade6Blocks = new List<BlockData>();
        List<BlockData> grade7Blocks = new List<BlockData>();
        List<BlockData> grade8Blocks = new List<BlockData>();

        // populate the lists based on grade
        for (int i = 0; i < blockDataList.Length; i++)
        {
            switch (blockDataList[i].grade)
            {
                case "6th Grade":
                    grade6Blocks.Add(blockDataList[i]);
                    break;
                case "7th Grade":
                    grade7Blocks.Add(blockDataList[i]);
                    break;
                case "8th Grade":
                    grade8Blocks.Add(blockDataList[i]);
                    break;
                default:
                    break;
            }
        }
        grade6Blocks = grade6Blocks.OrderBy(b => b.domain).ThenBy(b => b.cluster).ThenBy(b => b.standardid).ToList();
        grade7Blocks = grade7Blocks.OrderBy(b => b.domain).ThenBy(b => b.cluster).ThenBy(b => b.standardid).ToList();
        grade8Blocks = grade8Blocks.OrderBy(b => b.domain).ThenBy(b => b.cluster).ThenBy(b => b.standardid).ToList();

  
   
        Action<List<BlockData>, Transform, StackBehaviour> createBlocks = (blocks, startingPosition, actualStack) =>
        {
           
            for (int i = 0; i < blocks.Count; i++)
            {
                BlockData blockData = blocks[i];
                GameObject blockPrefab = null;
                switch (blockData.mastery)
                {
                    case 0:
                        blockPrefab = glassBlock;
                        break;
                    case 1:
                        blockPrefab = woodBlock;
                        break;
                    case 2:
                        blockPrefab = stoneBlock;
                        break;
                }
                GameObject block = Instantiate(blockPrefab, startingPosition);
                block.GetComponent<BlockBehaviour>().SetData(blockData);
                actualStack.LoadBlocks(block);
                if (!isRotated)
                {
                    block.transform.SetLocalPositionAndRotation(actualPosition, Quaternion.Euler(0f, 0f, 0f));
                    actualPosition += new Vector3(0f, 0f, 0.4f);
                }
                else
                {
                    block.transform.SetLocalPositionAndRotation(actualPosition, Quaternion.Euler(0f, 90f, 0f));
                    actualPosition += new Vector3(0.4f, 0f, 0f);
                }
                blockCount++;
                if (blockCount == 3)
                {
                    yOffset += 0.2f;
                    blockCount = 0;
                    if (!isRotated)
                    {

                        actualPosition = new Vector3(-block.transform.localScale.z, 0, block.transform.localScale.z) + new Vector3(0, yOffset, 0);
                        isRotated = true;
                    }

                    else
                    {
                        actualPosition = new Vector3(0, yOffset, 0);
                        isRotated = false;
                    }
                }
            }

            yOffset = 0;
            actualPosition = Vector3.zero;
        };

        
        
        createBlocks(grade6Blocks, grade6,grade6Stack);
        createBlocks(grade7Blocks, grade7, grade7Stack);
        createBlocks(grade8Blocks, grade8, grade8Stack);




    }
}


[System.Serializable]
public class BlockData
{
    public int id;
    public string subject;
    public string grade;
    public int mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;
}

[System.Serializable]
public class BlockDataArray
{
    public BlockData[] responseData;
}
