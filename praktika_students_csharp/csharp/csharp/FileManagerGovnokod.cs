using System;
using System.Collections.Generic;
using System.IO;

namespace DirtyTextEditor
{
    
    public class FileManager
    {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static List<string> GetFiles(string directory)
        {
            
            var files = new List<string>();
            try
            {
                var dir = new DirectoryInfo(directory);
                foreach (var entry in dir.GetFileSystemInfos())
                {
                    
                    if (entry.Exists && entry.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        files.Add(entry.FullName);
                    }
                    else
                    {
                        files.Add(Path.GetFileName(entry.FullName));
                    }
                }
                files.Sort((a,b)=>a.Length - b.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return files;
        }
    }
}
