using AdoWithCore.Mapper;
using AdoWithCore.Repositorries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AdoWithCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RepositoryContext context;

        public UserController(RepositoryContext _context) {
            context = _context;
        }
        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserDTO user) {

            context.SetCommandType(RepositoryContext.CommandType.PROCEDURE);
            context.AddQueryParam("@Username",user.UserName);
            context.AddQueryParam("@Password", user.Password);
            context.AddQueryParam("@Email", user.Email);
            context.AddQueryParam("@Firstname", user.FirstName);
            context.AddQueryParam("@Lastname", user.LastName);
            SqlParameter userID = context.AddOutputParam("@UserId",System.Data.SqlDbType.Int);

            context.DataWriteQuery("addNewuser");

            context.Clean();

            return  Ok(new UserSingleDTO() {
                Id = (int) userID.Value,
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName}) ;
        }
    }
}
