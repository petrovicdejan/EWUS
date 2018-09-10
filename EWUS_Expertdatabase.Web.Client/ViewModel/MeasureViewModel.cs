using EWUS_Expertdatabase.Model;
using System.Collections.ObjectModel;

namespace EWUS_Expertdatabase.Web.Client
{
    public class MeasureViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int SerialNumber { get; set; }
        
        public Classification OperationType { get; set; }

        public int? OperationTypeId { get; set; }
        
        public string Saving { get; set; }

        public string Description { get; set; }

        public int InvestmentCost { get; set; }
        
        public Collection<MeasurePicture> MeasurePictures { get; set; }
        
        public Collection<MeasureLink> MeasureLinks { get; set; }
        
        public Collection<DocumentItem> DocumentItems { get; set; }
    }
}
