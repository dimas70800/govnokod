using System;
using System.Collections.Generic;

namespace DirtyTextEditor
{
    public static class UIHelper
    {
        public static void PrintBorder(string title = "")
        {
            Console.WriteLine("  " + new string('=', 76));
            if (!string.IsNullOrEmpty(title))
            {
                int padding = (76 - title.Length) / 2;
                Console.WriteLine($"  |{title.PadLeft(padding + title.Length).PadRight(76)}|");
                Console.WriteLine("  " + new string('=', 76));
            }
        }

        public static void PrintSeparator()
        {
            Console.WriteLine("  " + new string('-', 76));
        }

        public static void PrintFileTableHeader()
        {
            Console.Write("  " + "N".PadRight(4));
            Console.Write("Name".PadRight(40));
            Console.Write("Size".PadRight(15));
            Console.WriteLine("Type");
            PrintSeparator();
        }

        public static void PrintFileRow(int index, string name, bool isDir, long size)
        {
            Console.Write("  " + index.ToString().PadRight(4));

            string display_name = name;
            if (name.Length > 36)
            {
                display_name = name.Substring(0, 33) + "...";
            }


            Console.Write(display_name.PadRight(40) + " ");

            if (isDir)
            {
                Console.Write("-".PadRight(15));
                Console.WriteLine("Folder");
            }
            else
            {
                string size_str = "";
                try
                {
                    if (size == 0)
                    {
                        size_str = size.ToString() + " B";
                    }
                    else if (size < 1024)
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
                }
                catch { size_str = "?"; }

                Console.Write(size_str.PadRight(15));
                Console.WriteLine("File");
            }
        }

        public static void DisplayMainMenu()
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

        public static void DisplayFileList(EditorContext ctx)
        {
            PrintBorder("SODERZHIMOE KATALOGA");
            Console.WriteLine($"  Path: {ctx.CurrentDirectory}");
            Console.WriteLine($"  Elements: {ctx.CurrentFiles.Count}");
            PrintSeparator();

            if (ctx.CurrentFiles.Count == 0)
            {
                Console.WriteLine("  Folder is empty");
                PrintSeparator();
                return;
            }

            PrintFileTableHeader();

            for (int i = 0; i < ctx.CurrentFiles.Count; i++)
            {
                string full_path = Path.Combine(ctx.CurrentDirectory, ctx.CurrentFiles[i]);
                bool isDir = Directory.Exists(full_path);
                long size = 0;

                if (!isDir && File.Exists(full_path))
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

                PrintFileRow(i + 1, ctx.CurrentFiles[i], isDir, size);
            }

            PrintSeparator();
        }
    }
}
