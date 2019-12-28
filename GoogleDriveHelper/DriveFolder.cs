using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDriveHelper
{
    public class DriveFolder
    {
        public string id { get; set; }
        public string name { get; set; }
        public int filesNumber { get; set; }

        public List<DriveFolder> subFolders;

        public DriveFolder(string folderId, string folderName)
        {
            this.id = folderId;
            this.name = folderName;
            this.subFolders = new List<DriveFolder>();
        }

        public string print() => JsonConvert.SerializeObject(this);
    }
}
