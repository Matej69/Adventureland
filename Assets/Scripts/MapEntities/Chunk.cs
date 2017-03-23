using UnityEngine;
using System.Collections;


namespace n_chunk
{

    //** VALUES MUST BE SMALLER THEN 255 BECAUSE WE'R USING BYTE, NOT INT
    public static class CHUNK_SIZE {
        public const int X = 50;
        public const int Z = 50;
        public const int Y = 150;
    }


    public class Chunk
    {
        public n_block.BlockLogic[,,] blocks = new n_block.BlockLogic[CHUNK_SIZE.X, CHUNK_SIZE.Z, CHUNK_SIZE.Y];

        public Chunk()
        {
            FillWithData();
        }

        //** We can pass some parameters for different chunk generation
        public void FillWithData()
        {
            for (int x = 0; x < n_chunk.CHUNK_SIZE.X; ++x)
                for (int z = 0; z < n_chunk.CHUNK_SIZE.Z; ++z)
                    for (int y = 0; y < n_chunk.CHUNK_SIZE.Y; ++y)
                    {
                        int level = n_block.BlockOccurrenceDatabase.GetLevelFromDepth(y);
                        n_block.E_BLOCK blockType = n_block.BlockOccurrenceDatabase.GenerateRandomBlock(level);

                        Vector3Int pos = new Vector3Int(x, y, z);
                        blocks[x, z, y] = new n_block.BlockLogic(pos, (byte)blockType);
                    }
        }

        public void DoForEachBlock(System.Func<Vector3Int, n_block.E_BLOCK, GameObject> fun)
        {
            foreach (n_block.BlockLogic block in blocks)
                fun(block.cord, (n_block.E_BLOCK)block.id);
        }

        
        public bool DoesBlockExists(Vector3Int _posID)
        {
            //if out of any of bounds
            if (_posID.x < 0 || _posID.x > CHUNK_SIZE.X - 1 ||
                _posID.y < 0 || _posID.y > CHUNK_SIZE.Y - 1 ||
                _posID.z < 0 || _posID.z > CHUNK_SIZE.Z - 1)
                return false;
            //if inside bounds but it's memory is null
            return (blocks[_posID.x, _posID.z, _posID.y] != null);
        }

        public void ClearBlockFromMemory(Vector3Int _posID)
        {
            blocks[_posID.x, _posID.z, _posID.y] = null;
        }

        public n_block.BlockLogic GetBlock(Vector3Int _posID)
        {
            return blocks[_posID.x, _posID.z, _posID.y];
        }


        



    }





}


