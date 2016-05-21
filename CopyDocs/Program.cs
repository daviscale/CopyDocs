using System;
using System.IO;

namespace CopyDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                string sourceDirectoryName = args[0];
                string destinationDirectoryName = args[1];
                DirectoryCopy(sourceDirectoryName, destinationDirectoryName);
            } else
            {
                Console.WriteLine("Please supply a source directory name and a target directory name as arguments");
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destinationDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationDirName))
            {
                Directory.CreateDirectory(destinationDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string oldPath = Path.Combine(sourceDirName, file.Name);
                string newPath = Path.Combine(destinationDirName, file.Name);
                Console.WriteLine($"Copying {oldPath} to {newPath}");
                file.CopyTo(newPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destinationDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
    }
}
