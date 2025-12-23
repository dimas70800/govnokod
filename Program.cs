using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirtyTextEditor
{

    class Program
    {
        static List<string> CurrentFiles = new List<string>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

            while (true)
            {
                Console.Clear();
                CurrentFiles = FileManager.GetFiles(Directory.GetCurrentDirectory());
                UIHelper.DisplayFileList();
                UIHelper.DisplayMainMenu();

                Console.Write("\n  Ваш выбор: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                if (choice == 99) break;

                HandleCommand(choice);
            }
        }
        //выбор что делать
        static void HandleCommand(int choice)
        {
            switch (choice)
            {
                //чтение
                case 1: TextEditor.ReadFile(GetPath()); break;
                    //редактирование
                case 2: TextEditor.EditFile(); break;
                    //сохранение
                case 3: TextEditor.SaveFile(); break;
                    //поиск в файле текущем
                case 4:
                    Console.Write("\n  Enter search query: ");
                    SearchEngine.SearchInFile(Console.ReadLine());
                    break;
                    //удаление
                case 5: FileManager.DeleteFile(GetPath()); break;
                    //создание файла
                case 6:
                    Console.Write("\n  Enter filename: ");
                    FileManager.CreateFile(Path.Combine(Directory.GetCurrentDirectory(), Console.ReadLine() ?? "new.txt"));
                    break;
                    //переход в другую директорию
                case 7: NavigateDirectory(); break;
                    //копирование файла
                case 8:
                    string s = GetPath();
                    if (s != null) FileManager.CopyFile(s, s + ".copy");
                    break;
                    //переименование файла
                case 9:
                    string o = GetPath();
                    if (o != null)
                    {
                        Console.Write("  Enter new name: ");
                        FileManager.RenameFile(o, Path.Combine(Directory.GetCurrentDirectory(), Console.ReadLine()));
                    }
                    break;
                    //анализ файла
                case 10: Utils.AnalyzeFileProperties(GetPath()); break;
                    //создание бэкапа
                case 11: TextEditor.CreateBackup(); break;
            }
            if (choice != 2 && choice != 7) { Console.Write("\n  Нажмите Enter..."); Console.ReadLine(); }
        }
        //выбор пути вводом
        static string GetPath()
        {
            Console.Write("\n  Choose file number: ");
            if (int.TryParse(Console.ReadLine(), out int i) && i > 0 && i <= CurrentFiles.Count)
                return Path.Combine(Directory.GetCurrentDirectory(), CurrentFiles[i - 1]);
            return null;
        }

        //переход в другую директорию
        static void NavigateDirectory()
        {
            Console.Write("\n  Enter path (.. for parent): ");
            string newdir = Console.ReadLine();
            string target = newdir == ".." ? Path.GetDirectoryName(Directory.GetCurrentDirectory()) : Path.Combine(Directory.GetCurrentDirectory(), newdir);
            if (Directory.Exists(target)) Directory.SetCurrentDirectory(target);
        }
    }
}
