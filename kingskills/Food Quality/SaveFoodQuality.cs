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
        public string flavorText;

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
            serial += chefID.ToString() + "D";
            serial += flavorText + "^";

            //Jotunn.Logger.LogMessage(serial);
            return serial;
        }

        public override void Deserialize(string data)
        {
            //Jotunn.Logger.LogWarning($"data has {data.Count()} elements]");
            int i = 0;


            string foodQualitySerial = DeserializeField('A', data, ref i);
            if (!float.TryParse(foodQualitySerial, out foodQuality))
                Jotunn.Logger.LogWarning("Failed to parse foodquality");


            string countSerial = DeserializeField('B', data, ref i);
            int count = 0;
            if (!int.TryParse(countSerial, out count))
                Jotunn.Logger.LogWarning("Failed to parse buff count");


            int j = 0;
            while (j < count)
            {
                int buffID = 0;
                string buffIDSerial = DeserializeField('C', data, ref i);

                if (!int.TryParse(buffIDSerial, out buffID))
                    Jotunn.Logger.LogWarning("Failed to parse a buff");

                buffs.Add((BuffType)buffID);

                j++;
            }


            string chefIDSerial = DeserializeField('D', data, ref i);
            if (!long.TryParse(chefIDSerial, out chefID))
                Jotunn.Logger.LogWarning("Failed to parse chefID");


            string flavorTextSerial = DeserializeField('^', data, ref i);
            flavorText = flavorTextSerial;
        }

        public static string DeserializeField(char key, string data, ref int i)
        {
            string poppedData = "";
            bool go = true;

            while (go)
            {
                if (i >= data.Count()) break;

                if (data[i] != key)
                {
                    poppedData += data[i];
                    //Jotunn.Logger.LogWarning($"[{poppedData}]");
                }
                else
                {
                    go = false;
                }
                i++;
                //Jotunn.Logger.LogWarning($"parsing array index: [{i}]");
            }
            return poppedData;
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
