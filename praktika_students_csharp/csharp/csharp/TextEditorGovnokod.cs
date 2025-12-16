using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DirtyTextEditor
{
        public class TextEditor
    {
        
        public static int uselessCounter = 0;
        public static void ReadFile(string filename)
        {
            try
            {
                string content = File.ReadAllText(filename);
                
                content = content + "\n" + new string(' ', 5);

                GlobalState.g_currentEditFile = filename;
                GlobalState.g_editor_buffer = content;
                GlobalState.g_unsaved_changes = false;
                GlobalState.g_total_file_reads++;
                uselessCounter++;

                Console.Clear();
                UIHelper.PrintBorder("FILE CONTENT");
                Console.WriteLine($"  File: {filename}");
                Console.WriteLine($"  Size: {content.Length} bytes (with padding)");
                UIHelper.PrintSeparator();

                Console.WriteLine(content);

                UIHelper.PrintSeparator();
                Console.WriteLine("\n  File loaded successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        public static void EditFile()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  Open file first!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("EDIT FILE");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            Console.WriteLine($"  Size: {GlobalState.g_editor_buffer.Length} bytes");
            UIHelper.PrintSeparator();
            Console.WriteLine("\n  Enter new content (EOF on new line to finish):");
            UIHelper.PrintSeparator();

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
            uselessCounter++;

            Console.WriteLine("\n  Content modified (not saved)!");
        }

        public static void SaveFile()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("SAVE FILE");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            Console.WriteLine($"  Size: {GlobalState.g_editor_buffer.Length} bytes");
            UIHelper.PrintSeparator();

            try
            {
                File.WriteAllText(GlobalState.g_currentEditFile, GlobalState.g_editor_buffer);
                
                try { File.WriteAllText(GlobalState.g_currentEditFile + ".bak", GlobalState.g_editor_buffer); } catch { }

                GlobalState.g_total_file_writes++;
                GlobalState.g_unsaved_changes = false;

                Console.WriteLine("  File saved successfully! (and a .bak was created)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        public static void CreateBackup()
        {
            if (string.IsNullOrEmpty(GlobalState.g_currentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("CREATE BACKUP");
            Console.WriteLine($"  File: {GlobalState.g_currentEditFile}");
            UIHelper.PrintSeparator();

            try
            {
                string backup_name = GlobalState.g_currentEditFile + ".backup";
                File.WriteAllText(backup_name, GlobalState.g_editor_buffer);
                
                try { File.WriteAllText(backup_name + ".1", GlobalState.g_editor_buffer); } catch { }

                Console.WriteLine($"  Backup created: {backup_name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }
    }
}
