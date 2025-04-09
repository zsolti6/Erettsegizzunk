def prepend_text_to_all_lines(file_path, text_to_prepend):
    # Read the existing content of the file
    with open(file_path, 'r', encoding='utf-8') as file:
        lines = file.readlines()
    
    # Prepend the specified text and a tab to each line
    new_lines = [text_to_prepend + '\t' + line for line in lines]
    
    # Write the modified content back to the file
    with open(file_path, 'w', encoding='utf-8') as file:
        file.writelines(new_lines)

# Example usage:
file_path = 'feladatok2.txt'  # Replace with the path to your text file
text_to_prepend = 'Oldja meg az alábbi feladatot!'
prepend_text_to_all_lines(file_path, text_to_prepend)
