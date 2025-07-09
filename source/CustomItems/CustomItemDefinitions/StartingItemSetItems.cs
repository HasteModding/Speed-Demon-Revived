using CustomItemLib;
using Landfall.Haste;

namespace SpeedDemon.CustomItems.CustomItemDefinitions
{
    internal class StartingItemSetItems
    {
        internal static void LoadStartingItemSetItems()
        {
            ItemFactory.AddItemToDatabase( // s0
                itemName: "SD_SIS_Boost",
                rarity: Rarity.Epic,
                title: new UnlocalizedString("Glass Cannon"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("<color=#d83bff>Plutonium Ring</color> and <color=#089e00>Golden Necklace</color>\nFocus: Boost at a Cost"),
                usesEffectDescription: true
            );
            ItemLoader.CopyDefaultMeshes("SD_SIS_Boost", "PermanentBoost");

            ItemFactory.AddItemToDatabase( // s1
                itemName: "SD_SIS_Luck",
                rarity: Rarity.Legendary,
                title: new UnlocalizedString("Entrepreneur"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("<color=#f0980c>Enchanted Coin</color> and <color=#089e00>Stimulants</color>\nFocus: Luck and Spark Generation"),
                usesEffectDescription: true
            );
            ItemLoader.CopyDefaultMeshes("SD_SIS_Luck", "RandomChanceToGetLuck");

            ItemFactory.AddItemToDatabase( // s2
                itemName: "SD_SIS_SelfDamage",
                rarity: Rarity.Epic,
                title: new UnlocalizedString("Berserker"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("<color=#d83bff>Painful Coil</color> and <color=#089e00>Leather Chest</color>\nFocus: Low Health"),
                usesEffectDescription: true
            );
            ItemLoader.CopyDefaultMeshes("SD_SIS_SelfDamage", "BoostPerMissingHealth");

            ItemFactory.AddItemToDatabase( // s3
                itemName: "SD_SIS_NoItems",
                rarity: Rarity.Common,
                title: new UnlocalizedString("Deprived"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("<color=#ff0000>No Items</color>\nFor the OG Speed Demon Experience."),
                usesEffectDescription: true
            );
            ItemLoader.CopyDefaultMeshes("SD_SIS_NoItems", "LuckPerMissingHealth");
        }

        public class StartingItemSet
        {
            public ItemInstance displayItem;
            public List<string> itemNames;

            public StartingItemSet(ItemInstance displayItem, List<string> itemNames)
            {
                this.displayItem = displayItem;
                this.itemNames = itemNames;
            }
        }

        internal static void LoadStartingItemSets()
        {
            RewardOverride.StartingItems.startingItemSets.Add(
                new StartingItemSet(
                    displayItem: ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_SIS_Boost"),
                    itemNames: new List<string> { "SD_EpicPermanentBoost", "SD_PermanentBoost" }
                )
            );

            RewardOverride.StartingItems.startingItemSets.Add(
                new StartingItemSet(
                    displayItem: ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_SIS_Luck"),
                    itemNames: new List<string> { "SD_RandomChanceToGetLuck", "SD_LuckAndBoost" }
                )
            );

            RewardOverride.StartingItems.startingItemSets.Add(
                new StartingItemSet(
                    displayItem: ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_SIS_SelfDamage"),
                    itemNames: new List<string> { "SD_BoostPerMissingHealth", "SD_MaxHealthAndBoost" }
                )
            );

            RewardOverride.StartingItems.startingItemSets.Add(
                new StartingItemSet(
                    displayItem: ItemDatabase.instance.items.FirstOrDefault(e => e.name == "SD_SIS_NoItems"),
                    itemNames: new List<string> {}
                )
            );
        }
    }
}
