# ModernUO F1Stats 更新备忘录

本文档用于以后同步官方 ModernUO 并自动重新应用 F1 改动，减少手工冲突与部署失误。

## 一次性准备

- 仓库目录：`~/下载/ModernUO/ModernUO`
- 确认已存在脚本与补丁：
  - `scripts/reapply-f1stats.sh`
  - `scripts/patches/f1stats-uogateway.patch`
- 确认本机可发布：
  - `dotnet --version`
  - `./publish.sh release linux x64`

## 每次更新（推荐自动流程）

### 1) 执行一键更新脚本

```bash
cd ~/下载/ModernUO/ModernUO
./scripts/reapply-f1stats.sh
```

脚本会自动做：
- 拉取官方 `origin/main`
- 重建功能分支（默认 `feature/f1stats-modernuo`）
- 自动应用 F1 patch
- 自动执行发布验证 `./publish.sh release linux x64`

### 2) 可选：自动提交开关

```bash
AUTO_COMMIT=1 COMMIT_MESSAGE="feat: reapply f1stats patch" ./scripts/reapply-f1stats.sh
```

## 若补丁冲突

脚本会先尝试普通 apply，再尝试 3-way。若仍失败，手工处理文件：

- `Projects/UOContent/Network/UOGateway.cs`

处理后重新发布验证：

```bash
./publish.sh release linux x64
```

## 推送到你的 fork

```bash
git push -u myfork feature/f1stats-modernuo
```

如需覆盖你 fork 的 `main`（谨慎）：

```bash
git push myfork feature/f1stats-modernuo:main
```

## NAS 部署（两种）

### A. 仅脚本更新（不重建镜像，推荐日常）

```bash
rsync -av --delete ~/下载/ModernUO/ModernUO/Distribution/Assemblies/ <nas_user>@<nas_ip>:/vol2/1000/docker/servuo/modernuo/assemblies/
ssh <nas_user>@<nas_ip> 'cd /vol2/1000/docker/servuo/modernuo && docker compose restart modernuo'
ssh <nas_user>@<nas_ip> 'docker logs --tail 200 modernuo'
```

### B. 需要重建镜像时

```bash
docker buildx build --builder muo-builder --platform linux/amd64 -t modernuo:latest --build-arg TARGETARCH=amd64 --output type=docker,dest=modernuo_latest_amd64.tar .
scp modernuo_latest_amd64.tar <nas_user>@<nas_ip>:/path/on/nas/
ssh <nas_user>@<nas_ip> 'docker load -i /path/on/nas/modernuo_latest_amd64.tar && cd /vol2/1000/docker/servuo/modernuo && docker compose up -d --force-recreate'
```

## 运行稳定性检查清单

- `config/modernuo.json` 的 `settings` 值全部是字符串。
- `network.encryptionMode` 使用有效值（建议 `Both`）。
- `uogateway.enabled`、`ClassicUO.FreeshardStats.SendRawUtf8` 已开启。
- `docker-compose.yml` 保留 `security_opt`（io_uring 场景）。
- CPU 优化参数存在：
  - `COMPlus_gcServer=0`
  - `COMPlus_GCConcurrent=0`
  - `DOTNET_ThreadPool_UnusedThreadsThreshold=1`

## F1 检测快速验证

- 使用官方 `F1STATS.PS1`（seed + 0xF1 + 0xFF）
- 期望收到 JSON：`clients/items/chars/age/memory`
- `age` 应递增（秒级）
