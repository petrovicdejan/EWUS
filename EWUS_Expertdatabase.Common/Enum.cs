using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Common
{
    public enum ResultStatus
    {
        /// <summary>
        ///     General status for a successful request.
        /// </summary>
        OK = 200,
        /// <summary>
        ///     Used on POST request when creating a new resource.
        /// </summary>
        Created = 201,
        /// <summary>
        ///    Typically if any validation for your parameters fails.
        /// </summary>
        BadRequest = 400,
        /// <summary>
        ///     Authentication is required to make this request.
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        ///     Unable to access regardless of authentication or not.  
        /// </summary>
        Forbidden = 403,
        /// <summary>
        ///     It someone access a resource or URI that doesn’t exist.
        /// </summary>
        NotFound = 404,
        /// <summary>
        ///     For POST and PUT requests if the resource already exists.   
        /// </summary>
        Conflict = 409,
        /// <summary>
        ///     Any general error on the system.
        /// </summary>
        InternalServerError = 500
    }

}
