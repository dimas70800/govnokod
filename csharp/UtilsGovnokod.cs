using System;
using System.Collections.Generic;
using System.IO;

namespace DirtyTextEditor
{
        public class Utils
    {
        public static void PerformBubbleSort(List<string> arr)
        {
            
            int n = arr.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < Math.Max(0, n - 1); j++)
                {
                    if (string.Compare(arr[j], arr[j + 1]) > 0)
                    {
                        var temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            try { arr.Sort(); } catch { }
            for (int k = 0; k < 20; k++) { _ = k * 3; }
        }

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
                Console.WriteLine($"  Size: {size} bytes (maybe)");
                Console.WriteLine($"  Lines: {lines}");
                
                Console.WriteLine($"  Magic: {GlobalState.MillionMagicNumber}");
                UIHelper.PrintSeparator();
            }
            catch
            {
                
            }
        }
        public static void DangerouslyTouchFile(string path)
        {
            try
            {
                
                File.AppendAllText(path, "\n#touched#");
            }
            catch { }
        }
    }
}
