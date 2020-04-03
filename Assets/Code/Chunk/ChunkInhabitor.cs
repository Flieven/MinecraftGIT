using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInhabitor : MonoBehaviour
{
    #region Variables
    [SerializeField] private Vector3 ChunkIndex = Vector3.zero;
    private Chunk ChunkData;

    #endregion

    public void InhabitChunk()
    {
        ChunkData = transform.GetComponent<Chunk>();
        ChunkIndex = ChunkData.getChunkIndex;
        EvaluateChunkIndex();
    }

    private void EvaluateChunkIndex()
    {
        if(ChunkIndex.y == 0) { MakeBaseChunk(); }
        else { MakeDefaultChunk(); }
    }

    private void MakeBaseChunk()
    {
        for (int x = 0; x < ChunkData.Width; x++)
        {
            for (int z = 0; z < ChunkData.Depth; z++)
            {
                //Debug.Log("Changing Block Data");
                ChunkData.getBlockList[x, 0, z].GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Bedrock"));
                for(int y = 1; y < ChunkData.Height; y++)
                {
                    CalculatePerlinWorm(ChunkData.getBlockList[x, y, z].transform.position);

                    if(ChunkData.getBlockList[x,y,z].GetComponent<Block>().getBlockData.getVisibility) { ChunkData.getBlockList[x, y, z].GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Stone")); }
                }
            }
        }
    }

    private void MakeDefaultChunk()
    {
        for (int x = 0; x < ChunkData.Width; x++)
        {
            for (int z = 0; z < ChunkData.Depth; z++)
            {
                for (int y = 0; y < ChunkData.Height; y++)
                {

                    if (ChunkData.getChunkIndex.y >= transform.parent.GetComponent<ChunkColumn>().ChunkNumber / 2 && ChunkData.getChunkIndex.y <= transform.parent.GetComponent<ChunkColumn>().ChunkNumber / 2)
                    {
                        float HeightMap = ChunkData.getWorld.gameObject.GetComponent<PerlinNoise>().CalculateHeightMap(ChunkData.getBlockList[x, y, z].transform.position);

                        CalculatePerlinWorm(ChunkData.getBlockList[x, y, z].transform.position);
                        CalculateHeightBlocks(HeightMap, new Vector3(x,y,z));
                    }

                    else if (ChunkData.getBlockList[x, y, z].GetComponent<Block>().getBlockData.getVisibility && ChunkData.getChunkIndex.y < transform.parent.GetComponent<ChunkColumn>().ChunkNumber / 2)
                    {
                        CalculatePerlinWorm(ChunkData.getBlockList[x, y, z].transform.position);
                        if(ChunkData.getBlockList[x, y, z].GetComponent<Block>().getBlockData.getVisibility)
                        {
                            ChunkData.getBlockList[x, y, z].GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Stone"));
                        }
                    }

                    else { ChunkData.getBlockList[x, y, z].GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Air")); }
                }
            }
        }
    }

    private void CalculateHeightBlocks(float HeightMap, Vector3 pos)
    {
        int ChunkOffset;
        Vector3 BlockPosition = ChunkData.getBlockList[(int)pos.x, (int)pos.y, (int)pos.z].transform.position;


        if ((Mathf.Abs(HeightMap) / HeightMap) == -1)
        {
            ChunkOffset = Mathf.CeilToInt(HeightMap / ChunkData.Height);
            HeightMap = (ChunkData.getChunkSize.y + (HeightMap % ChunkData.getChunkSize.y) - 1);
        }
        else
        {
            ChunkOffset = Mathf.FloorToInt(HeightMap / ChunkData.Height);
            HeightMap = HeightMap % ChunkData.getChunkSize.y;
        }

        GameObject BaseBlock = null;
        GameObject BaseChunk = transform.parent.GetComponent<ChunkColumn>().GetChunk((int)ChunkIndex.y + ChunkOffset);

        if (BaseChunk && HeightMap < ChunkData.getChunkSize.y)
        {
            if(!ChunkData.getWorld.SpawnChunk)
            {
                ChunkData.getWorld.SpawnChunk = BaseChunk;
                BaseChunk.GetComponent<Chunk>().IsSpawnChunk = true;
                BaseChunk.GetComponent<Chunk>().RenderChunk = true;
            }

            //Debug.Log(transform.parent.name + " | " + BaseChunk.name);
            for (int y = 0; y < BaseChunk.GetComponent<Chunk>().Height; y++)
            {
                BaseBlock = BaseChunk.GetComponent<Chunk>().GetBlock((int)pos.x, y, (int)pos.z);

                if(BaseBlock.GetComponent<Block>().getBlockData.getVisibility)
                {
                    if (BaseBlock && BaseBlock.transform.localPosition.y >= (int)HeightMap - 2 && BaseBlock.transform.localPosition.y < (int)HeightMap)
                    { BaseBlock.GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Dirt")); }
                    else if (BaseBlock && BaseBlock.transform.localPosition.y == (int)HeightMap)
                    { BaseBlock.GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Grass")); }
                    else if (BaseBlock && BaseBlock.transform.localPosition.y < HeightMap)
                    { BaseBlock.GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Stone")); }
                    else
                    { BaseBlock.GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Air")); }
                }
            }
        }
    }

    private void CalculatePerlinWorm(Vector3 pos)
    {
        float Density = transform.GetComponent<Chunk>().getWorld.GetComponent<PerlinNoise>().CalculateDensity(pos);

        if (Density < 0.3)
        {
            int WormSize = Random.Range(0, 4);
            WormSize += (int)(Density % 3.0f);
            Collider[] hitColliders = Physics.OverlapSphere(pos, WormSize);
            foreach(Collider col in hitColliders)
            {
                col.gameObject.GetComponent<Block>().setBlockData(transform.GetComponent<Chunk>().getWorld.GetComponent<BlockDataList>().getFromList("Air"));
            }

        }
    }

}
