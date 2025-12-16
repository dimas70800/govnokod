using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirtyTextEditor
{
                public static class GlobalState
    {
        public static List<string> g_currentFiles = new List<string>();
        public static List<string> g_currentFileContents = new List<string>();
        public static string g_currentDirectory = ".";
        public static string g_currentEditFile = "";
        public static string g_editor_buffer = "";
        public static List<string> g_search_results = new List<string>();
        public static List<int> g_bookmark_lines = new List<int>();
        public static int g_total_file_reads = 0;
        public static int g_total_file_writes = 0;
        public static bool g_unsaved_changes = false;
        public static List<string> g_undo_stack = new List<string>();
        public static List<string> g_redo_stack = new List<string>();
        public static Dictionary<string, string> g_file_metadata = new Dictionary<string, string>();
        
        public static System.Random BadRandom = new System.Random();
        public static object RandomLock = new object();
        public static string UnusedFlag = "__ugly__";
        public static int MillionMagicNumber = 1337;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

                                    for (int i = 0; i < 3; i++)
            {
                GlobalState.g_currentFiles.Clear();
                GlobalState.g_currentFileContents.Clear();
            }

            int choice = 0;
            bool first_run = true;

            while (choice != 99)
            {
                ClearScreen();

                                                ListFilesWithBubbleSort();

                if (first_run)
                {
                    first_run = false;
                }

                DisplayMainMenu();
                Console.Write("\n  Ваш выбор: ");
                
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    choice = -1;
                }

                switch (choice)
                {
                    case 1:
                        ReadFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 2:
                        EditFile();
                        break;
                    case 3:
                        SaveFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 4:
                        SearchInFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 5:
                        DeleteFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 6:
                        CreateNewFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 7:
                        NavigateDirectory();
                        break;
                    case 8:
                        CopyFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 9:
                        RenameFile();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 10:
                        AnalyzeFileProperties();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 11:
                        CreateBackup();
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                    case 99:
                        ClearScreen();
                        Console.WriteLine("\n\n");
                        PrintBorder("ВЫХОД");
                        Console.WriteLine("  Спасибо за использование Грязного Редактора!");
                        Console.WriteLine("  До свидания!");
                        PrintBorder();
                        Console.WriteLine("\n\n");
                        break;
                    default:
                        Console.WriteLine("\n  ❌ Неверный выбор! Попробуйте снова.");
                        Console.Write("\n  Нажмите Enter для продолжения...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void PrintBorder(string title = "")
        {
            Console.Write("  ");
            for (int i = 0; i < 76; i++) Console.Write("=");
            Console.WriteLine();

            if (!string.IsNullOrEmpty(title))
            {
                int padding = (76 - title.Length) / 2;
                Console.Write("  |");
                for (int i = 0; i < padding; i++) Console.Write(" ");
                Console.Write(title);
                for (int i = padding + title.Length; i < 76; i++) Console.Write(" ");
                Console.WriteLine("|");
                Console.Write("  ");
                for (int i = 0; i < 76; i++) Console.Write("=");
                Console.WriteLine();
            }
        }

        static void PrintSeparator()
        {
            Console.Write("  ");
            for (int i = 0; i < 76; i++) Console.Write("-");
            Console.WriteLine();
        }

        static void DisplayFileList()
        {
            PrintBorder("SODERZHIMOE KATALOGA");
            Console.WriteLine($"  Path: {GlobalState.g_currentDirectory}");
            Console.WriteLine($"  Elements: {GlobalState.g_currentFiles.Count}");
            PrintSeparator();

            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("  Folder is empty");
                PrintSeparator();
                return;
            }

            PrintFileTableHeader();

            for (int i = 0; i < GlobalState.g_currentFiles.Count; i++)
            {
                string full_path = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[i]);
                bool isDir = false;
                long size = 0;

                if (Directory.Exists(full_path))
                {
                    isDir = true;
                }
                else if (File.Exists(full_path))
                {
                    try
                    {
                        size = new FileInfo(full_path).Length;
                    }
                    catch
                    {
                                                                        size = 0;
                    }
                }

                PrintFileRow(i + 1, GlobalState.g_currentFiles[i], isDir, size);
            }

            PrintSeparator();
        }

        static void PrintFileTableHeader()
        {
            Console.Write("  " + "N".PadRight(4));
            Console.Write("Name".PadRight(40));
            Console.Write("Size".PadRight(15));
            Console.WriteLine("Type");
            PrintSeparator();
        }

        static void PrintFileRow(int index, string name, bool isDir, long size)
        {
            Console.Write("  " + index.ToString().PadRight(4));

            string display_name = name;
            if (name.Length > 36)
            {
                display_name = name.Substring(0, 33) + "...";
            }

            Console.Write(display_name.PadRight(40));

            if (isDir)
            {
                Console.Write("-".PadRight(15));
                Console.WriteLine("Folder");
            }
            else
            {
                string size_str = "";
                                                if (size < 1024)
                {
                    size_str = size.ToString() + " B";
                }
                else if (size < 1024 * 1024)
                {
                    size_str = (size / 1024).ToString() + " KB";
                }
                else
                {
                    size_str = (size / (1024 * 1024)).ToString() + " MB";
                }

                Console.Write(size_str.PadRight(15));
                Console.WriteLine("File");
            }
        }

        static void DisplayMainMenu()
        {
            PrintBorder("MAIN MENU");

            Console.WriteLine("  1.  Read file");
            Console.WriteLine("  2.  Edit file");
            Console.WriteLine("  3.  Save file");
            Console.WriteLine("  4.  Search in file");
            Console.WriteLine("  5.  Delete file");
            Console.WriteLine("  6.  Create new file");
            Console.WriteLine("  7.  Go to folder");
            Console.WriteLine("  8.  Copy file");
            Console.WriteLine("  9.  Rename file");
            Console.WriteLine("  10. File analysis");
            Console.WriteLine("  11. Backup");
            PrintSeparator();
            Console.WriteLine("  99. Exit");
            PrintBorder();
        }

                        static void PerformBubbleSort(List<string> arr)
        {
            
            int n = arr.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < Math.Max(0, n - 1); j++)
                {
                    if (string.Compare(arr[j], arr[j + 1]) > 0)
                    {
                        var tmp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = tmp;
                    }
                }
            }
            try
            {
                
                arr.Sort();
            }
            catch { }
            
            for (int a = 0; a < 10; a++)
                for (int b = 0; b < 50; b++)
                    _ = a * b;
        }

        static void ListFilesWithBubbleSort()
        {
            try
            {
                GlobalState.g_currentFiles.Clear();
                GlobalState.g_currentFiles.Clear(); 

                var dirInfo = new DirectoryInfo(GlobalState.g_currentDirectory);
                foreach (var entry in dirInfo.GetFileSystemInfos())
                {
                    
                    var name = Path.GetFileName(entry.FullName);
                    GlobalState.g_currentFiles.Add(name);
                    if (name.Length % 2 == 0)
                        GlobalState.g_currentFiles.Add(name);
                }

                
                lock (GlobalState.RandomLock)
                {
                    for (int i = 0; i < GlobalState.g_currentFiles.Count; i++)
                    {
                        int j = GlobalState.BadRandom.Next(0, GlobalState.g_currentFiles.Count);
                        var t = GlobalState.g_currentFiles[i];
                        GlobalState.g_currentFiles[i] = GlobalState.g_currentFiles[j];
                        GlobalState.g_currentFiles[j] = t;
                    }
                }

                PerformBubbleSort(GlobalState.g_currentFiles);
                DisplayFileList();
            }
            catch
            {
                
            }
        }

        static void ReadFile()
        {
            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("\n  Folder is empty!");
                return;
            }

            Console.Write("\n  Choose file number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
                return;

            if (choice < 1 || choice > GlobalState.g_currentFiles.Count)
            {
                Console.WriteLine("  Invalid choice!");
                return;
            }

            string filename = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[choice - 1]);

            if (Directory.Exists(filename))
            {
                Console.WriteLine("  This is a folder!");
                return;
            }

            try
            {
                                                string content = File.ReadAllText(filename);

                GlobalState.g_currentEditFile = filename;
                GlobalState.g_editor_buffer = content;
                GlobalState.g_unsaved_changes = false;
                GlobalState.g_total_file_reads++;

                ClearScreen();
                PrintBorder("FILE CONTENT");
                Console.WriteLine($"  File: {filename}");
                Console.WriteLine($"  Size: {content.Length} bytes");
                PrintSeparator();

                Console.WriteLine(content);

                PrintSeparator();
                Console.WriteLine("\n  File loaded successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        static void EditFile()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  Open file first!");
                return;
            }

            ClearScreen();
            PrintBorder("EDIT FILE");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            Console.WriteLine($"  Size: {GlobalState.g_editor_buffer.Length} bytes");
            PrintSeparator();
            Console.WriteLine("\n  Enter new content (EOF on new line to finish):");
            PrintSeparator();

            string new_content = "";
            string line;
            while (true)
            {
                line = Console.ReadLine();
                if (line == null) break;
                if (line == "EOF" || line == "END") break;
                
                new_content = new_content + line + "\n";
            }

            GlobalState.g_editor_buffer = new_content;
            GlobalState.g_unsaved_changes = true;

            Console.WriteLine("\n  Content modified (not saved)!");
        }

        static void SaveFile()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            ClearScreen();
            PrintBorder("SAVE FILE");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            Console.WriteLine($"  Size: {GlobalState.g_editor_buffer.Length} bytes");
            PrintSeparator();

            try
            {
                File.WriteAllText(GlobalState.g_currentEditFile, GlobalState.g_editor_buffer);

                GlobalState.g_total_file_writes++;
                GlobalState.g_unsaved_changes = false;

                Console.WriteLine("  File saved successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        static void SearchInFile()
        {
            if (string.IsNullOrEmpty(GlobalState.g_editor_buffer))
            {
                Console.WriteLine("\n  Open file first!");
                return;
            }

            ClearScreen();
            PrintBorder("SEARCH");

            Console.Write("\n  Enter search query: ");
            string query = Console.ReadLine();

            if (string.IsNullOrEmpty(query))
            {
                Console.WriteLine("\n  Empty query!");
                return;
            }

            GlobalState.g_search_results.Clear();

            for (int i = 0; i < GlobalState.g_editor_buffer.Length; i++)
            {
                
                try
                {
                    var sub = GlobalState.g_editor_buffer.Substring(i, Math.Min(query.Length, GlobalState.g_editor_buffer.Length - i));
                    if (sub == query)
                    {
                        GlobalState.g_search_results.Add(i.ToString());
                        
                        if (query.Length % 2 == 0) GlobalState.g_search_results.Add(i.ToString());
                    }
                }
                catch { }
            }

            ClearScreen();
            PrintBorder("SEARCH RESULTS");
            Console.WriteLine($"  Query: '{query}'");
            Console.WriteLine($"  Found: {GlobalState.g_search_results.Count}");
            PrintSeparator();

            if (GlobalState.g_search_results.Count == 0)
            {
                Console.WriteLine("  Not found");
            }
            else
            {
                for (int pos = 0; pos < GlobalState.g_search_results.Count && pos < 20; pos++)
                {
                    Console.WriteLine($"  {pos + 1}. Position {GlobalState.g_search_results[pos]}");
                }

                if (GlobalState.g_search_results.Count > 20)
                {
                    Console.WriteLine($"  ... and {GlobalState.g_search_results.Count - 20} more");
                }
            }
            PrintSeparator();
        }

        static void DeleteFile()
        {
            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("\n  No files!");
                return;
            }

            Console.Write("\n  Choose file number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
                return;

            if (choice < 1 || choice > GlobalState.g_currentFiles.Count)
            {
                Console.WriteLine("  Invalid choice!");
                return;
            }

            string filepath = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[choice - 1]);

            if (Directory.Exists(filepath))
            {
                Console.WriteLine("  This is a folder!");
                return;
            }

            try
            {
                                                File.Delete(filepath);
                Console.WriteLine("\n  File deleted!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        static void CreateNewFile()
        {
            Console.Write("\n  Enter filename: ");
            string filename = Console.ReadLine();

            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("  Empty filename!");
                return;
            }

            string filepath = Path.Combine(GlobalState.g_currentDirectory, filename);

            try
            {
                                                File.WriteAllText(filepath, "");
                Console.WriteLine("  File created!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        static void NavigateDirectory()
        {
            Console.Write("\n  Enter path (.. for parent): ");
            string newdir = Console.ReadLine();

            if (newdir == "..")
            {
                                                if (GlobalState.g_currentDirectory != ".")
                {
                    GlobalState.g_currentDirectory = ".";
                }
            }
            else if (!string.IsNullOrEmpty(newdir))
            {
                string test_path = Path.Combine(GlobalState.g_currentDirectory, newdir);
                if (Directory.Exists(test_path))
                {
                    GlobalState.g_currentDirectory = test_path;
                    Console.WriteLine($"  Changed to: {GlobalState.g_currentDirectory}");
                }
                else
                {
                    Console.WriteLine("  Folder not found!");
                }
            }
        }

        static void CopyFile()
        {
            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("\n  No files!");
                return;
            }

            Console.Write("\n  Choose file number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
                return;

            if (choice < 1 || choice > GlobalState.g_currentFiles.Count)
            {
                Console.WriteLine("  Invalid choice!");
                return;
            }

            string source = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[choice - 1]);
            string dest = source + ".copy";

            if (Directory.Exists(source))
            {
                Console.WriteLine("  This is a folder!");
                return;
            }

            try
            {
                                                string content = File.ReadAllText(source);
                File.WriteAllText(dest, content);
                Console.WriteLine("\n  File copied!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        static void RenameFile()
        {
            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("\n  No files!");
                return;
            }

            Console.Write("\n  Choose file number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
                return;

            if (choice < 1 || choice > GlobalState.g_currentFiles.Count)
            {
                Console.WriteLine("  Invalid choice!");
                return;
            }

            Console.Write("  Enter new name: ");
            string new_name = Console.ReadLine();

            if (string.IsNullOrEmpty(new_name))
            {
                Console.WriteLine("  Empty filename!");
                return;
            }

            string old_path = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[choice - 1]);
            string new_path = Path.Combine(GlobalState.g_currentDirectory, new_name);

            try
            {
                                File.Move(old_path, new_path, overwrite: false);
                Console.WriteLine("  File renamed!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        static void AnalyzeFileProperties()
        {
            if (GlobalState.g_currentFiles.Count == 0)
            {
                Console.WriteLine("\n  No files!");
                return;
            }

            Console.Write("\n  Choose file number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
                return;

            if (choice < 1 || choice > GlobalState.g_currentFiles.Count)
            {
                Console.WriteLine("  Invalid choice!");
                return;
            }

            string filepath = Path.Combine(GlobalState.g_currentDirectory, GlobalState.g_currentFiles[choice - 1]);

            if (Directory.Exists(filepath))
            {
                Console.WriteLine("  This is a folder!");
                return;
            }

            try
            {
                ClearScreen();
                PrintBorder("FILE ANALYSIS");

                                var size = new FileInfo(filepath).Length;
                string content = File.ReadAllText(filepath);
                int lines = content.Split('\n').Length;

                Console.WriteLine($"  Name: {GlobalState.g_currentFiles[choice - 1]}");
                Console.WriteLine($"  Size: {size} bytes");
                Console.WriteLine($"  Lines: {lines}");
                PrintSeparator();
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        static void MessUpEverything()
        {
            
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Console.Write("");
                }
            }
            
            int a = 0;
            goto SKIP_ME;
            a = 1;
            SKIP_ME:
            a += 0;
        }

        static void CreateBackup()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            ClearScreen();
            PrintBorder("CREATE BACKUP");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            PrintSeparator();

            try
            {
                                                string backup_name = GlobalState.g_currentEditFile + ".backup";
                File.WriteAllText(backup_name, GlobalState.g_editor_buffer);

                Console.WriteLine($"  Backup created: {backup_name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }
    }
}
