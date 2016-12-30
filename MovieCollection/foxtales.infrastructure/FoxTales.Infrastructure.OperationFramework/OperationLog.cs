// Copyright (C) 2014 FoxTales
// Released under the MIT License
 
using System;
using System.Text;

namespace FoxTales.Infrastructure.OperationFramework
{
    public abstract class OperationLogBase
    {
        internal OperationLogType Type { get; private set; }

        public abstract string Description { get; }

        protected OperationLogBase(OperationLogType type)
        {
            Type = type;
        }

        public abstract string GetCode();
    }

    public class OperationLog<TErrorCode> : OperationLogBase where TErrorCode : struct, IConvertible, IComparable, IFormattable
    {
        public TErrorCode Code { get; private set; }
        public string Message { get; private set; }

        internal OperationLog(OperationLogType type, TErrorCode code, string message = null) : base(type)
        {
            Code = code;
            Message = message;
        }

        public override string Description
        {
            get
            {
                var description = new StringBuilder(GetCode());
                if (Message != null)
                {
                    description.Append(": ");
                    description.Append(Message);
                }

                return description.ToString();
            }
        }

        public override string GetCode()
        {
            return string.Format("{0}_{1}", typeof (TErrorCode).Name, Code);
        }
    }

    public enum OperationLogType
    {
        Warning,
        Error
    }
}