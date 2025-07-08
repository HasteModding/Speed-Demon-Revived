using CustomItemLib;
using Landfall.Haste;

namespace SpeedDemon.CustomItems.CustomItemDefinitions
{
    internal class UtilityItems
    {
        internal static void LoadUtilityItems()
        {
            ItemFactory.AddItemToDatabase( // u0
                itemName: "SD_UI_Refresh",
                title: new UnlocalizedString("Refresh"),
                flavorText: new UnlocalizedString("There are plenty more where that came from."),
                triggerType: ItemTriggerType.BoughtItem,
                description: new UnlocalizedString("<color=#c896fa>Refresh</color> reward, Lose <color=#e2b96b>250</color> <color=#c896fa>Sparks</color>"),
                usesEffectDescription: true
            );
            ItemLoader.CopyDefaultMeshes("SD_UI_Refresh", "LuckPerMissingHealth");

            ItemFactory.AddItemToDatabase( // u1
                itemName: "SD_UI_StartingItemsLeft",
                title: new UnlocalizedString("Previous Class"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("Go to the previous selection"),
                usesEffectDescription: true
            );
            //ItemLoader.CopyDefaultMeshes("SD_UI_StartingItemsLeft", "SparksOnPerfectLanding");

            ItemFactory.AddItemToDatabase( // u2
                itemName: "SD_UI_StartingItemsRight",
                title: new UnlocalizedString("Next Class"),
                flavorText: new UnlocalizedString("<size=0>You shouldn't be able to see this</size>"),
                description: new UnlocalizedString("Go to the next selection"),
                usesEffectDescription: true
            );
            //ItemLoader.CopyDefaultMeshes("SD_UI_StartingItemsRight", "SparksOnPerfectLanding");
        }
    }
}
