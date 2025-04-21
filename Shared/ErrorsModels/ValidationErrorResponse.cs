using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorsModels
{
    public class ValidationErrorResponse
    {
        public int SatutsCode { get; set; } = StatusCodes.Status400BadRequest;

        public string ErrorMessage { get; set; } = "Validation Error";

        public IEnumerable<ValidationError> Errors { get; set; }


    }
}
