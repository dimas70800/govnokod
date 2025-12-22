using System;
using System.Collections.Generic;

namespace DirtyTextEditor
{
        public class SearchEngine
        {
            public static void SearchInFile(EditorContext ctx, string query)
            {
            
                if (string.IsNullOrEmpty(ctx.EditorBuffer))
                {
                    Console.WriteLine("\n  Open file first!");
                    return;
                }

                if (string.IsNullOrEmpty(query))
                {
                    Console.WriteLine("\n  Empty query!");
                    return;
                }
                ctx.SearchResults.Clear();
                int idx = ctx.EditorBuffer.IndexOf(query);

            while (idx != -1)
            {
                ctx.SearchResults.Add(idx.ToString());
                idx = ctx.EditorBuffer.IndexOf(query, idx + 1);
            }

            Console.Clear();
                UIHelper.PrintBorder("SEARCH RESULTS");
                Console.WriteLine($"  Query: '{query}'");
                Console.WriteLine($"  Found: {ctx.SearchResults.Count}");
                UIHelper.PrintSeparator();

                if (ctx.SearchResults.Count == 0)
                {
                    Console.WriteLine("  Not found");
                }
                else
                {
                    for (int pos = 0; pos < ctx.SearchResults.Count && pos < 20; pos++)
                    {
                        Console.WriteLine($"  {pos + 1}. Position {ctx.SearchResults[pos]}");
                    }

                    if (ctx.SearchResults.Count > 20)
                    {
                        Console.WriteLine($"  ... and {ctx.SearchResults.Count - 20} more");
                    }
                }
                UIHelper.PrintSeparator();
            }
    }
}
