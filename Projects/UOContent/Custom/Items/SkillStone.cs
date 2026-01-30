using Server;
using Server.Gumps;
using Server.Network;
using System;

namespace Server.Items
{
    public class SkillStone : Item
    {
        private int m_TargetSkillValue = 100; // 技能目标值

        [Constructible]
        public SkillStone() : base(0x1EA7)
        {
            Name = "技能石柱";
            Hue = 0x0481; // 青色
            Weight = 0;
            Movable = false;
        }

        public SkillStone(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TargetSkillValue
        {
            get { return m_TargetSkillValue; }
            set { m_TargetSkillValue = Math.Max(0, value); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ItemShape
        {
            get { return ItemID; }
            set { ItemID = value; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 3))
            {
                from.SendLocalizedMessage(500446); // 太远了
                return;
            }

            // 增加玩家所有技能
            IncreaseAllSkills(from);

            from.SendMessage(0x0482, $"技能石柱强化了你的所有技能到 {m_TargetSkillValue} 点！");
            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x373A, 15);
        }

        private void IncreaseAllSkills(Mobile m)
        {
            foreach (Skill skill in m.Skills)
            {
                if (skill.Base < m_TargetSkillValue)
                {
                    skill.Base = m_TargetSkillValue;
                }
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_TargetSkillValue);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_TargetSkillValue = reader.ReadInt();
        }
    }
}
