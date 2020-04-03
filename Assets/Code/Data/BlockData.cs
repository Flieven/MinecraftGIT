using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Unity Minecraft/Block", order = 1)]
public class BlockData : ScriptableObject
{
    private enum BlockTypes { Air, Dirt, Wood, Stone, Sand, Utility, Liquid, Redstone, Natural, Bedrock, Decorative }

#pragma warning disable 0414
    [SerializeField] BlockTypes blockCategory = BlockTypes.Air;
#pragma warning restore 0414

    [SerializeField] private string ID = "";
    [SerializeField] private bool Destructable = true;
    [SerializeField] private bool Movable = true;
    [SerializeField] private bool Visible = true;
    [SerializeField] private bool Collidable = true;
    [SerializeField] private int BlockDensity = 0;
    [SerializeField] private Vector2 FrontVec = Vector2.zero, BackVec = Vector2.zero, TopVec = Vector2.zero, BottomVec = Vector2.zero, LeftVec = Vector2.zero, RightVec = Vector2.zero;

    private Vector2 FT_Vec = Vector2.zero, BT_Vec = Vector2.zero, TT_Vec = Vector2.zero, DT_Vec = Vector2.zero, LT_Vec = Vector2.zero, RT_Vec = Vector2.zero;
    private Vector2[] UVs;

    //[SerializeField] private Texture2D textureAtlas = null;
    //[SerializeField] private Texture2D InventoryIcon = null;

    private void OnValidate()
    {
        Debug.Log("VALIDATE ME!");
        calcUVs();
    }

    private void calcUVs()
    {
        setVectorModifiers();
        UVs = new Vector2[24];
        // Front
        UVs[2] = new Vector2(FT_Vec.x, FT_Vec.y);
        UVs[3] = new Vector2(0.04f + FT_Vec.x, FT_Vec.y);
        UVs[1] = new Vector2(FT_Vec.x, 0.0495f + FT_Vec.y);
        UVs[0] = new Vector2(0.04f + FT_Vec.x, 0.0495f + FT_Vec.y);
        // Top
        UVs[9] = new Vector2(TT_Vec.x, TT_Vec.y);
        UVs[8] = new Vector2(0.04f + TT_Vec.x, TT_Vec.y);
        UVs[10] = new Vector2(TT_Vec.x, 0.0495f + TT_Vec.y);
        UVs[11] = new Vector2(0.04f + TT_Vec.x, 0.0495f + TT_Vec.y);
        // Back
        UVs[6] = new Vector2(BT_Vec.x, BT_Vec.y);
        UVs[7] = new Vector2(0.04f + BT_Vec.x, BT_Vec.y);
        UVs[5] = new Vector2(BT_Vec.x, 0.0495f + BT_Vec.y);
        UVs[4] = new Vector2(0.04f + BT_Vec.x, 0.0495f + BT_Vec.y);
        // Bottom
        UVs[12] = new Vector2(DT_Vec.x, DT_Vec.y);
        UVs[13] = new Vector2(DT_Vec.x, 0.0495f + DT_Vec.y);
        UVs[14] = new Vector2(0.04f + DT_Vec.x, 0.0495f + DT_Vec.y);
        UVs[15] = new Vector2(0.04f + DT_Vec.x, DT_Vec.y);
        // Left
        UVs[19] = new Vector2(LT_Vec.x, LT_Vec.y);
        UVs[16] = new Vector2(LT_Vec.x, 0.0495f + LT_Vec.y);
        UVs[17] = new Vector2(0.04f + LT_Vec.x, 0.0495f + LT_Vec.y);
        UVs[18] = new Vector2(0.04f + LT_Vec.x, LT_Vec.y);
        // Right        
        UVs[23] = new Vector2(RT_Vec.x, RT_Vec.y);
        UVs[20] = new Vector2(RT_Vec.x, 0.0495f + RT_Vec.y);
        UVs[21] = new Vector2(0.04f + RT_Vec.x, 0.0495f + RT_Vec.y);
        UVs[22] = new Vector2(0.04f + RT_Vec.x, RT_Vec.y);
    }

    private void setVectorModifiers()
    {
        FT_Vec.x = FrontVec.x * 0.04f;
        FT_Vec.y = FrontVec.y * 0.0526f;

        BT_Vec.x = BackVec.x * 0.04f;
        BT_Vec.y = BackVec.y * 0.0526f;

        TT_Vec.x = TopVec.x * 0.04f;
        TT_Vec.y = TopVec.y * 0.0526f;

        DT_Vec.x = BottomVec.x * 0.04f;
        DT_Vec.y = BottomVec.y * 0.0526f;

        LT_Vec.x = LeftVec.x * 0.04f;
        LT_Vec.y = LeftVec.y * 0.0526f;

        RT_Vec.x = RightVec.x * 0.04f;
        RT_Vec.y = RightVec.y * 0.0526f;
    }

    //Public Get/Set

    public Vector2[] getUVbyDir(direction dir)
    {
        List<Vector2> UVsToPass = new List<Vector2>();
        switch (dir)
        {
            case direction.NORTH:
                //Debug.Log("Called NORTH!");
                UVsToPass.Add(UVs[0]);
                UVsToPass.Add(UVs[1]);
                UVsToPass.Add(UVs[2]);
                UVsToPass.Add(UVs[3]);
                break;
            //=====\\
            case direction.SOUTH:
                //Debug.Log("Called UP!");
                UVsToPass.Add(UVs[4]);
                UVsToPass.Add(UVs[5]);
                UVsToPass.Add(UVs[6]);
                UVsToPass.Add(UVs[7]);
                break;
            //=====\\
            case direction.UP:
                //Debug.Log("Called SOUTH!");
                UVsToPass.Add(UVs[8]);
                UVsToPass.Add(UVs[9]);
                UVsToPass.Add(UVs[10]);
                UVsToPass.Add(UVs[11]);
                break;
            //=====\\
            case direction.DOWN:
                //Debug.Log("Called DOWN!");
                UVsToPass.Add(UVs[12]);
                UVsToPass.Add(UVs[13]);
                UVsToPass.Add(UVs[14]);
                UVsToPass.Add(UVs[15]);
                break;
            //=====\\
            case direction.WEST:
                //Debug.Log("Called WEST!");
                UVsToPass.Add(UVs[16]);
                UVsToPass.Add(UVs[17]);
                UVsToPass.Add(UVs[18]);
                UVsToPass.Add(UVs[19]);
                break;
            //=====\\
            case direction.EAST:
                //Debug.Log("Called EAST!");
                UVsToPass.Add(UVs[20]);
                UVsToPass.Add(UVs[21]);
                UVsToPass.Add(UVs[22]);
                UVsToPass.Add(UVs[23]);
                break;
        }

        return UVsToPass.ToArray();

    }

    public string getblockID { get { return ID; } }
    public bool getDestructable { get { return Destructable; } }
    public bool getMovable { get { return Movable; } }
    public bool getVisibility { get { return Visible; } }
    public bool getCollidable { get { return Collidable; } }
    public int getDensity { get { return BlockDensity; } }
}
