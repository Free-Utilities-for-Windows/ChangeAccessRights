using System.Security.AccessControl;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a path to a file or folder as a command-line argument.");
            return;
        }

        string path = args[0];
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            Console.WriteLine("The specified path does not exist.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\n1. Show access rights");
            Console.WriteLine("2. Change access rights");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowAccessRights(path);
                    break;
                case 2:
                    ChangeAccessRightsMenu(path);
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                    break;
            }
        }
    }

    static void ShowAccessRights(string path)
    {
        FileSystemSecurity fileSystemSecurity;
        if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            fileSystemSecurity = fileInfo.GetAccessControl();
        }
        else
        {
            var dirInfo = new DirectoryInfo(path);
            fileSystemSecurity = dirInfo.GetAccessControl();
        }

        AuthorizationRuleCollection acl =
            fileSystemSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
        foreach (FileSystemAccessRule ace in acl)
        {
            Console.WriteLine($"{ace.IdentityReference.Value} has {ace.FileSystemRights} rights.");
        }
    }

    static void ChangeAccessRightsMenu(string path)
    {
        Console.Write("Enter the username: ");
        string username = Console.ReadLine();

        Console.Write("Enter the rights (Read, Write, FullControl): ");
        string rights = Console.ReadLine();

        FileSystemRights fileSystemRights;
        if (!Enum.TryParse(rights, out fileSystemRights))
        {
            Console.WriteLine("Invalid rights. Please enter Read, Write, or FullControl.");
            return;
        }

        Console.Write("Do you want to add or remove these rights? (add/remove): ");
        string action = Console.ReadLine();

        AccessControlType controlType;
        if (action.ToLower() == "add")
        {
            controlType = AccessControlType.Allow;
        }
        else if (action.ToLower() == "remove")
        {
            controlType = AccessControlType.Deny;
        }
        else
        {
            Console.WriteLine("Invalid action. Please enter add or remove.");
            return;
        }

        ChangeAccessRights(path, username, fileSystemRights, controlType);

        Console.WriteLine("Access rights changed successfully.");
    }

    static void ChangeAccessRights(string path, string username, FileSystemRights rights, AccessControlType controlType)
    {
        FileSystemSecurity security;
        if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            security = fileInfo.GetAccessControl();
        }
        else if (Directory.Exists(path))
        {
            var dirInfo = new DirectoryInfo(path);
            security = dirInfo.GetAccessControl();
        }
        else
        {
            Console.WriteLine("The specified path does not exist.");
            return;
        }

        FileSystemAccessRule rule = new FileSystemAccessRule(username, rights, controlType);

        if (controlType == AccessControlType.Allow)
        {
            security.AddAccessRule(rule);
        }
        else
        {
            security.RemoveAccessRule(rule);
        }

        if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            fileInfo.SetAccessControl((FileSecurity)security);
        }
        else if (Directory.Exists(path))
        {
            var dirInfo = new DirectoryInfo(path);
            dirInfo.SetAccessControl((DirectorySecurity)security);

            foreach (string entry in Directory.GetFileSystemEntries(path))
            {
                ChangeAccessRights(entry, username, rights, controlType);
            }
        }
    }
}