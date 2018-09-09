using EWUS_Expertdatabase.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EWUS_Expertdatabase.Common
{
    [Serializable]
    [DataContract]
    public class Result<TEntity> : Result
    {
        public Result()
        {
            this.Type = typeof(TEntity);
        }
        
        public Result(IType item)
        {
            this.Type = item.Type;

            this.Status = ResultStatus.OK;
        }

        public new TEntity Value
        {
            get
            {
                if (base.Value != null)
                    return (TEntity)(base.Value);
                else
                    return default(TEntity);

            }

            set
            {
                base.Value = value;

                if (value != null && value is IEnumerable && this.RecordsAffected <= 0)
                {
                    this.RecordsAffected = (value as IEnumerable).Count();
                }
            }
        }

        public void SetResult(ResultStatus status, string exceptionMessage = "", string userMessage = "")
        {
            this.Status = status;
            this.ExceptionMessage = exceptionMessage;
            this.UserMessage = userMessage;
        }

        public Result<TEntity> SetAndReturnResult(ResultStatus status, string exceptionMessage = "", string userMessage = "")
        {
            SetResult(status, exceptionMessage, userMessage);

            return this;
        }

        public static Result<TEntity> CreateTypedInstance(ResultStatus status, string exceptionMessage = "", string userMessage = "")
        {
            Result<TEntity> r = new Result<TEntity>();
            r.SetResult(status, exceptionMessage, userMessage);

            return r;
        }
    }

    [Serializable]
    [DataContract]
    public class Result 
    {
        public Result()
        {
            this.NextLink = new Reference();
            this.CanContinue = true;
            this.ShowMessageOnSuccess = true;
        }

        
        [XmlIgnore]
        public TimeSpan? MaxAge { get; set; }

        [XmlIgnore]
        public object Value { get; set; }

        [DataMember]
        public string SerializedValue { get; set; }


        [XmlIgnore]
        public object Tag { get; set; }

        [XmlIgnore]
        public Type Type { get; set; }

        /// <summary>
        ///     Gets or Sets the result object id.
        /// </summary>
        [DataMember]
        public string ObjectId { get; set; }

        /// <summary>
        ///     Gets or Sets the result object name. Optioanl.
        /// </summary>
        [DataMember]
        public string ObjectName { get; set; }

        /// <summary>
        ///     Gets or Sets the correlation id.
        /// </summary>
        public string CorrelationId { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        /// <summary>
        ///     The number of objects written to the underlying database.
        /// </summary>
        [DataMember]
        public int RecordsAffected { get; set; }

        [DataMember]
        public long TotalCount { get; set; }

        [DataMember]
        public TimeSpan ExecuteTime { get; set; }

        string _FormatedExecuteTime = string.Empty;


        [DataMember]
        public string FormatedExecuteTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_FormatedExecuteTime) == false)
                    return _FormatedExecuteTime;
                else
                    return ExecuteTime.ToString();
            }
            set { _FormatedExecuteTime = value; }
        }

        [DataMember]
        public string ExceptionMessage { get; set; }

        [DataMember]
        public string UserMessage { get; set; }

        [DataMember]
        public int ExceptionCode { get; set; }

        [DataMember]
        public Reference NextLink { get; set; }

        [DataMember]
        public bool Success
        {
            get
            {
                var success = (string.IsNullOrWhiteSpace(ExceptionMessage)
                    && (this.Status == ResultStatus.OK || this.Status == ResultStatus.Created));

                return success;
            }

            set
            {
                var success = value;
            }
        }

        /// <summary>
        ///    Get or Set Can Chain Continu after this result
        /// </summary>
        public bool CanContinue
        {
            get;
            set;
        }

        /// <summary>
        ///    Get or Set - Show Message on success result
        /// </summary>
        [DataMember]
        public bool ShowMessageOnSuccess
        {
            get;
            set;
        }

        public bool HasException()
        {
            return string.IsNullOrWhiteSpace(ExceptionMessage) == false;
        }

        [DataMember]
        public ResultStatus Status { get; set; }

        public static Result CreateInstance(ResultStatus status)
        {
            Result r = new Result();
            r.Status = status;
            return r;
        }
                
        [XmlIgnore]
        public string CommandText { get; set; }

        public static Result<TEntity> ToResult<TEntity>(ResultStatus status, Type type, string exceptionMessage = "", int exceptionCode = 0)
        {
            Result<TEntity> result = new Result<TEntity>();
            result.Status = status;
            //result.Value = value;
            result.Type = type;
            result.ExceptionMessage = exceptionMessage;
            result.ExceptionCode = exceptionCode;

            return result;
        }
    }
}
