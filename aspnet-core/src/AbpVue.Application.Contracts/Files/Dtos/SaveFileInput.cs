using System;
using System.Collections.Generic;
using System.Text;

namespace AbpVue.Files.Dtos
{
    public class SaveFileInput
    {
        public string BolbName { get; set; }
        public byte[] Bytes { get; set; }
    }
}
