using System;
using System.Collections.Generic;
using System.IO;

namespace DirtyTextEditor
{
    public class Utils
    {
        //эффективная сортировка
        public static void Sort(List<string> arr)
        {
            arr.Sort();
        }
        //анализ файла
        public static void AnalyzeFileProperties(string filepath)
        {
            try
            {
                Console.Clear();
                UIHelper.PrintBorder("FILE ANALYSIS");
                
                var size = new FileInfo(filepath).Length;
                string content = File.ReadAllText(filepath);
                int lines = content.Split('\n').Length;
                
                Console.WriteLine($"  Name: {Path.GetFileName(filepath)}");
                Console.WriteLine($"  Size: {size} bytes");
                Console.WriteLine($"  Lines: {lines}");
                
                UIHelper.PrintSeparator();
            }
            catch (Exception e)
            {
                Console.WriteLine("\n  Error: " + e.Message);
            }
        }
    }
}
