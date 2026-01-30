using Server;
using Server.Items;
using Server.Mobiles;
using Server.Quests;

namespace Server.Scripts.Custom
{
    public class BritainBankQuestSetup
    {
        public static void Initialize()
        {
            // 在初始化时调用此方法来设置Britain银行的技能石柱
            CreateSkillStoneAtBank();
            CreateQuestGiver();
        }

        private static void CreateSkillStoneAtBank()
        {
            // Britain银行门口坐标（近似值，可根据需要调整）
            // 大陆：Trammel
            Map map = Map.Trammel;
            Point3D location = new Point3D(1476, 1628, 10); // Britain银行门口

            SkillStone stone = new SkillStone
            {
                TargetSkillValue = 100, // 设置技能增强到100
                Map = map,
                Location = location
            };

            World.AddEntity(stone);
            World.Broadcast(0x0482, false, "技能石柱已在Britain银行门口生成！");
        }

        private static void CreateQuestGiver()
        {
            // 在Britain城镇中心创建任务给予者
            Map map = Map.Trammel;
            Point3D location = new Point3D(1474, 1626, 10);

            BritainBankQuestGiver giver = new BritainBankQuestGiver
            {
                Map = map,
                Location = location,
                Direction = Direction.South
            };

            World.AddEntity(giver);
        }
    }
}
