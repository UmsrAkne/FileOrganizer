namespace FileOrganizer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DoubleFileList
    {
        public DoubleFileList(List<ExtendFileInfo> files)
        {
            Files = files;
            OriginalFiles = Files.ToList();
        }

        public List<ExtendFileInfo> Files { get; set; }

        private List<ExtendFileInfo> OriginalFiles { get; set; }
    }
}
