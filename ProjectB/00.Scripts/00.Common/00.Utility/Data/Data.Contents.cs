using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	#region Item
	[Serializable]
	public class ItemDataJson
	{
		public string itemName; //templateID				
		public string rarity;
		public string type;
	}

	[Serializable]
	public class ItemDataLoader : ILoader<string, ItemDataJson>
	{
		public List<ItemDataJson> weapons = new List<ItemDataJson>();

		public Dictionary<string, ItemDataJson> MakeDict()
		{
			Dictionary<string, ItemDataJson> dict = new Dictionary<string, ItemDataJson>();

			foreach (ItemDataJson item in weapons)
			{
				dict.Add(item.itemName, item);
			}
			return dict;
		}
	}

    #endregion


    #region HairColor
    [Serializable]
    public class HairColor
    {
        public string hairName;
        public string color;
    }

    [Serializable]
    public class HairColorLoader : ILoader<string, List<HairColor>>
    {
        public List<HairColor> PCK_Hair_001 = new List<HairColor>();
        public List<HairColor> PCK_Hair_002 = new List<HairColor>();
        public List<HairColor> PCK_Hair_003 = new List<HairColor>();

        public Dictionary<string, List<HairColor>> MakeDict()
        {
            Dictionary<string, List<HairColor>> dict = new Dictionary<string, List<HairColor>>();

            List<HairColor> list1 = new List<HairColor>();
            List<HairColor> list2 = new List<HairColor>();
            List<HairColor> list3 = new List<HairColor>();

            foreach (HairColor item in PCK_Hair_001)
            {
                list1.Add(item);
            }

            foreach (HairColor item in PCK_Hair_002)
            {
                list2.Add(item);
            }

            foreach (HairColor item in PCK_Hair_003)
            {
                list3.Add(item);
            }

            dict.Add(PCK_Hair_001[0].hairName, list1);
            dict.Add(PCK_Hair_002[0].hairName, list1);
            dict.Add(PCK_Hair_003[0].hairName, list1);
            return dict;
        }
    }
    #endregion
}