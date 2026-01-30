using Server;
using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Quests
{
    public class BritainBankQuest : QuestSystem
    {
        private static readonly Type[] m_TypeReferenceTable =
        {
            typeof(BritainBankObjective)
        };

        public BritainBankQuest(PlayerMobile from) : base(from)
        {
            Objectives.Add(new BritainBankObjective());
        }

        public BritainBankQuest()
        {
        }

        public override Type[] TypeReferenceTable => m_TypeReferenceTable;

        public override object Name => "Britain银行之旅";

        public override object OfferMessage => "前往Britain银行门口，找到技能石柱并双击它来强化你的技能。";

        public override bool IsTutorial => false;
        public override TimeSpan RestartDelay => TimeSpan.Zero;
        public override int Picture => 0x0;

        public override void Accept()
        {
            base.Accept();
            From.SendMessage(0x0482, "你接受了任务。前往Britain银行门口找到技能石柱！");
            From.SendMessage(0x0482, "坐标提示：Britain银行在Britain城镇中心附近。");
        }

        public override void ChildDeserialize(IGenericReader reader)
        {
            var version = reader.ReadEncodedInt();
        }

        public override void ChildSerialize(IGenericWriter writer)
        {
            writer.WriteEncodedInt(0);
        }
    }

    public class BritainBankObjective : QuestObjective
    {
        public override int Message => 0; // Placeholder

        public BritainBankObjective()
        {
        }
    }

    public class BritainBankQuestGiver : BaseCreature
    {
        [Constructible]
        public BritainBankQuestGiver() : base(AIType.AI_Vendor, FightMode.None)
        {
            Name = "任务管理员";
            Body = 0x190; // 人类男性
            Hue = 0x83F8;
            Race = Race.Human;

            // 衣服
            AddItem(new Robe(0x047E));

            SetStr(100);
            SetDex(100);
            SetInt(100);

            SetSkill(SkillName.Parry, 50);
            SetSkill(SkillName.Swords, 50);
            SetSkill(SkillName.Tactics, 50);
        }

        public BritainBankQuestGiver(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            // 这里可以添加对话逻辑
            from.SendMessage("你好，我可以给你一个任务。前往Britain银行门口并使用技能石柱。");
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
