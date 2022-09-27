using KcloudScript.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KcloudScript.Api.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public IActionResult SetResponse(HttpStatusCode httpStatus, bool _result, object _data, string _message)
        {
            return new ObjectResult(new ResponseEntity(httpStatus, _result, _data, _message))
            {
                StatusCode = (int)httpStatus
            };
        }
    }
}
