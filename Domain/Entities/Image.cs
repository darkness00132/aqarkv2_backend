using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public required string Url { get; set; }
    }
}
