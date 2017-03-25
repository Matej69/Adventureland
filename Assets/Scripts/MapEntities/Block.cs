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

        public void SetProperties(byte _id, Vector3Int _cord, bool _isVisible) { id = _id; cord = _cord; isVisible = _isVisible; }
        
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
            blocksInfo.Add(E_BLOCK.GRASS, new BlockInfo(E_BLOCK.GRASS, "grass", 1, 1, LoadMaterial("grass")));
            blocksInfo.Add(E_BLOCK.GROUND, new BlockInfo(E_BLOCK.GROUND, "ground", 2, 2, LoadMaterial("ground")));
            blocksInfo.Add(E_BLOCK.STONE, new BlockInfo(E_BLOCK.STONE, "stone", 4, 2, LoadMaterial("stone")));
            blocksInfo.Add(E_BLOCK.SAND, new BlockInfo(E_BLOCK.SAND, "sand", 4, 2, LoadMaterial("sand")));
            blocksInfo.Add(E_BLOCK.COAL, new BlockInfo(E_BLOCK.COAL, "coal", 8, 2, LoadMaterial("coal")));
            blocksInfo.Add(E_BLOCK.METAL, new BlockInfo(E_BLOCK.METAL, "metal", 30, 2, LoadMaterial("metal")));
            blocksInfo.Add(E_BLOCK.IRON, new BlockInfo(E_BLOCK.IRON, "iron", 15, 2, LoadMaterial("iron")));
            blocksInfo.Add(E_BLOCK.GOLD, new BlockInfo(E_BLOCK.GOLD, "gold", 50, 2, LoadMaterial("gold")));
            blocksInfo.Add(E_BLOCK.DIAMOND, new BlockInfo(E_BLOCK.DIAMOND, "diamond", 100, 2, LoadMaterial("diamond")));
            blocksInfo.Add(E_BLOCK.RUBY, new BlockInfo(E_BLOCK.RUBY, "ruby", 150, 2, LoadMaterial("ruby")));
            blocksInfo.Add(E_BLOCK.EMERALD, new BlockInfo(E_BLOCK.EMERALD, "emerald", 200, 2, LoadMaterial("emerald")));
            blocksInfo.Add(E_BLOCK.LAPIS, new BlockInfo(E_BLOCK.LAPIS, "lapis", 250, 2, LoadMaterial("lapis")));
        }

        private static Material LoadMaterial(string _fileName)
        {
            return (Material)Resources.Load("Materials/" + _fileName, typeof(Material));
        }
    }










    public class BlockPossibilityRange
    {
        public int from, to;
        public BlockPossibilityRange(int _from, int _to)
        {
            from = _from;
            to = _to;
        }
    }

    public class BlockOccurrenceInfo
    {
        public int level;
        public Dictionary<E_BLOCK, BlockPossibilityRange> blocksOccurrence;

        public BlockOccurrenceInfo(int _lvl, Dictionary<E_BLOCK, BlockPossibilityRange> _blocksOccurrence)
        {
            level = _lvl;
            blocksOccurrence = _blocksOccurrence;
        }

    }


    public class BlockOccurrenceDatabase
    {
        private static bool dictionaryFilled = false;
        private static List<BlockOccurrenceInfo> occurrencesInfos = new List<BlockOccurrenceInfo>();

        private static int NUM_OF_LVLS = 7;

        public static Dictionary<E_BLOCK, BlockPossibilityRange> GetOccurrencesInfo(int _lvl)
        {
            //if it's the first time we are getting block occurrence, fill blocksOccurrence dictionary
            //if it is not skip filling dictionary
            if (!dictionaryFilled)
            {
                FillBlockOccurrenceDictionary();
                dictionaryFilled = true;
            }

            foreach(BlockOccurrenceInfo info in occurrencesInfos)
            {
                if (info.level == _lvl)
                    return info.blocksOccurrence;
            }
           
            Debug.LogError("CAN NOT GET BLOCK INFO DICTIONARY with LEVEL = " + _lvl + " FROM DICTIONARY");
            return null;
        }

        public static E_BLOCK GenerateRandomBlock(int _lvl)
        {
            int rand = Random.Range(0, 100);
            
            foreach (KeyValuePair<E_BLOCK, BlockPossibilityRange> pair in GetOccurrencesInfo(_lvl))
            {
                if (rand >= pair.Value.from && rand <= pair.Value.to)
                    return pair.Key;
            }

            Debug.LogError("CAN NOT GENERATE BLOCK FOR LEVEL = " + _lvl + " SO DEFAULT WILL BE E_BLOCK.GROUND");
            return E_BLOCK.GROUND;
        }

        public static int GetLevelFromDepth(int _depth)
        {
            int maxDepth = n_chunk.CHUNK_SIZE.Y;
            return (_depth / (maxDepth / NUM_OF_LVLS) );
        }

        private static void FillBlockOccurrenceDictionary()
        {
            occurrencesInfos.Add(new BlockOccurrenceInfo(0, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.DIAMOND, new BlockPossibilityRange(0,50) }, { E_BLOCK.GROUND, new BlockPossibilityRange(50, 100) } }) );
            occurrencesInfos.Add(new BlockOccurrenceInfo(1, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.STONE, new BlockPossibilityRange(0, 10) }, { E_BLOCK.GROUND, new BlockPossibilityRange(10, 80) }, { E_BLOCK.METAL, new BlockPossibilityRange(80, 90) }, { E_BLOCK.IRON, new BlockPossibilityRange(90, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(2, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.STONE, new BlockPossibilityRange(0, 30) }, { E_BLOCK.SAND, new BlockPossibilityRange(30, 65) }, { E_BLOCK.COAL, new BlockPossibilityRange(65, 85) }, { E_BLOCK.METAL, new BlockPossibilityRange(85, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(3, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.GOLD, new BlockPossibilityRange(0, 5) }, { E_BLOCK.SAND, new BlockPossibilityRange(5, 80) }, { E_BLOCK.COAL, new BlockPossibilityRange(80, 95) }, { E_BLOCK.LAPIS, new BlockPossibilityRange(95, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(4, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.GOLD, new BlockPossibilityRange(0, 10) }, { E_BLOCK.GROUND, new BlockPossibilityRange(10, 50) }, { E_BLOCK.COAL, new BlockPossibilityRange(50, 90) }, { E_BLOCK.LAPIS, new BlockPossibilityRange(90, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(5, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.GOLD, new BlockPossibilityRange(0, 15) }, { E_BLOCK.COAL, new BlockPossibilityRange(15, 90) }, { E_BLOCK.LAPIS, new BlockPossibilityRange(90, 95) }, { E_BLOCK.DIAMOND, new BlockPossibilityRange(95, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(6, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.RUBY, new BlockPossibilityRange(0, 5) }, { E_BLOCK.METAL, new BlockPossibilityRange(5, 80) }, { E_BLOCK.LAPIS, new BlockPossibilityRange(80, 85) }, { E_BLOCK.DIAMOND, new BlockPossibilityRange(85, 100) } }));
            occurrencesInfos.Add(new BlockOccurrenceInfo(7, new Dictionary<E_BLOCK, BlockPossibilityRange> { { E_BLOCK.RUBY, new BlockPossibilityRange(0, 20) }, { E_BLOCK.IRON, new BlockPossibilityRange(20, 65) }, { E_BLOCK.DIAMOND, new BlockPossibilityRange(65, 85) }, { E_BLOCK.EMERALD, new BlockPossibilityRange(85, 100) } }));
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





    public class BlockText
    {
        private static GameObject blockText;
        
        public static GameObject GetPrefab()
        {
            return ((blockText != null) ? blockText : blockText = (GameObject)Resources.Load("Objects/Blocks/Block3DText") );
        }

    }

    





}






