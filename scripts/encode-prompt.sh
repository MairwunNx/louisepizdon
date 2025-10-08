#!/bin/bash
# Script to encode vision prompt to base64

if [ -z "$1" ]; then
    echo "Usage: $0 <prompt-file>"
    echo "Example: $0 prompt.txt"
    exit 1
fi

if [ ! -f "$1" ]; then
    echo "Error: File '$1' not found"
    exit 1
fi

echo "Encoding $1 to base64..."
base64 -w 0 "$1"
echo ""
echo "✅ Done! Copy the output above and use it as visionPromptBase64 in your config."