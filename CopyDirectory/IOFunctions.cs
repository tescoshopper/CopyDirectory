using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory
{
    /// <summary>
    /// Represents event arguments for copied file.
    /// </summary>
    public class FileCopiedEventArgs : EventArgs
    {
        public string File { get; set; }
    }

    /// <summary>
    /// Copy Directiory IO functions.
    /// </summary>
    public static class IOFunctions
    {
        public static event EventHandler<FileCopiedEventArgs> FileCopied;

        /// <summary>
        /// Delegate to handle action when file copied.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public static void OnFileCopied(FileCopiedEventArgs e)
        {
            EventHandler<FileCopiedEventArgs> handler = FileCopied;
            handler?.Invoke(null, e);
        }

        /// <summary>
        /// Function to recursively copy a source folder to a destination folder.
        /// </summary>
        /// <param name="sourceFolder">Full path of source folder.</param>
        /// <param name="destinationFolder">Full path of destination folder.</param>
        public static async Task CopyFilesAsync(string sourceFolder, string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);
            foreach (string filename in Directory.EnumerateFiles(sourceFolder))
            {
                using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream DestinationStream = File.Create(destinationFolder + filename.Substring(filename.LastIndexOf('\\'))))
                    {
                        await SourceStream.CopyToAsync(DestinationStream);
                        OnFileCopied(new FileCopiedEventArgs() { File = filename });
                    }
                }
            }

            foreach (string folder in Directory.EnumerateDirectories(sourceFolder))
            {
               await CopyFilesAsync(folder, destinationFolder + folder.Substring(folder.LastIndexOf('\\')));
            }
        }
    }
}
