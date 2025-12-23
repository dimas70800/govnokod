using System;
using System.Collections.Generic;

namespace DirtyTextEditor
{
        public class SearchEngine
        {
            public static void SearchInFile(string query)
            {
            //проверки существования
                if (string.IsNullOrEmpty(TextEditor.EditorBuffer))
                {
                    Console.WriteLine("\n  Open file first!");
                    return;
                }

                if (string.IsNullOrEmpty(query))
                {
                    Console.WriteLine("\n  Empty query!");
                    return;
                }
                List<string> SearchResults = new List<string>();
                int idx = TextEditor.EditorBuffer.IndexOf(query);

                //обход всех вхождений искомого слова
            while (idx != -1)
            {
                SearchResults.Add(idx.ToString());
                idx = TextEditor.EditorBuffer.IndexOf(query, idx + 1);
            }
            //вывод всех нахождений
            Console.Clear();
                UIHelper.PrintBorder("SEARCH RESULTS");
                Console.WriteLine($"  Query: '{query}'");
                Console.WriteLine($"  Found: {SearchResults.Count}");
                UIHelper.PrintSeparator();

                if (SearchResults.Count == 0)
                {
                    Console.WriteLine("  Not found");
                }
                else
                {
                    for (int pos = 0; pos < SearchResults.Count && pos < 20; pos++)
                    {
                        Console.WriteLine($"  {pos + 1}. Position {SearchResults[pos]}");
                    }

                    if (SearchResults.Count > 20)
                    {
                        Console.WriteLine($"  ... and {SearchResults.Count - 20} more");
                    }
                }
                UIHelper.PrintSeparator();
            }
    }
}
