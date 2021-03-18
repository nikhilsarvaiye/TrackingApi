namespace Common
{
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;

    public class ApplicationError : Exception
    {
        public ApplicationError() : base()
        {
        }

        public ApplicationError(string message) : base(message)
        {
        }

        public ApplicationError(string message, Exception innerException) : base(message, innerException)
        {
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ApplicationError InnerError { get; set; }

        public string StackTrace { get; set; }

        public List<ValidationFailure> Errors { get; set; }

        public static ApplicationError From(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            return new ApplicationError
            {
                StatusCode = GetExceptionErrorCode(exception),
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                InnerError = ApplicationError.From(exception.InnerException),
                Errors = exception.GetType()?.GetProperty("Errors")?.GetValue(exception, null) as List<ValidationFailure>
            };
        }

        public override string ToString()
        {
            if (this == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }

        private static int GetExceptionErrorCode(Exception exception)
        {
            var winException = exception as Win32Exception;
            if (winException != null)
            {
                return winException.ErrorCode;
            }

            if (exception.InnerException != null)
            {
                return GetExceptionErrorCode(exception.InnerException);
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
