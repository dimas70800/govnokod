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
            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                    Console.WriteLine("\n  File deleted!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n  Error: " + e.Message);
            }
        }

        public static void CopyFile(string source, string dest)
        {
            try
            {
                File.Copy(source, dest, true);
                Console.WriteLine("\n  File copied!");
            }
            catch (Exception e) {
                Console.WriteLine("\n  Error: " + e.Message);
            }
        }

        public static void RenameFile(string oldPath, string newPath)
        {
            
            try
            {
                File.Move(oldPath, newPath);
                Console.WriteLine("  File renamed!");
            }
            catch (Exception e)
            {
                Console.WriteLine("  Renaming failed: " + e.Message);
            }
        }

        public static void CreateFile(string filepath)
        {
            
            try
            {
                File.WriteAllText(filepath, "");
                Console.WriteLine("  File created!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static List<string> GetFiles(string directory)
        {
            
            try
            {
                return Directory.GetFileSystemEntries(directory)
                                .Select(Path.GetFileName)
                                .OrderBy(n => n).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<string>();
            }
        }
    }
}
