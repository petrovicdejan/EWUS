using EWUS_Expertdatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Common
{
    //public interface IRepository : IDisposable
    //{
    //    object GetById(long Id);

    //    IEnumerable<object> GetAll();

    //    Result Save(Item value);

    //    Result ExecuteCommand(Item value);
    //}

    ///// <summary>
    /////     Represent IValueConverter type
    ///// </summary>
    //public interface IValueConverter
    //{
    //    Type From { get; }

    //    Type To { get; }

    //    bool TryToConvert(object from, out object to);
    //}

    public interface IType
    {
        Type Type { get; set; }
    }

    //public interface ICoreObject
    //{

    //}
}
