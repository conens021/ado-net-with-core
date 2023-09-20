using Microsoft.AspNetCore.Mvc;

namespace AdoWithCore.Controllers
{
    public class ResponseHelper
    {
        public static object? ToJson(object? data) {

            JsonResult json = new JsonResult(data);
            json.StatusCode = 200;
            json.ContentType = "application/json";
            return json.Value;
        
        }
    }
}
