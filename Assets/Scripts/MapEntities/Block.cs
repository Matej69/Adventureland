using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace n_block
{

    public static class BLOCK_SIZE {
        public const int SIZE = 1;
    }

    public enum E_BLOCK {
        GRASS,
        GROUND,
        SIZE
    }


    //*** USED FOR LOGIC ARRAYS -> DATA FOR EACH BLOCK THAT IS TAKING AS SMALL AMOUNT MEMORY AS IT NEEDS
    //*** FROM LOGIC BLOCK WE WILL BE CREATING BLOCK OBJECTS
    //*** EACH BLOCK NEEDS TO KNOW ITS POSITION IN BlockLogic[x,y,z] AND SINCE EACH CHUNK WILL BE 50X50X100, 3 BYTES WILL BE ENOUGH
    public class BlockLogic
    {
        public byte id { private set; get; }
        public Vector3Int cord { private set; get; }
        public bool isVisible = false;
        
        public BlockLogic(Vector3Int _cord, byte _id)
        {
            cord = _cord;
            id = _id;
            isVisible = false;
        }
    }





    public class BlockInfo
    {
        public E_BLOCK id { private set; get; }
        public int durability { private set; get; }
        public int price { private set; get; }
        public Material material { private set; get; }


        public BlockInfo(E_BLOCK _id, int _dur, int _price, Material _mat)
        {
            id = _id;
            durability = _dur;
            price = _price;
            material = _mat;
        } 
    }





    public class BlockInfoDatabase
    {
        private static bool dictionaryFilled = false;

        private static Dictionary<E_BLOCK, BlockInfo> blocksInfo = new Dictionary<E_BLOCK, BlockInfo>();
              

        public static BlockInfo GetBlockInfo(E_BLOCK _id)
        {
            //if it's the first time we are getting blocks, fill blockInfo dictionary
            //if it is not skip filling dictionary
            if(!dictionaryFilled)
            {
                FillBlocksDictionary();
                dictionaryFilled = true;
            }

            foreach (KeyValuePair<E_BLOCK, BlockInfo> pair in blocksInfo)
                if (pair.Key == _id)
                    return pair.Value;
            Debug.LogError("CAN NOT GET BlockInfo with ID = " + _id + " FROM DICTIONARY");
            return null;
        }


        private static void FillBlocksDictionary()
        {
            blocksInfo.Add(E_BLOCK.GROUND, new BlockInfo(E_BLOCK.GROUND, 2, 1, LoadMaterial("ground")));
            blocksInfo.Add(E_BLOCK.GRASS, new BlockInfo(E_BLOCK.GRASS, 3, 2, LoadMaterial("grass")));
        }

        private static Material LoadMaterial(string _fileName)
        {
            return (Material)Resources.Load("Materials/" + _fileName, typeof(Material));
        }
    }








}






