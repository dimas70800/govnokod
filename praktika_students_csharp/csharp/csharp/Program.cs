using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirtyTextEditor
{

    public class EditorContext
    {
        public List<string> CurrentFiles = new List<string>();
        public string CurrentDirectory = Directory.GetCurrentDirectory();
        public string CurrentEditFile = "";
        public string EditorBuffer = "";
        public List<string> SearchResults = new List<string>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var ctx = new EditorContext();
            Console.Clear();

            while (true)
            {
                Console.Clear();
                ctx.CurrentFiles = FileManager.GetFiles(ctx.CurrentDirectory);
                UIHelper.DisplayFileList(ctx);
                UIHelper.DisplayMainMenu();

                Console.Write("\n  Ваш выбор: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                if (choice == 99) break;

                HandleCommand(choice, ctx);
            }
        }

        static void HandleCommand(int choice, EditorContext ctx)
        {
            switch (choice)
            {
                case 1: TextEditor.ReadFile(GetPath(ctx), ctx); break;
                case 2: TextEditor.EditFile(ctx); break;
                case 3: TextEditor.SaveFile(ctx); break;
                case 4:
                    Console.Write("\n  Enter search query: ");
                    SearchEngine.SearchInFile(ctx, Console.ReadLine());
                    break;
                case 5: FileManager.DeleteFile(GetPath(ctx)); break;
                case 6:
                    Console.Write("\n  Enter filename: ");
                    FileManager.CreateFile(Path.Combine(ctx.CurrentDirectory, Console.ReadLine() ?? "new.txt"));
                    break;
                case 7: NavigateDirectory(ctx); break;
                case 8:
                    string s = GetPath(ctx);
                    if (s != null) FileManager.CopyFile(s, s + ".copy");
                    break;
                case 9:
                    string o = GetPath(ctx);
                    if (o != null)
                    {
                        Console.Write("  Enter new name: ");
                        FileManager.RenameFile(o, Path.Combine(ctx.CurrentDirectory, Console.ReadLine()));
                    }
                    break;
                case 10: Utils.AnalyzeFileProperties(GetPath(ctx)); break;
                case 11: TextEditor.CreateBackup(ctx); break;
            }
            if (choice != 2 && choice != 7) { Console.Write("\n  Нажмите Enter..."); Console.ReadLine(); }
        }
        static string GetPath(EditorContext ctx)
        {
            Console.Write("\n  Choose file number: ");
            if (int.TryParse(Console.ReadLine(), out int i) && i > 0 && i <= ctx.CurrentFiles.Count)
                return Path.Combine(ctx.CurrentDirectory, ctx.CurrentFiles[i - 1]);
            return null;
        }

        static void NavigateDirectory(EditorContext ctx)
        {
            Console.Write("\n  Enter path (.. for parent): ");
            string newdir = Console.ReadLine();
            string target = newdir == ".." ? Path.GetDirectoryName(ctx.CurrentDirectory) : Path.Combine(ctx.CurrentDirectory, newdir);
            if (Directory.Exists(target)) ctx.CurrentDirectory = target;
        }
    }
}
