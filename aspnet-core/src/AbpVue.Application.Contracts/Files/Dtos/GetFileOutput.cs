using System;
using System.Collections.Generic;
using System.Text;

namespace AbpVue.Files.Dtos
{
    public class GetFileOutput
    {
        public string BlobName { get; set; }
        public byte[] Bytes { get; set; }
    }
}
