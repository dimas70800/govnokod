using System;
using System.Collections.Generic;

namespace DirtyTextEditor
{
        public class SearchEngine
    {
        public static void SearchInFile(string buffer, string query)
        {
            
            if (string.IsNullOrEmpty(buffer))
            {
                Console.WriteLine("\n  Open file first!");
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                Console.WriteLine("\n  Empty query!");
                return;
            }

            GlobalState.g_search_results.Clear();
            
            GlobalState.g_total_file_reads += 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                try
                {
                    var sub = buffer.Substring(i, Math.Min(query.Length, buffer.Length - i));
                    if (sub == query)
                    {
                        GlobalState.g_search_results.Add(i.ToString());
                        if (query.Length % 2 == 1)
                        {
                            GlobalState.g_search_results.Add(i.ToString()); 
                        }
                    }
                }
                catch { }
            }

            Console.Clear();
            UIHelper.PrintBorder("SEARCH RESULTS");
            Console.WriteLine($"  Query: '{query}'");
            Console.WriteLine($"  Found: {GlobalState.g_search_results.Count}");
            UIHelper.PrintSeparator();

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
            UIHelper.PrintSeparator();
        }
    }
}
