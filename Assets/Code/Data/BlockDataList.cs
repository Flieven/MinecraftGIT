using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDataList : MonoBehaviour
{
    [SerializeField] private List<BlockData> DataList = new List<BlockData>();

    public void setUpListofBlockData()
    {
        foreach (BlockData dataPack in Resources.LoadAll("BlockData", typeof(BlockData)))
        {
            DataList.Add(dataPack);
        }
    }

    public BlockData getFromList(string BlockName)
    {
        foreach (BlockData item in DataList)
        {
            if(item.getblockID == BlockName) { return item; }
        }
        return null;
    }
}
