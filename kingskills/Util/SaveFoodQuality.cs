using ExtendedItemDataFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static kingskills.StackableItems;

namespace kingskills
{
    public class SaveFoodQuality : BaseExtendedItemComponent, ExtendedItemUnique<SaveFoodQuality>
    {
        public float foodQuality;
        public List<BuffType> buffs;
        public long chefID;

        public enum BuffType
        {
            Spicy, Sweet
        }

        public SaveFoodQuality (ExtendedItemData parent)
            : base(typeof(SaveFoodQuality).AssemblyQualifiedName, parent)
        {
            buffs = new List<BuffType>();
        }

        public override string Serialize()
        {
            string serial = foodQuality.ToString() + "A"; 
            serial += buffs.Count + "B";
            foreach (BuffType buff in buffs)
            {
                serial += buff + "C";
            }
            serial += chefID + "D";

            //Jotunn.Logger.LogMessage(serial);
            return serial;
        }

        public override void Deserialize(string data)
        {
            string poppedData = "";
            int i = 0;
            bool go = true;

            while (go)
            {
                if (data[i] != 'A')
                {
                    poppedData += data[i];
                }
                else
                {
                    go = false;
                }
                i++;
            }

            if (!float.TryParse(poppedData, out foodQuality))
                Jotunn.Logger.LogWarning("Failed to parse foodquality");

            //Jotunn.Logger.LogMessage($"parsed out food quality of [{poppedData}]");
            poppedData = "";
            go = true;

            while (go)
            {
                if (data[i] != 'B')
                {
                    poppedData += data[i];
                }
                else
                {
                    go = false;
                }
                i++;
            }

            //Jotunn.Logger.LogMessage($"parsed out array count of [{poppedData}]");
            int count = 0;
            if (!int.TryParse(poppedData, out count))
                Jotunn.Logger.LogWarning("Failed to parse buff count");
            int j = 0;
            int buffID = 0;

            while (j < count)
            {
                go = true;
                poppedData = "";
                while (go)
                {
                    if (data[i] != 'C')
                    {
                        poppedData += data[i];
                    }
                    else
                    {
                        go = false;
                    }
                    i++;
                }
                //Jotunn.Logger.LogMessage($"popped out a buff type of [{poppedData}]");
                if (!int.TryParse(poppedData, out buffID))
                    Jotunn.Logger.LogWarning("Failed to parse a buff");
                buffs.Add((BuffType)buffID);

                j++;
            }

            poppedData = "";
            go = true;

            while (go)
            {
                if (data.Count() == i) break;
                if (data[i] != 'D')
                {
                    poppedData += data[i];
                }
                else
                {
                    go = false;
                }
                i++;
            }

            //Jotunn.Logger.LogMessage($"parsed out chef ID of [{poppedData}]");
            if (!long.TryParse(poppedData, out chefID))
                Jotunn.Logger.LogWarning("Failed to parse chefID");
        }

        public override BaseExtendedItemComponent Clone() => (BaseExtendedItemComponent)MemberwiseClone();

        public bool Equals(SaveFoodQuality other)
        {
            return foodQuality == other.foodQuality && buffs.Equals(other.buffs) && chefID == other.chefID;
        }

        ~SaveFoodQuality()
        {
            buffs.Clear();
        }
    }
}
