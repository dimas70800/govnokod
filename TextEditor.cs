using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DirtyTextEditor
{
    public class TextEditor
    {
        public static string EditorBuffer = "";
        public static string CurrentEditFile = "";
        //чтение файла
        public static void ReadFile(string path)
        {
            if (path == null || !File.Exists(path)) return;
            try
            {
                EditorBuffer = File.ReadAllText(path);
                CurrentEditFile = path;
                Console.Clear();
                UIHelper.PrintBorder("FILE CONTENT");
                Console.WriteLine($"  File: {path}");
                Console.WriteLine($"  Size: {EditorBuffer.Length} bytes(with padding)");
                UIHelper.PrintSeparator();
                Console.WriteLine(EditorBuffer);
                UIHelper.PrintSeparator();
                Console.WriteLine("\n  File loaded successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        //редактирование файла
        public static void EditFile()
        {
            if (string.IsNullOrEmpty(CurrentEditFile))
            {
                Console.WriteLine("\n  Open file first!");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("EDIT FILE");
            Console.WriteLine($"  File: {CurrentEditFile}");
            Console.WriteLine($"  Size: {EditorBuffer.Length} bytes");
            UIHelper.PrintSeparator();
            Console.WriteLine("\n  Enter new content (EOF on new line to finish):");
            UIHelper.PrintSeparator();

            EditorBuffer = "";
            string line = Console.ReadLine();
            while (line != "EOF")
            {
                EditorBuffer += line + Environment.NewLine;
                line = Console.ReadLine();
            }

            Console.WriteLine("\n  Content modified (not saved)!");
        }

        //сохранение файла
        public static void SaveFile()
        {
            if (string.IsNullOrEmpty(CurrentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("SAVE FILE");
            Console.WriteLine($"  File: {CurrentEditFile}");
            Console.WriteLine($"  Size: {EditorBuffer.Length} bytes");
            UIHelper.PrintSeparator();

            try
            {
                File.WriteAllText(CurrentEditFile, EditorBuffer);
                File.WriteAllText(CurrentEditFile + ".bak", EditorBuffer);
                Console.WriteLine("  File saved successfully! (and a .bak was created)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        //создание бэкапа
        public static void CreateBackup()
        {
            if (string.IsNullOrEmpty(CurrentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("CREATE BACKUP");
            Console.WriteLine($"  File: {CurrentEditFile}");
            UIHelper.PrintSeparator();

            try
            {
                string backup_name = CurrentEditFile + ".backup";
                File.WriteAllText(backup_name, EditorBuffer);
                Console.WriteLine($"  Backup created: {backup_name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }
    }
}
