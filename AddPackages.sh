#!/bin/bash

# ===============================================
# AddPackages.sh
# Tự động đọc packages.txt và add package cho project
# ===============================================

PACKAGE_FILE="Packages.txt"

if [ ! -f "$PACKAGE_FILE" ]; then
    echo "Error: $PACKAGE_FILE not found!"
    exit 1
fi

while IFS= read -r line; do
    # Bỏ qua comment và dòng trống
    [[ "$line" =~ ^#.*$ ]] && continue
    [[ -z "$line" ]] && continue

    # Split line thành args
    args=($line)
    PROJECT=${args[0]}
    # Lấy phần còn lại là packages + optional framework
    PACKAGE_ARGS=("${args[@]:1}")

    # Resolve project path flexibly
    PROJECT_FILE=""
    PROJECT_DIR=""

    if [[ "$PROJECT" == *.csproj ]]; then
        # Khi nhập trực tiếp đường dẫn .csproj
        if [ -f "$PROJECT" ]; then
            PROJECT_FILE="$PROJECT"
            PROJECT_DIR="$(dirname "$PROJECT")"
        fi
    else
        # Trường hợp PROJECT là tên thư mục dự án (ví dụ: TicketHive.Api)
        if [ -f "$PROJECT/$PROJECT.csproj" ]; then
            PROJECT_FILE="$PROJECT/$PROJECT.csproj"
            PROJECT_DIR="$PROJECT"
        elif [ -f "src/$PROJECT/$PROJECT.csproj" ]; then
            PROJECT_FILE="src/$PROJECT/$PROJECT.csproj"
            PROJECT_DIR="src/$PROJECT"
        else
            # Thử tìm kiếm trong toàn repo (lần đầu trùng khớp)
            FOUND=$(find . -type f -name "$PROJECT.csproj" -print -quit)
            if [ -n "$FOUND" ]; then
                PROJECT_FILE="$FOUND"
                PROJECT_DIR="$(dirname "$FOUND")"
            fi
        fi
    fi

    if [ -z "$PROJECT_FILE" ] || [ ! -f "$PROJECT_FILE" ]; then
        echo "Warning: Could not resolve project file for '$PROJECT'. Skipping."
        continue
    fi

    # Chạy dotnet add package (dùng thư mục dự án)
    echo "Adding packages ${PACKAGE_ARGS[@]} to project $PROJECT_DIR ..."
    dotnet add "$PROJECT_DIR" package "${PACKAGE_ARGS[@]}"

    if [ $? -eq 0 ]; then
        echo "Packages added successfully for $PROJECT"
    else
        echo "Failed to add packages for $PROJECT"
    fi

done < "$PACKAGE_FILE"
