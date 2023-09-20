using AdoWithCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using AdoWithCore.Repositorries;
using AdoWithCore.Mapper;

namespace AdoWithCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly ILogger<PhotoController> _logger;
        private readonly RepositoryContext context;
        private readonly ContextAdapter _contextAdapter;

        public PhotoController(ILogger<PhotoController> logger, RepositoryContext repositoryContext, ContextAdapter contextAdapter)
        {
            _logger = logger;
            context = repositoryContext;
            _contextAdapter = contextAdapter;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                DataSet dataSet = _contextAdapter.DataReadQuery("SELECT * FROM PHOTO");

                List<Photo> photos = PhotoMapper.fromDataSetToList(dataSet.Tables["Table"]);

                Dictionary<string, List<Photo>> response = new()
                {
                    { "Photos", photos }
                };

                ResponseHelper rh = new ResponseHelper();

                return Ok(ResponseHelper.ToJson(response));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/{path}")]
        public ActionResult GetPhotoByPath(String path)
        {
            try
            {
                _contextAdapter.AddQueryParam("@Path", path);
                _contextAdapter.SetCommandType(ContextAdapter.CommandType.PROCEDURE);
                DataSet dataSet = _contextAdapter.DataReadQuery("getPhotoByPath");

                List<Photo> photos = PhotoMapper.fromDataSetToList(dataSet.Tables["Table"]);
                Dictionary<string, List<Photo>> response = new()
                {
                    { "Photos", photos }
                };
               
                return Ok(ResponseHelper.ToJson(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{path}")]
        public ActionResult GetPhotoByName(string path)
        {
            string query = "getPhotoByPath";

            context.SetCommandType(RepositoryContext.CommandType.PROCEDURE);
            context.AddQueryParam("@PathName", path);
            SqlDataReader dataReader = context.DataReadQuery(query);

            PhotoMapper mapper = new PhotoMapper() { DataReader = dataReader };
            List<Photo> photos = mapper.ToList();

            context.Clean();

            Dictionary<string, List<Photo>> response = new Dictionary<string, List<Photo>>();

            response.Add("Photos", photos);

            ResponseHelper rh = new ResponseHelper();

            return Ok(ResponseHelper.ToJson(response));

        }

        [HttpPost]
        public ActionResult Post([FromBody] PhotoCreateDAO photo)
        {

            string guery = "INSERT INTO PHOTO(Path,CreatedAt,UpdatedAt,GalleryId) VALUES(@PhotoPath,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,@GalleryId)";

            try
            {
                context.AddQueryParam("@PhotoPath", photo.Path);
                context.AddQueryParam("@GalleryId", photo.GalleryId);
                SqlWriteResponse response = context.DataWriteQuery(guery);
                context.Clean();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPatch]
        public ActionResult Update([FromBody] PhotoUpdateDAO photo)
        {

            string query = $"update Photo set Path = @PhotoPath ,GalleryId = @GalleryId where Id = @PhotoId";

            context.AddQueryParam("@PhotoPath", photo.Path);
            context.AddQueryParam("@GalleryId", photo.GalleryId);
            context.AddQueryParam("@PhotoId", photo.Id);
            SqlWriteResponse response = context.DataWriteQuery(query);

            context.Clean();

            return Ok(response);
        }


        [HttpDelete("/{id}")]
        public ActionResult Delete(int id)
        {
            string query = $"delete from Photo where Id = @PhotoId";

            context.AddQueryParam("@PhotoId", id);
            SqlWriteResponse response = context.DataWriteQuery(query);

            context.Clean();

            return Ok(response);
        }
    }
}