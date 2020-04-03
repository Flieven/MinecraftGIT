using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkColumn : MonoBehaviour
{
    #region Variables

    #region ChunkData
    [SerializeField] GameObject ChunkObject = null;
    private Vector3 ChunkSize = Vector3.zero;
    #endregion

    #region ColumnData
    [SerializeField] private Vector3 chunkColumnIndex = Vector3.zero;
    [SerializeField] private int ColumnSize = 0;
    #endregion

    [SerializeField] private GameObject[] ChunkList;

    private IEnumerator routine = null;
    private int numSpawnedChunks = 0;
    private DataCoordinates[] offsets =
    {
        new DataCoordinates(0,0,1),
        new DataCoordinates(1,0,0),
        new DataCoordinates(0,0,-1),
        new DataCoordinates(-1,0,0),
        new DataCoordinates(0,1,0),
        new DataCoordinates(0,-1,0)
    };

    private World WorldRef;

    #endregion

    public void SpawnChunks()
    {
        ChunkList = new GameObject[ColumnSize];
        ChunkSize = ChunkObject.GetComponent<Chunk>().getChunkSize;

        for (int i = 0; i < ColumnSize; i++)
        {
            GameObject tempObj = Instantiate(ChunkObject, new Vector3(transform.position.x, transform.position.y + (ChunkSize.y * numSpawnedChunks + 1), transform.position.z), Quaternion.identity);
            tempObj.GetComponent<Chunk>().setWorldRef(WorldRef);
            setChunkData(tempObj);
            routine = tempObj.GetComponent<Chunk>().loadChunk();
            tempObj.GetComponent<Chunk>().StartCoroutine(routine);
            ChunkList[i] = tempObj;
            numSpawnedChunks++;
        }
    }

    private void setChunkData(GameObject McChunkster)
    {
        McChunkster.transform.parent = this.gameObject.transform;
        McChunkster.name = "Chunk(" + chunkColumnIndex.x + "," +numSpawnedChunks + "," + chunkColumnIndex.z + ")";
        McChunkster.GetComponent<Chunk>().setChunkIndex((int)chunkColumnIndex.x, numSpawnedChunks, (int)chunkColumnIndex.z);
    }

    public GameObject GetChunkNeighbour(int x, int y, int z, direction dir)
    {
        Vector3 pos = new Vector3(x, y, z);
        DataCoordinates offsetToCheck = offsets[(int)dir];
        DataCoordinates neighbourCoord = new DataCoordinates((int)pos.x + offsetToCheck.x, (int)pos.y + offsetToCheck.y, (int)pos.z + offsetToCheck.z);

        if (neighbourCoord.x != chunkColumnIndex.x || neighbourCoord.z != chunkColumnIndex.z)
        {
            return WorldRef.GetChunkNeighbour(neighbourCoord.x, neighbourCoord.y, neighbourCoord.z);
        }
        else { return GetChunk(neighbourCoord.y); }
    }

    public GameObject GetChunk(int y)
    {
        if (y >= 0 && y < ColumnSize)
        {
            if (ChunkList[y] != null)
            { return ChunkList[y]; }
            else {/* Debug.LogWarning("Attempted to access array element that does not exists, returning null");*/ return null; }
        }
        else { /*Debug.LogWarning("Attempted to access array element that does not exists, returning null");*/ return null; }
    }

    public void setColumnIndext(int x, int z) { chunkColumnIndex = new Vector3(x, 0, z); }
    public void setWorld(World world) { WorldRef = world; }

    public void updateChunks()
    {
        foreach(GameObject chunk in ChunkList)
        {
            chunk.GetComponent<Chunk>().updateChunk();
        }
    }

    public void inhabitChunks()
    {
        foreach(GameObject chunk in ChunkList)
        {
            chunk.GetComponent<ChunkInhabitor>().InhabitChunk();
        }
    }

    #region Getters

    public int Width
    { get { return (int)WorldRef.Width; } }

    public int Height
    { get { return (int)ChunkSize.y; } }

    public int Depth
    { get { return (int)WorldRef.Depth; } }

    public int ChunkNumber
    { get { return ColumnSize; } }

    public Vector3 getColumnIndex
    { get { return chunkColumnIndex; } }

    #endregion
}
