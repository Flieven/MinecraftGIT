using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockDataList), typeof(PerlinNoise))]
public class World : MonoBehaviour
{
    #region Variables

    private DataCoordinates[] offsets =
    {
        new DataCoordinates(0,0,1),
        new DataCoordinates(1,0,0),
        new DataCoordinates(0,0,-1),
        new DataCoordinates(-1,0,0),
        new DataCoordinates(0,1,0),
        new DataCoordinates(0,-1,0)
    };

    [SerializeField] private Vector2 WorldSize = Vector2.zero;

    [Header("Object References")]
    [SerializeField] private GameObject ChunkColumnObject = null;
    [SerializeField] private GameObject ChunkObject = null;

    [Header("Spawnables")]
    [SerializeField] private GameObject PlayerObject = null;

    private GameObject SpawnChunkRef = null;

    private Vector3 ChunkSize = Vector3.zero;

    /* 
     * TODO: 
     * Change this to either an unordered_map or a Hashtable for dynamic key-value pair construction / procedural mapping.
     */
    private GameObject[,] ColumnList;
    #endregion


    private void Awake()
    {
        transform.GetComponent<BlockDataList>().setUpListofBlockData();
        ChunkSize = ChunkObject.GetComponent<Chunk>().getChunkSize;
        ColumnList = new GameObject[(int)WorldSize.x, (int)WorldSize.y];
        StartCoroutine(InitalizeWorld());
    }

    private IEnumerator InitalizeWorld()
    {
        for (int x = 0; x < WorldSize.x; x++)
        {
            for (int z = 0; z < WorldSize.y; z++)
            {
                GameObject tempObj = Instantiate(ChunkColumnObject, new Vector3(x * ChunkSize.x, 0, z * ChunkSize.z), Quaternion.identity);
                tempObj.name = "Chunk Column(" + x + "," + z +")";
                tempObj.transform.parent = this.gameObject.transform;
                tempObj.GetComponent<ChunkColumn>().setColumnIndext(x, z);
                tempObj.GetComponent<ChunkColumn>().setWorld(this);
                ColumnList[x, z] = tempObj;
                tempObj.GetComponent<ChunkColumn>().SpawnChunks();
            }
        }

        yield return null;
    }

    //public IEnumerator GenerateChunkColumn(int x, int z)
    //{
    //    GameObject tempObj = Instantiate(ChunkColumnObject, new Vector3(x * ChunkSize.x, 0, z * ChunkSize.z), Quaternion.identity);
    //    tempObj.name = "Chunk Column(" + x + "," + z + ")";
    //    tempObj.transform.parent = this.gameObject.transform;
    //    tempObj.GetComponent<ChunkColumn>().setColumnIndext(x, z);
    //    tempObj.GetComponent<ChunkColumn>().setWorld(this);
    //    ColumnList.Add(tempObj);
    //    tempObj.GetComponent<ChunkColumn>().SpawnChunks();

    //    yield return null;
    //}

    public GameObject GetChunkNeighbour(int x, int y, int z)
    {
        //Debug.Log("Getting Neighbour Chunk: " + neighbourCoord.x + "," + neighbourCoord.z);

        if (x < 0 || x >= Width || z < 0 || z >= Depth) { return null; }
        else { return GetChunk(x, y, z); }
    }

    private GameObject GetChunk(int x, int y, int z)
    {
        Vector3 ColumnToCheck = new Vector3(x, y, z);
        foreach(GameObject column in ColumnList)
        {
            if(column.GetComponent<ChunkColumn>().getColumnIndex == ColumnToCheck)
            {
                return column;
            }
        }

        return null;
    }

    public int Width
    { get { return ColumnList.GetLength(0); } }
    public int Depth
    { get { return ColumnList.GetLength(1); } }

    public GameObject SpawnChunk
    {
        get { return SpawnChunkRef; }
        set { SpawnChunkRef = value; }
    }

    public GameObject getFromChunkList(int x, int z) { return ColumnList[x, z]; }

    public GameObject getPlayer
    { get { return PlayerObject; } }

}
