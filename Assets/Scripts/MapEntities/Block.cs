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
        STONE,
        SAND,
        COAL,
        METAL,
        IRON,
        GOLD,
        DIAMOND,
        RUBY,
        EMERALD,
        LAPIS,
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
        public string name { private set; get; }
        public int durability { private set; get; }
        public int price { private set; get; }
        public Material material { private set; get; }


        public BlockInfo(E_BLOCK _id, string _name, int _dur, int _price, Material _mat)
        {            
            id = _id;
            name = _name;
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
            blocksInfo.Add(E_BLOCK.GRASS, new BlockInfo(E_BLOCK.GRASS, "grass", 2, 1, LoadMaterial("grass")));
            blocksInfo.Add(E_BLOCK.GROUND, new BlockInfo(E_BLOCK.GROUND, "ground", 3, 2, LoadMaterial("ground")));
            blocksInfo.Add(E_BLOCK.STONE, new BlockInfo(E_BLOCK.STONE, "stone", 3, 2, LoadMaterial("stone")));
            blocksInfo.Add(E_BLOCK.SAND, new BlockInfo(E_BLOCK.SAND, "sand", 3, 2, LoadMaterial("sand")));
            blocksInfo.Add(E_BLOCK.COAL, new BlockInfo(E_BLOCK.COAL, "coal", 3, 2, LoadMaterial("coal")));
            blocksInfo.Add(E_BLOCK.METAL, new BlockInfo(E_BLOCK.METAL, "metal", 3, 2, LoadMaterial("metal")));
            blocksInfo.Add(E_BLOCK.IRON, new BlockInfo(E_BLOCK.IRON, "iron", 3, 2, LoadMaterial("iron")));
            blocksInfo.Add(E_BLOCK.GOLD, new BlockInfo(E_BLOCK.GOLD, "gold", 3, 2, LoadMaterial("gold")));
            blocksInfo.Add(E_BLOCK.DIAMOND, new BlockInfo(E_BLOCK.DIAMOND, "diamond", 3, 2, LoadMaterial("diamond")));
            blocksInfo.Add(E_BLOCK.RUBY, new BlockInfo(E_BLOCK.RUBY, "ruby", 3, 2, LoadMaterial("ruby")));
            blocksInfo.Add(E_BLOCK.EMERALD, new BlockInfo(E_BLOCK.EMERALD, "emerald", 3, 2, LoadMaterial("emerald")));
            blocksInfo.Add(E_BLOCK.LAPIS, new BlockInfo(E_BLOCK.LAPIS, "lapis", 3, 2, LoadMaterial("lapis")));
        }

        private static Material LoadMaterial(string _fileName)
        {
            return (Material)Resources.Load("Materials/" + _fileName, typeof(Material));
        }
    }
    


    public class BlockSprites
    {
        private static bool dictionaryFilled = false;
        private static Dictionary<E_BLOCK, Sprite> blockSprites = new Dictionary<E_BLOCK, Sprite>();

         public static Sprite GetBlockSprite(E_BLOCK _id)
        {
            //if it's the first time we are getting blocks, fill blockInfo dictionary
            //if it is not skip filling dictionary
            if(!dictionaryFilled)
            {
                FillBlocksDictionary();
                dictionaryFilled = true;
            }

            foreach (KeyValuePair<E_BLOCK, Sprite> pair in blockSprites)
                if (pair.Key == _id)
                    return pair.Value;
            Debug.LogError("CAN NOT GET Sprite with ID = " + _id + " FROM DICTIONARY");
            return null;
        }


        private static void FillBlocksDictionary()
        {
            blockSprites.Add(E_BLOCK.GROUND, LoadSprite("ground"));
            blockSprites.Add(E_BLOCK.GRASS, LoadSprite("grass"));
            blockSprites.Add(E_BLOCK.STONE, LoadSprite("stone"));
            blockSprites.Add(E_BLOCK.SAND, LoadSprite("sand"));
            blockSprites.Add(E_BLOCK.COAL, LoadSprite("coal"));
            blockSprites.Add(E_BLOCK.METAL, LoadSprite("metal"));
            blockSprites.Add(E_BLOCK.IRON, LoadSprite("iron"));
            blockSprites.Add(E_BLOCK.GOLD, LoadSprite("gold"));
            blockSprites.Add(E_BLOCK.DIAMOND, LoadSprite("diamond"));
            blockSprites.Add(E_BLOCK.RUBY, LoadSprite("ruby"));
            blockSprites.Add(E_BLOCK.EMERALD, LoadSprite("emerald"));
            blockSprites.Add(E_BLOCK.LAPIS, LoadSprite("lapis"));
        }

        private static Sprite LoadSprite(string _fileName)
        {
            return (Sprite)Resources.Load("Sprites/Blocks/" + _fileName, typeof(Sprite));
        }
    }

    





}






