def add_theme_ids_to_file(input_file, output_file):
    # List of theme IDs for each row (first 36 are from the original input, 
    # and the last 10 are manually added as per your request)
    theme_ids = [
          # Manually added IDs for the last 10 rows
24, 24, 24, 25, 27, 25, 26, 25, 24, 22, 27, 24, 22, 25, 21, 21, 24, 25, 22, 21, 27, 26, 26, 25,
    21, 27, 24, 22, 25, 21, 27, 24, 22, 22, 21, 21, 25, 22,24, 24, 24, 25, 27, 25, 26, 25, 24, 22,22
    ]
    
     # Open the input file and read its contents
    with open(input_file, 'r', encoding='utf-8') as file:
        lines = file.readlines()

    # Check if there are enough rows in the file to match the theme IDs
    if len(lines) != len(theme_ids):
        print(f"Warning: The number of rows in the file ({len(lines)}) does not match the number of theme IDs ({len(theme_ids)})")
        return

    # Open the output file for writing
    with open(output_file, 'w', encoding='utf-8') as file:
        # Iterate over each line and add the corresponding theme ID at the end of the line
        for i, line in enumerate(lines):
            # Strip any leading/trailing spaces or newlines from the line
            line = line.strip()

            # Ensure we add a tab before the theme ID
            if line:  # Only modify non-empty lines
                theme_id = theme_ids[i]
                modified_line = f"{line}\t{theme_id}\n"
            else:
                modified_line = "\n"  # For empty lines, just write an empty line

            # Write the modified line to the output file
            file.write(modified_line)

# Example usage
input_file = 'feladatok2.txt'  # Replace with your input file path
output_file = 'output_file.txt'  # Replace with your desired output file path
add_theme_ids_to_file(input_file, output_file)
