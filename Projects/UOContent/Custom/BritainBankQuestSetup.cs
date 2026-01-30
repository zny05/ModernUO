using Server;
using Server.Items;
using Server.Mobiles;
using Server.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Scripts.Custom
{
    public class BritainBankQuestSetup
    {
        private static readonly Point3D SkillStoneLocation = new Point3D(1476, 1628, 10);
        private static readonly Point3D QuestGiverLocation = new Point3D(1474, 1626, 10);
        private const int SearchRadius = 5;

        public static void Initialize()
        {
            // 在初始化时调用此方法来设置Britain银行的技能石柱
            CreateSkillStoneAtBank();
            CreateQuestGiver();
        }

        private static void CreateSkillStoneAtBank()
        {
            Map map = Map.Trammel;

            // 检查该位置是否已经存在技能石柱
            if (FindExistingSkillStone(map))
            {
                return; // 已存在，跳过创建
            }

            SkillStone stone = new SkillStone
            {
                TargetSkillValue = 100, // 设置技能增强到100
                Map = map,
                Location = SkillStoneLocation
            };

            try
            {
                World.AddEntity(stone);
                World.Broadcast(0x0482, false, "技能石柱已在Britain银行门口生成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[错误] 无法创建技能石柱: {ex.Message}");
                stone.Delete();
            }
        }

        private static void CreateQuestGiver()
        {
            Map map = Map.Trammel;

            // 检查该位置是否已经存在任务给予者
            if (FindExistingQuestGiver(map))
            {
                return; // 已存在，跳过创建
            }

            try
            {
                BritainBankQuestGiver giver = new BritainBankQuestGiver
                {
                    Map = map,
                    Location = QuestGiverLocation,
                    Direction = Direction.South
                };

                World.AddEntity(giver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[错误] 无法创建任务给予者: {ex.Message}");
            }
        }

        private static bool FindExistingSkillStone(Map map)
        {
            // 搜索指定范围内的技能石柱
            foreach (Item item in map.GetItemsInRange(SkillStoneLocation, SearchRadius))
            {
                if (item is SkillStone)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool FindExistingQuestGiver(Map map)
        {
            // 搜索指定范围内的任务给予者
            foreach (Mobile mobile in map.GetMobilesInRange(QuestGiverLocation, SearchRadius))
            {
                if (mobile is BritainBankQuestGiver)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
