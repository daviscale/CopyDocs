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
                string sourcePath = args[0];
                string destinationPathPrefix = args[1];
                string destinationPath = CreateDestinationPath(sourcePath, destinationPathPrefix);

                DirectoryCopy(sourcePath, destinationPath);
            } else
            {
                Console.WriteLine("Please supply a source directory and a target directory as arguments");
            }
        }
        
        private static String CreateDestinationPath(string sourcePath, string destinationPathPrefix)
        {
            string folderToCopy = new DirectoryInfo(sourcePath).Name;
            string currentDateTime = DateTime.Now.ToString();
            string backupFolderName = $"{folderToCopy}-{currentDateTime}";
            return Path.Combine(destinationPathPrefix, backupFolderName);
        }

        private static String CreateTimestampString()
        {
            DateTime currentDateTime = DateTime.Now;
            var year = currentDateTime.Year.ToString();
            var month = currentDateTime.Month.ToString();
            var day = currentDateTime.Day.ToString();
            var hour = currentDateTime.Hour.ToString();
            var minute = currentDateTime.Minute.ToString();
            var second = currentDateTime.Second.ToString();
            return $"{year}{month}{day}{hour}{minute}{second}";
        }

        private static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourcePath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourcePath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string oldPath = Path.Combine(sourcePath, file.Name);
                string newPath = Path.Combine(destinationPath, file.Name);
                Console.WriteLine($"Copying {oldPath} to {newPath}");
                file.CopyTo(newPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destinationPath, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
    }
}
