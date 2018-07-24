using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Args
{
    public class FileFoundEventArgs : EventArgs
    {
        public string FileFullPath{ get; }


        public FileFoundEventArgs(string fileFullPath)
        {
            this.FileFullPath = fileFullPath;
        }
    }
}
