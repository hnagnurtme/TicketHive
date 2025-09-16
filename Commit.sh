#!/bin/bash

# ===============================================
# commit.sh
# Tạo commit message theo chuẩn Conventional Commits
# ===============================================

# Kiểm tra xem có trong Git repository không
if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
  echo "Lỗi: Không phải Git repository. Hãy khởi tạo Git bằng 'git init'."
  exit 1
fi

# Kiểm tra xem có thay đổi nào để commit không
if git diff --cached --quiet && git diff --quiet; then
  echo "Lỗi: Không có thay đổi nào để commit."
  exit 1
fi

# Danh sách các loại commit theo chuẩn Conventional Commits
echo "Chọn loại commit:"
echo "1. feat: Tính năng mới"
echo "2. fix: Sửa lỗi"
echo "3. docs: Cập nhật tài liệu"
echo "4. style: Định dạng mã (không ảnh hưởng logic)"
echo "5. refactor: Tái cấu trúc mã"
echo "6. test: Thêm hoặc sửa kiểm thử"
echo "7. chore: Các thay đổi nhỏ khác"
read -p "Nhập số (1-7): " choice

# Ánh xạ lựa chọn thành loại commit
case $choice in
  1) type="feat" ;;
  2) type="fix" ;;
  3) type="docs" ;;
  4) type="style" ;;
  5) type="refactor" ;;
  6) type="test" ;;
  7) type="chore" ;;
  *) echo "Lựa chọn không hợp lệ. Thoát..."; exit 1 ;;
esac

# Nhập mô tả commit
read -p "Nhập mô tả commit (ngắn gọn): " description

# Kiểm tra mô tả có rỗng không
if [ -z "$description" ]; then
  echo "Lỗi: Mô tả commit không được để trống."
  exit 1
fi

# Tạo commit message
commit_message="$type: $description"

# Thêm tất cả các thay đổi
echo "Đang thêm tất cả các thay đổi..."
git add .

# Tạo commit
echo "Đang tạo commit với message: $commit_message"
git commit -m "$commit_message"

# Kiểm tra xem commit có thành công không
if [ $? -eq 0 ]; then
  echo "Commit thành công!"
else
  echo "Lỗi: Commit thất bại."
  exit 1
fi