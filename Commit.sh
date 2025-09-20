#!/bin/bash

# ===============================================
# commit.sh
# Script tạo commit message theo chuẩn Conventional Commits
# ===============================================

# Kiểm tra có trong Git repository không
if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
  echo "❌ Lỗi: Không phải Git repository. Hãy chạy 'git init' trước."
  exit 1
fi

# Kiểm tra có thay đổi nào không
if git diff --cached --quiet && git diff --quiet; then
  echo "❌ Lỗi: Không có thay đổi nào để commit."
  exit 1
fi

# Menu commit types
echo "Chọn loại commit:"
echo "1. feat     : ✨ Tính năng mới"
echo "2. fix      : 🐛 Sửa lỗi"
echo "3. docs     : 📚 Cập nhật tài liệu"
echo "4. style    : 🎨 Định dạng mã (không ảnh hưởng logic)"
echo "5. refactor : ♻️  Tái cấu trúc mã (không sửa bug, không thêm tính năng)"
echo "6. test     : ✅ Thêm hoặc sửa test"
echo "7. chore    : 🔧 Thay đổi vặt (config, script...)"
echo "8. perf     : ⚡️ Cải thiện hiệu năng"
echo "9. ci       : 🤖 Thay đổi CI/CD pipeline"
echo "10. build   : 🏗️  Thay đổi build system hoặc dependencies"
echo "11. revert  : ⏪ Revert commit trước đó"
read -p "👉 Nhập số (1-11): " choice

# Ánh xạ type
case $choice in
  1) type="feat" ;;
  2) type="fix" ;;
  3) type="docs" ;;
  4) type="style" ;;
  5) type="refactor" ;;
  6) type="test" ;;
  7) type="chore" ;;
  8) type="perf" ;;
  9) type="ci" ;;
  10) type="build" ;;
  11) type="revert" ;;
  *) echo "❌ Lựa chọn không hợp lệ."; exit 1 ;;
esac

# Nhập scope
read -p "👉 Nhập scope (ví dụ: auth, ticket; Enter để bỏ qua): " scope

# Nhập description
read -p "👉 Nhập mô tả commit (ngắn gọn): " description
if [ -z "$description" ]; then
  echo "❌ Lỗi: Mô tả commit không được để trống."
  exit 1
fi

# Format commit message
if [ -n "$scope" ]; then
  commit_message="$type($scope): $description"
else
  commit_message="$type: $description"
fi

# Add và commit
echo "🔍 Đang thêm thay đổi..."
git add .

echo "📝 Commit message: $commit_message"
git commit -m "$commit_message"

if [ $? -eq 0 ]; then
  echo "✅ Commit thành công!"
else
  echo "❌ Commit thất bại."
  exit 1
fi
