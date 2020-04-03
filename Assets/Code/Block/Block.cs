using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction { NORTH, EAST, SOUTH, WEST, UP, DOWN }

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
[RequireComponent(typeof(MeshBuilder))]
public class Block : MonoBehaviour
{
    #region Variables

    #region Mesh Variables
    private Mesh mesh = null;
    private MeshCollider col = null;
    private List<Vector3> vertices;
    private List<int> triangles;
    [SerializeField] private List<Vector2> Uvs;
    #endregion

    #region Block Data
    [SerializeField] private BlockData blockData = null;
    [SerializeField] private float Scale = 1f;
    [SerializeField] private Vector3 Position = Vector3.zero;
    [SerializeField] private Vector3 blockIndex = Vector3.zero;
    #endregion

    [SerializeField] private Chunk ParentChunk;

    private Vector3 blockPositions = Vector3.zero;
    private float adjScale;

    [Header("DEBUG")]
    [SerializeField] private GameObject[] Neighbours;

    #endregion

    private void Awake()
    {
        Neighbours = new GameObject[6];
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();
        adjScale = Scale * 0.5f;
        blockPositions = new Vector3((float)Position.x * Scale, (float)Position.y * Scale, (float)Position.z * Scale);
        MakeCube();
    }

    private void Start()
    {
        col.sharedMesh = null;
        col.sharedMesh = mesh;
        col.enabled = blockData.getCollidable;
    }

    private void MakeCube()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        Uvs = new List<Vector2>();
    }

    public void MakeFace(int faceDir, float faceScale, Vector3 facePos)
    {
        vertices.AddRange(MeshBuilder.faceVertices(faceDir, faceScale, facePos));

        int VertCount = vertices.Count;

        triangles.Add(VertCount - 4);
        triangles.Add(VertCount - 3);
        triangles.Add(VertCount - 2);

        triangles.Add(VertCount - 4);
        triangles.Add(VertCount - 2);
        triangles.Add(VertCount - 1);

        Uvs.AddRange(blockData.getUVbyDir((direction)faceDir));
    }

    private void InitializeMesh()
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        Uvs.Clear();


        gameObject.name = blockData.getblockID;
        gameObject.GetComponent<MeshCollider>().enabled = blockData.getCollidable;

        if(ParentChunk.RenderChunk)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!blockData.getVisibility) { }
                else if (ParentChunk.getNeighbour((int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z, (direction)i) == null || !ParentChunk.getNeighbour((int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z, (direction)i).GetComponent<Block>().blockData.getVisibility)
                { MakeFace(i, adjScale, blockPositions); }
                Neighbours[i] = (ParentChunk.getNeighbour((int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z, (direction)i));
            }
        }

        mesh.vertices = vertices.ToArray();
        //Debug.Log("Vertieces : " + mesh.vertices.Length);
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.uv = Uvs.ToArray();
        col.sharedMesh = mesh;

    }

    #region Public Functions

    public void updateMesh() { InitializeMesh(); }

    public void setParentChunk(Chunk chunk) { ParentChunk = chunk; }

    public void setBlockData(BlockData DataPack)
    {
        blockData = DataPack;

        updateMesh();

        if(!blockData.getVisibility)
        {
            foreach (GameObject Neighbour in Neighbours)
            {
                if(Neighbour)
                { Neighbour.GetComponent<Block>().updateMesh(); }
            }
        }

    }

    #region Getters
    public float getBlockScale { get { return adjScale; } }
    public Vector3 getBlockPositions { get { return blockPositions; } }
    public float getScale { get { return adjScale; } }

    public BlockData getBlockData {  get { return blockData; } }

    //public Vector3 getBlockIndex { get { return blockIndex; } }
    //public Block getBlockData { get { return blockData; } }
    #endregion

    #region Setters
    public void setBlockIndex(Vector3 index) { blockIndex = index; }
    #endregion

    #endregion
}
