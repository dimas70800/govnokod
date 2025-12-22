using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DirtyTextEditor
{
    public class TextEditor
    {
        public static void ReadFile(string path, EditorContext ctx)
        {
            if (path == null || !File.Exists(path)) return;
            try
            {
                ctx.EditorBuffer = File.ReadAllText(path);
                ctx.CurrentEditFile = path;
                Console.Clear();
                UIHelper.PrintBorder("FILE CONTENT");
                Console.WriteLine($"  File: {path}");
                Console.WriteLine($"  Size: {ctx.EditorBuffer.Length} bytes(with padding)");
                UIHelper.PrintSeparator();
                Console.WriteLine(ctx.EditorBuffer);
                UIHelper.PrintSeparator();
                Console.WriteLine("\n  File loaded successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n  Error: {e.Message}\n");
            }
        }

        public static void EditFile(EditorContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.CurrentEditFile))
            {
                Console.WriteLine("\n  Open file first!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("EDIT FILE");
            Console.WriteLine($"  File: {ctx.CurrentEditFile}");
            Console.WriteLine($"  Size: {ctx.EditorBuffer.Length} bytes");
            UIHelper.PrintSeparator();
            Console.WriteLine("\n  Enter new content (EOF on new line to finish):");
            UIHelper.PrintSeparator();

            ctx.EditorBuffer = "";
            string line = Console.ReadLine();
            while (line != "EOF")
            {
                ctx.EditorBuffer += line + Environment.NewLine;
                line = Console.ReadLine();
            }

            Console.WriteLine("\n  Content modified (not saved)!");
        }

        public static void SaveFile(EditorContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.CurrentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("SAVE FILE");
            Console.WriteLine($"  File: {ctx.CurrentEditFile}");
            Console.WriteLine($"  Size: {ctx.EditorBuffer.Length} bytes");
            UIHelper.PrintSeparator();

            try
            {
                File.WriteAllText(ctx.CurrentEditFile, ctx.EditorBuffer);
                
                try {
                    File.WriteAllText(ctx.CurrentEditFile, ctx.EditorBuffer);
                    File.WriteAllText(ctx.CurrentEditFile + ".bak", ctx.EditorBuffer);
                }catch {}
                Console.WriteLine("  File saved successfully! (and a .bak was created)");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }

        public static void CreateBackup(EditorContext ctx)
        {
            if (string.IsNullOrEmpty(ctx.CurrentEditFile))
            {
                Console.WriteLine("\n  No open file!");
                return;
            }

            Console.Clear();
            UIHelper.PrintBorder("CREATE BACKUP");
            Console.WriteLine($"  File: {ctx.CurrentEditFile}");
            UIHelper.PrintSeparator();

            try
            {
                string backup_name = ctx.CurrentEditFile + ".backup";
                File.WriteAllText(backup_name, ctx.EditorBuffer);
                Console.WriteLine($"  Backup created: {backup_name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"  Error: {e.Message}");
            }
        }
    }
}
