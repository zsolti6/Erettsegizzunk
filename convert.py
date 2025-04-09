import re

# Load the file
with open("output_file.txt", "r", encoding="utf-8") as file:
    content = file.read()

# Define the replacements
replacements = {
    r"(\d)x(\d)": r"\1 \times \2",  # 5x7 => 5 × 7 (optional)
    r" ∙ ": r" \\cdot ",             # multiplication dot
    r" ∩ ": r" \\cap ",              # intersection
    r" ∪ ": r" \\cup ",              # union
    r"\\": r" \\setminus ",          # set difference (A\B)
    r"→": r"\\to",                   # arrow for functions
    r"(\w)\^(\d)": r"\1^\2",         # exponents like x^2
    r"\{": r"\\{",                   # opening brace
    r"\}": r"\\}",                   # closing brace
}

# Apply the replacements
for pattern, replacement in replacements.items():
    content = re.sub(pattern, replacement, content)

# Extra: Wrap math parts in \( ... \) if needed (optional, can be improved later)
# Example: Wrap all { ... } sets
content = re.sub(r"(A = \\{.*?\\})", r"\\(\1\\)", content)
content = re.sub(r"(B = \\{.*?\\})", r"\\(\1\\)", content)
content = re.sub(r"(A \\cap B)", r"\\(\1\\)", content)
content = re.sub(r"(A \\cup B)", r"\\(\1\\)", content)
content = re.sub(r"(A \\setminus B)", r"\\(\1\\)", content)
content = re.sub(r"(f: \\mathbb\{R\} \\to \\mathbb\{R\})", r"\\(\1\\)", content)

# Save the converted text
with open("feladatok_latex.txt", "w", encoding="utf-8") as file:
    file.write(content)

print("✅ Conversion complete! Saved to 'feladatok_latex.txt'.")
