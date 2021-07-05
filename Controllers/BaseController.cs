using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Test.Controllers
{
    public class BaseController<T> : Controller
    {
        private IMapper _mapper;
        private ILogger<T> _logger;

        protected IMapper Mapper
        {
            get { return _mapper ?? (_mapper = HttpContext?.RequestServices.GetService<IMapper>()); }
            set { _mapper = value; }
        }

        protected ILogger<T> Logger
        {
            get { return _logger ?? (_logger = HttpContext?.RequestServices.GetService<ILogger<T>>()); }
            set { _logger = value; }
        }

        protected void LogModelState()
        {
            var message = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Logger.LogError($"The model is not valid: {message}");
        }
        protected bool ModelStateIsValid()
        {
            if (!ModelState.IsValid)
            {
                LogModelState();
            }

            return ModelState.IsValid;
        }
    }
}