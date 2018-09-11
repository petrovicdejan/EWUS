using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWUS_Expertdatabase.Web.Client
{
    public class ClassificationViewModel
    {
        public long Id { get; set; }

        public string ClassificationType { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }
    }
}