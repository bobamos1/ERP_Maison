using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ERP_Maison.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public MainController (IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                string query = @"
                    SELECT id, No_Project FROM Tbl_Projects
                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AssyAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand=new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                return new JsonResult(table);
            }
            catch (Exception ex)
            {
                return new JsonResult("error");
            }
        }

        [HttpPost]
        public JsonResult Post(Dictionary<string, object> dict)
        {
            try
            {
                string query = @"
                    INSERT INTO Tbl_Projects (No_Project) VALUES (@No_Project)
                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AssyAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        foreach (KeyValuePair<string, object> pair in dict)
                            myCommand.Parameters.AddWithValue("@" + pair.Key, pair.Value);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                return new JsonResult("Insert Succes");
            }
            catch (Exception ex)
            {
                //"\n\n\n" + proj.ProjectName;
                return new JsonResult(ex);
            }
        }

        [HttpPut]
        public JsonResult Put(Dictionary<string, object> dict)
        {
            try
            {
                string query = @"
                    UPDATE Tbl_Projects SET No_Project = @No_Project WHERE id = @id
                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AssyAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        foreach (KeyValuePair<string, object> pair in dict)
                            myCommand.Parameters.AddWithValue("@" + pair.Key, pair.Value);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                return new JsonResult("Update Succes");
            }
            catch (Exception ex)
            {
                return new JsonResult("error");
            }
        }

        [HttpDelete]
        public JsonResult Delete(long id)
        {
            try
            {
                string query = @"
                    Delete Tbl_Projects WHERE id = @id
                ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AssyAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                return new JsonResult("Delete Succes");
            }
            catch (Exception ex)
            {
                return new JsonResult("error");
            }
        }
         private JsonResult doQuery(Dictionary<string, object> dict)
        {
            /*
            string query = "";
            foreach (KeyValuePair<string, object> pair in dict)
                if (pair.Key == "query")
                {
                    query = pair.Value.ToString();
                    break;
                }
            if (query == "")
                return new JsonResult("No Query");
            try
            {
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("AssyAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        foreach (KeyValuePair<string, object> pair in dict)
                            myCommand.Parameters.AddWithValue("@" + pair.Key, pair.Value);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                return new JsonResult("Insert Succes");
            }
            catch (Exception ex)
            {
                //"\n\n\n" + proj.ProjectName;
                return new JsonResult(ex);
            }
            */
            return new JsonResult("");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath+"/Images/"+fileName;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex);
            }
        }
    }
}
