using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataCoordinates
{
    internal int x;
    internal int y;
    internal int z;

    public DataCoordinates(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[RequireComponent(typeof(ChunkInhabitor))]
public class Chunk : MonoBehaviour
{
    #region Variables

    private bool SpawnChunk = false;
    private bool ChunkRender = false;

    [SerializeField] private Vector3 ChunkSize = Vector3.zero;
   
    #pragma warning disable 0649
    [SerializeField] private GameObject BlockObject;
    #pragma warning restore 0649

    [SerializeField] private float BlockSize = 1.0f;

    [SerializeField] private Vector3 ChunkIndex = Vector3.zero;
    private GameObject[,,] BlockList;

    private DataCoordinates[] offsets =
{
        new DataCoordinates(0,0,1),
        new DataCoordinates(1,0,0),
        new DataCoordinates(0,0,-1),
        new DataCoordinates(-1,0,0),
        new DataCoordinates(0,1,0),
        new DataCoordinates(0,-1,0)
    };

    private World WorldRef = null;

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Updating Chunks");
            updateChunk();
        }
    }

    public IEnumerator loadChunk()
    {
        BlockList = new GameObject[(int)ChunkSize.x, (int)ChunkSize.y, (int)ChunkSize.z];
        BlockSize = BlockObject.GetComponent<Block>().getScale;

        GenerateChunk();

        yield return null;

        transform.GetComponent<ChunkInhabitor>().InhabitChunk();
        updateChunk();
        if (SpawnChunk)
        {
            foreach (GameObject block in BlockList)
            {
                if (block.name == "Grass")
                {
                    getWorld.getPlayer.transform.position = new Vector3(block.transform.position.x, block.transform.position.y + 2, block.transform.position.z);
                    break;
                }
            }
        }
    }

    private void GenerateChunk()
    {
        for (int y = 0; y < ChunkSize.y; y++)
        {
            for (int x = 0; x < ChunkSize.x; x++)
            {
                for (int z = 0; z < ChunkSize.z; z++)
                {
                    GameObject tempBlock = Instantiate(BlockObject, new Vector3(transform.position.x + BlockSize + x, transform.position.y + BlockSize + y, transform.position.z + BlockSize + z), Quaternion.identity);
                    Block BLogic = tempBlock.GetComponent<Block>();
                    BLogic.setParentChunk(this.gameObject.GetComponent<Chunk>());
                    tempBlock.transform.parent = gameObject.transform;

                    BlockList[x, y, z] = tempBlock;
                    BLogic.setBlockIndex(new Vector3(x, y, z));
                }
            }
        }
    }

    public GameObject getNeighbour(int x, int y, int z, direction dir)
    {
        Vector3 pos = new Vector3(x, y, z);
        DataCoordinates offsetToCheck = offsets[(int)dir];
        DataCoordinates neighbourCoord = new DataCoordinates((int)pos.x + offsetToCheck.x, (int)pos.y + offsetToCheck.y, (int)pos.z + offsetToCheck.z);

        if (neighbourCoord.x < 0 || neighbourCoord.x >= Width || neighbourCoord.y < 0 || neighbourCoord.y >= Height || neighbourCoord.z < 0 || neighbourCoord.z >= Depth) { return checkChunkNeighbours(x, y, z, dir); }
        else
        {
            //if(y == chunkSize.y) { Debug.Log("Returning Block at: X: " + neighbourCoord.x + " Y: " + neighbourCoord.y + " Z: " + neighbourCoord.z); }
            return GetBlock(neighbourCoord.x, neighbourCoord.y, neighbourCoord.z);
        }
    }

    private GameObject checkChunkNeighbours(int x, int y, int z, direction dir)
    {
        //Debug.Log("Chunk Direction given: " + dir);
        GameObject temp = transform.parent.GetComponent<ChunkColumn>().GetChunkNeighbour((int)ChunkIndex.x, (int)ChunkIndex.y, (int)ChunkIndex.z, dir);
        if (temp != null)
        {
            GameObject blockRef = null;
            switch (dir)
            {
                case direction.NORTH: if (temp.GetComponent<Chunk>().GetBlock(x, y, 0) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock(x, y, 0); /*Debug.Log("Called blockRef for NORTH Chunk, block ref: " + blockRef);*/ } break;
                case direction.SOUTH: if (temp.GetComponent<Chunk>().GetBlock(x, y, (int)ChunkSize.z -1 ) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock(x, y, (int)ChunkSize.z - 1); /*Debug.Log("Called blockRef for SOUTH Chunk, block ref: " + blockRef);*/ } break;
                case direction.EAST: if (temp.GetComponent<Chunk>().GetBlock(0, y, z) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock(0, y, z); /*Debug.Log("Called blockRef for EAST Chunk, block ref: " + blockRef);*/ } break;
                case direction.WEST: if (temp.GetComponent<Chunk>().GetBlock((int)ChunkSize.x - 1, y, z) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock((int)ChunkSize.x - 1, y, z); /*Debug.Log("Called blockRef for WEST Chunk, block ref: " + blockRef);*/ } break;
                case direction.UP: if (temp.GetComponent<Chunk>().GetBlock(x, 0, z) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock(x, 0, z); /*Debug.Log("Called blockRef for UP Chunk, block ref: " + blockRef);*/ } break;
                case direction.DOWN: if (temp.GetComponent<Chunk>().GetBlock(x, (int)ChunkSize.y - 1, z) != null) { blockRef = temp.GetComponent<Chunk>().GetBlock(x, (int)ChunkSize.y - 1, z); /*Debug.Log("Called blockRef for DOWN Chunk, block ref: " + blockRef);*/ } break;
            }
            return blockRef;
        }
        else { return null; }
    }

    public GameObject GetBlock(int x, int y, int z) { return BlockList[x, y, z]; }

    public void setChunkIndex(int x, int y, int z) { ChunkIndex = new Vector3(x, y, z); }

    public void setWorldRef(World Reference) { WorldRef = Reference; }

    #region Getters
    public int Width { get { return BlockList.GetLength(0); } }

    public int Height { get { return BlockList.GetLength(1); } }

    public int Depth { get { return BlockList.GetLength(2); } }

    public Vector3 getChunkSize { get { return ChunkSize; } }

    public Vector3 getChunkIndex {  get { return ChunkIndex; } }

    public GameObject[,,] getBlockList {  get { return BlockList; } }

    public World getWorld {  get { return WorldRef; } }

    public bool IsSpawnChunk
    { set { SpawnChunk = value; } }

    public bool RenderChunk
    {
        get { return ChunkRender; }
        set { ChunkRender = value; }
    }

    #endregion

    public void updateChunk()
    {
        foreach (GameObject block in BlockList)
        {
            block.GetComponent<Block>().updateMesh();
        }
    }
}
