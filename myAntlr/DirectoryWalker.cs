using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace myAntlr
{
    public class DirectoryWalker
    {
        string rootdirectory = "";
        string fileextension = "";
        int maxdepth = 1000;
        List<string> allfiles = new List<string>();

        public void setMaxDepth(int d)
        {
            maxdepth = d;
        }
        
        public DirectoryWalker(string di, string extension)
        {
            rootdirectory = di;
            fileextension = extension;
        }

        public List<string> getAllfiles()
        {
            DirectoryInfo rootdir = new DirectoryInfo(rootdirectory);
            walkDirectory(rootdir, 0);
            return allfiles;
        }


        void walkDirectory(DirectoryInfo rootdir, int depth)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = rootdir.GetFiles(fileextension);
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                Console.WriteLine(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    
                    // Console.WriteLine(fi.FullName);
                    allfiles.Add(fi.FullName);
                }
            }
            // Now find all the subdirectories under this directory.
            if (depth < maxdepth)
            {
                subDirs = rootdir.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    this.walkDirectory(dirInfo, depth + 1);
                }
            }
        }
        
    }
}
