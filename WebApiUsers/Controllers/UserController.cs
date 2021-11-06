using CORE.Users.Interfaces;
using CORE.Users.Models;
using CORE.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiUsers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IConfiguration _configuration;
        string ConnectionStringAzure = string.Empty;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                ConnectionStringAzure = _configuration.GetConnectionString("CloudServer");

            //ConnectionStringAzure = "Server=tcp:mtwdm-alexis.database.windows.net,1433;Initial Catalog=core_3crud;Persist Security Info=False;User ID=Alexlml96;Password=Maestria_123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        ///https://localhost:5001/api/User/GetUser?ID=2
        [HttpGet("GetUsers")]
        public IEnumerable<UserModel> GetUsers()
        {
            List<UserModel> model = new List<UserModel>();
#if true

            using (IUser User = FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            //using (IUser User = Users_CORE.Services.FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? Users_CORE.Models.EServer.CLOUD : Users_CORE.Models.EServer.CLOUD))
            {
                model = User.GetUsers(); 
            }

#endif
            return model;


        }

        ///https://localhost:5001/api/User/GetUser?ID=2
        [HttpGet]
        [Route("GetUser")]
        public ActionResult<UserModel> GetUser(int ID)
        {
            if (ID == 0)
                return BadRequest("Ingrese ID de usuario válido");

            UserModel model = new UserModel();
            using (IUser User = FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            //using (IUser User = Users_CORE.Services.FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? Users_CORE.Models.EServer.CLOUD : Users_CORE.Models.EServer.CLOUD))
            {
                model = User.GetUser(ID);
            }

            return model;
        }

        //https://localhost:5001/api/User/AddUser
        [HttpPost]
        [Route("[action]")]
        public ActionResult AddUser(UserModel user)
        {
            if (user == null)
                return BadRequest("Ingrese información de usuario");

            long model = 0;
            using (IUser User = FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            //using (IUser User = Users_CORE.Services.FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? Users_CORE.Models.EServer.CLOUD : Users_CORE.Models.EServer.CLOUD))
            {
                model = User.AddUser(user);
            }

            return model > 0 ? Ok() : BadRequest("Error al insertar");
        }
        //https://localhost:5001/api/User/UpdateUser
        [HttpPost]
        [Route("[action]")]
        public ActionResult UpdateUser(UserModel user)
        {
            if (user.Identificador == 0)
                return BadRequest("Ingrese un ID válido");

            bool model = false;
            using (IUser User = FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            //using (IUser User = Users_CORE.Services.FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? Users_CORE.Models.EServer.CLOUD : Users_CORE.Models.EServer.CLOUD))
            {
                model = User.UpdateUser(user);
            }

            return model == true ? Ok() : BadRequest("Error al actualizar");
        }

        //https://localhost:5001/api/user/deleteuser?ID=2
        [HttpDelete]
        [Route("[action]")]
        public ActionResult DeleteUser(int ID)
        {
            if (ID == 0)
                return BadRequest("Ingrese un ID válido");

            using (IUser User = FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? EServer.LOCAL : EServer.CLOUD))
            //using (IUser User = Users_CORE.Services.FactorizerService.Inicializar(ConnectionStringAzure == string.Empty ? Users_CORE.Models.EServer.CLOUD : Users_CORE.Models.EServer.CLOUD))
            {
                User.DeleteUser(ID);
            }

            return Ok();
        }
    }
}
