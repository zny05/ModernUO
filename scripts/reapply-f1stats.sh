#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PATCH_FILE="$ROOT_DIR/scripts/patches/f1stats-uogateway.patch"
TARGET_BRANCH="${TARGET_BRANCH:-feature/f1stats-modernuo}"
UPSTREAM_REMOTE="${UPSTREAM_REMOTE:-origin}"
UPSTREAM_BRANCH="${UPSTREAM_BRANCH:-main}"
BASE_BRANCH="${BASE_BRANCH:-main}"
AUTO_COMMIT="${AUTO_COMMIT:-0}"
COMMIT_MESSAGE="${COMMIT_MESSAGE:-feat: reapply f1stats patch}"

cd "$ROOT_DIR"

echo "[1/5] Preflight checks"
if ! git diff --quiet || ! git diff --cached --quiet; then
  echo "Working tree is not clean. Please commit/stash changes first."
  exit 1
fi

if [[ ! -f "$PATCH_FILE" ]]; then
  echo "Patch file not found: $PATCH_FILE"
  exit 1
fi

echo "[2/5] Sync local $BASE_BRANCH with $UPSTREAM_REMOTE/$UPSTREAM_BRANCH"
git fetch "$UPSTREAM_REMOTE" "$UPSTREAM_BRANCH"
git checkout "$BASE_BRANCH"
git reset --hard "$UPSTREAM_REMOTE/$UPSTREAM_BRANCH"

echo "[3/5] Recreate feature branch: $TARGET_BRANCH"
git checkout -B "$TARGET_BRANCH" "$BASE_BRANCH"

echo "[4/5] Apply F1 stats patch"
if git apply --check "$PATCH_FILE"; then
  git apply "$PATCH_FILE"
  echo "Patch applied cleanly."
else
  echo "Patch does not apply cleanly in direct mode, trying 3-way merge..."
  if git apply --3way "$PATCH_FILE"; then
    echo "Patch applied via 3-way merge."
  else
    echo "Patch apply failed. Resolve conflicts manually in Projects/UOContent/Network/UOGateway.cs"
    exit 1
  fi
fi

echo "[5/5] Publish validation"
chmod +x publish.sh
./publish.sh release linux x64

if [[ "$AUTO_COMMIT" == "1" ]]; then
  echo "[auto-commit] Commit patched changes"
  git add Projects/UOContent/Network/UOGateway.cs

  if git diff --cached --quiet; then
    echo "No staged changes to commit."
  else
    if git commit -m "$COMMIT_MESSAGE"; then
      echo "Committed: $COMMIT_MESSAGE"
    else
      echo "Commit failed. Ensure git user.name/user.email are configured."
      exit 1
    fi
  fi
fi

echo "Done. Branch: $TARGET_BRANCH"
if [[ "$AUTO_COMMIT" != "1" ]]; then
  echo "Next: git add Projects/UOContent/Network/UOGateway.cs && git commit -m \"feat: reapply f1stats patch\""
fi
