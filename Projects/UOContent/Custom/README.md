# ModernUO Britain银行任务系统

## 项目描述
这是一个为 ModernUO 服务器创建的自定义任务系统，包含一个位于Britain银行门口的技能石柱。

## 文件结构
```
Custom/
├── Items/
│   └── SkillStone.cs          # 技能石柱物品脚本
├── Quests/
│   └── BritainBankQuest.cs    # Britain银行任务脚本
├── BritainBankQuestSetup.cs   # 初始化脚本
└── README.md                   # 文档
```

## 功能说明

### 1. 技能石柱 (SkillStone.cs)
- **物品ID**: 0x1EA7（蓝色灯柱）
- **功能**: 双击后将玩家所有技能提升到指定数值（默认100）
- **属性**:
  - `TargetSkillValue`: 目标技能值（可通过GM命令调整）
  - 放置在Britain银行门口
  - 不可移动

### 2. Britain银行任务 (BritainBankQuest.cs)
- **任务名**: Britain银行之旅
- **目标**: 前往Britain银行门口使用技能石柱
- **奖励**: 1000金币
- **任务给予者**: 任务管理员NPC

### 3. 初始化脚本 (BritainBankQuestSetup.cs)
- 在服务器启动时自动生成技能石柱和任务给予者
- 坐标: Britain 1476, 1628, 10（可根据需要调整）

## 安装步骤

1. 将所有文件放入 `d:\ModernUO\Distribution\Custom\` 目录
2. 在服务器启动脚本中调用 `BritainBankQuestSetup.Initialize()` 方法
3. 重启服务器

## 使用方式

### 玩家操作
1. 找到任务给予者NPC并接受任务
2. 前往Britain银行门口
3. 双击技能石柱
4. 所有技能值将提升到100

### GM命令
- 创建技能石柱: `[create SkillStone`
- 设置目标技能值: `[TargetSkillValue 100`
- 删除物品: `[remove`

## 坐标调整
如果需要更改任务位置，编辑 `BritainBankQuestSetup.cs` 中的坐标：
```csharp
Point3D location = new Point3D(1476, 1628, 10); // 修改为目标坐标
```

## 扩展功能建议
- 添加任务追踪系统
- 实现NPC对话菜单
- 添加多个难度等级
- 创建每日任务变体
- 添加成就系统

## 版本信息
- ModernUO 版本: 最新稳定版
- C# 版本: .NET 5.0+
- 创建日期: 2026-01-29
