using System;
using System.Collections.Generic;
using System.IO;

namespace DirtyTextEditor
{
    
    public class FileManager
    {
        
        public static int GLOBAL_COUNTER = 0;
        public static string LastAction = "";

        
        public static void DeleteFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("no path provided");
                return;
            }

            RETRY:
            try
            {
                File.Delete(filepath);
                Console.WriteLine("\n  File deleted!");
                GLOBAL_COUNTER++;
                LastAction = "delete";
            }
            catch (Exception e)
            {
                
                Console.WriteLine("\n  Error: " + e.Message);
                try
                {
                    
                    using (var fs = File.Open(filepath, FileMode.OpenOrCreate)) { }
                    goto RETRY;
                }
                catch { /* ignore everything */ }
            }
        }

        public static void CopyFile(string source, string dest)
        {
            
            try
            {
                var content = File.ReadAllText(source);
                
                File.WriteAllText(dest, content);
                
                File.AppendAllText(dest, "\n" + "".PadLeft(1));
                Console.WriteLine("\n  File copied!");
                GLOBAL_COUNTER += 2;
                LastAction = "copy";
            }
            catch (Exception)
            {
                
                try { File.WriteAllText(dest, ""); } catch { }
            }
        }

        public static void RenameFile(string oldPath, string newPath)
        {
            
            try
            {
                File.Move(oldPath, newPath);
                Console.WriteLine("  File renamed!");
                LastAction = "rename";
            }
            catch
            {
                try
                {
                    var txt = File.ReadAllText(oldPath);
                    File.WriteAllText(newPath, txt);
                    File.Delete(oldPath);
                    Console.WriteLine("  File renamed by copy-delete fallback!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("  Renaming failed: " + e.Message);
                }
            }
        }

        public static void CreateFile(string filepath)
        {
            
            try
            {
                var filler = new string(' ', 10);
                File.WriteAllText(filepath, filler);
                Console.WriteLine("  File created!");
                GLOBAL_COUNTER++;
            }
            catch (Exception)
            {
                
            }
        }

        public static List<string> GetFiles(string directory)
        {
            
            var files = new List<string>();
            try
            {
                var di = new DirectoryInfo(directory);
                foreach (var entry in di.GetFileSystemInfos())
                {
                    
                    if (entry.Exists && entry.Attributes.HasFlag(FileAttributes.Directory))
                        files.Add(entry.FullName);
                    else
                        files.Add(Path.GetFileName(entry.FullName));
                }
                
                files.Sort((a,b)=>a.Length - b.Length);
            }
            catch
            {
                
            }
            return files;
        }

        
        private static void DoNothingButWasteTime()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Write("");
            }
        }
    }
}
