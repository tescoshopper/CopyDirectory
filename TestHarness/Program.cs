using CopyDirectory;
using System;
using System.Threading.Tasks;

namespace TestHarness
{
    class Program
    {
         static async Task<int> Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <Source Folder> <Destination Folder>");
                return 1;
            }

            // Handle all errors both expected and unexpected at this time.
            // Note this is a test harness only.
            try
            {
                Console.WriteLine("Copying Files ...");
                Console.WriteLine();

                IOFunctions.FileCopied += IOFunctions_FileCopied;
                Task copyFiles = IOFunctions.CopyFilesAsync(args[0], args[1]);

                for (int i = 0; i < 50000; i++)
                {
                    Console.WriteLine($"Doing something else in the UI {i}");
                }
                await copyFiles;
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation aborted with the following error:");
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
        private static void IOFunctions_FileCopied(object sender, FileCopiedEventArgs e)
        {
            Console.WriteLine(e.File);
        }
    }
}
