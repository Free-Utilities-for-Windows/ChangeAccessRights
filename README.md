# ChangeAccessRights
This is a command-line utility for viewing and modifying the access rights of files and directories on Windows.

## Usage

1. Open a command prompt.
2. Navigate to the directory containing the utility.
3. Run the utility with the path to the file or directory as a command-line argument. For example:

    ```
    .\ChangeAccessRights.exe "C:\Example\"
    ```

    This will start the utility and display a menu with the following options:

    ```
    1. Show access rights
    2. Change access rights
    3. Exit
    ```

### Show Access Rights

To view the access rights of the specified file or directory, enter `1` at the prompt. The utility will display the access rights for each user.

### Change Access Rights

To change the access rights of the specified file or directory, enter `2` at the prompt. The utility will then prompt you for the following information:

- The username of the user whose access rights you want to change.
- The rights you want to add or remove (Read, Write, FullControl).
- Whether you want to add or remove these rights.

After you've entered this information, the utility will change the access rights and display a confirmation message.
