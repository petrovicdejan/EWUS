using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace EWUS_Expertdatabase.Web.Client
{
    public class CustomerViewModel
    {
        public long Id { get; set; }

        public string Logo { get; set; }

        public string Name { get; set; }

        public Collection<DocumentItem> DocumentItems { get; set; }
    }
}