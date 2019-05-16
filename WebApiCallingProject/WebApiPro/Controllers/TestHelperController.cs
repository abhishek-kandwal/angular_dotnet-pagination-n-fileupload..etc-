using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using WebApiPro.Models;
using DBHelper;
using Microsoft.AspNetCore.Cors;
using System.IO;
using System.Net.Http.Headers;
using WebApiPro.Model;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class TestHelperController : ControllerBase
    {
        private bool whereRequired;

        // GET api/values
        [HttpGet(Name = "GetUsersDetails")]
        public List<DBDataModel> GetUsersDetails()
        {
            DataTable DataTbl = new DatabaseHelper().GetResults("SELECT * FROM UserRegistration");
           
            //return Studenttbl;
            //DataTable Studenttbl = new DBHelper().StoreProcedureQuery("AllData");
            //DataTable Studenttbl = new DBHelper.DBHelper().StoreProcedureQuery("InsertValue", new { Name = "raj sahu", Address = "newYork", PhoneNo = 8218178307, Email = "abhishek", State = "Downtown", Approved = 0 });
            //DataTable Studenttbl = new DBHelper.DBHelper().StoreProcedureQuery("InsertValue", new { Name = "raj sahu" });

            List<DBDataModel> listObj = new List<DBDataModel>();
            DBDataModel obj = null;
            foreach (DataRow DRow in DataTbl.Rows)
            {
                obj = new DBDataModel()
                {
                    Name = DRow["Name"].ToString(),
                    Address = DRow["Address"].ToString(),
                    PhoneNo = Convert.ToInt64(DRow["PhoneNo"]),
                    Email = DRow["Email"].ToString(),
                    State = DRow["State"].ToString(),
                    Approved = Convert.ToBoolean(DRow["Approved"])
                };
                listObj.Add(obj);
            }

            return listObj;
        }

        // GET api/values/5
        [HttpGet(Name = "GetUserDetail")]
        public List<DBDataModel> GetUserDetail([FromQuery] DBDataModel dBDataModel) // object value
        {
            string query = "SELECT * FROM UserRegistration where";
            bool whereRequired = false;

            if (!string.IsNullOrWhiteSpace(dBDataModel.Name))
            {
                query += " Name = '" + dBDataModel.Name + "'" ;
                whereRequired = true;
            }

            if (!string.IsNullOrWhiteSpace(dBDataModel.Address))
            {
                if (whereRequired)
                {
                    query += "AND";
                }
                query += " Address = '" + dBDataModel.Address + "'";
                whereRequired = true;
            }

            if ((dBDataModel.PhoneNo != 0))
            {
                if (whereRequired)
                {
                    query += "AND";
                }
                query += " PhoneNo = '" + dBDataModel.PhoneNo + "'";
                whereRequired = true;
            }

            if (!string.IsNullOrWhiteSpace(dBDataModel.Email))
            {
                if (whereRequired)
                {
                    query += "AND";
                }
                query += " Email = '" + dBDataModel.Email + "'";
                whereRequired = true;
            }

            if (!string.IsNullOrWhiteSpace(dBDataModel.State))
            {
                if (whereRequired)
                {
                    query += "AND";
                }
                query += " State = '" + dBDataModel.State + "'";
                whereRequired = true;
            }

            if ((dBDataModel.Approved== true) || (dBDataModel.Approved == false))
            {
                if (whereRequired)
                {
                    query += "AND";
                }
                query += " Approved = '" + dBDataModel.Approved + "'";
            }

            DataTable Studenttbl = new DatabaseHelper().GetResults(query);
            
            
            // DataTable Studenttbl = new DBHelper().StoreProcedureSearchQuery("NewData", value);
            List<DBDataModel> listObj = new List<DBDataModel>();
            DBDataModel obj = null;
            foreach (DataRow DRow in Studenttbl.Rows)
            {
                obj = new DBDataModel()
                {
                    Name = DRow["Name"].ToString(),
                    Address = DRow["Address"].ToString(),
                    PhoneNo = Convert.ToInt64(DRow["PhoneNo"]),
                    Email = DRow["Email"].ToString(),
                    State = DRow["State"].ToString(),
                    Approved = Convert.ToBoolean(DRow["Approved"])
                };
                listObj.Add(obj);
            }

            return listObj;

        }

        [HttpGet(Name = "PostDetails")]
        public void PostDetails([FromQuery] DBDataModel dBDataModel)
        {
            var dBHelper = new DatabaseHelper();
            string sqlQuery = "INSERT INTO UserRegistration VALUES ('" + dBDataModel.Name + "' , '" + dBDataModel.Address + "','" + dBDataModel.PhoneNo + "','" + dBDataModel.Email + "','" + dBDataModel.State + "','" + dBDataModel.Approved + "' )"; 
            dBHelper.PostValues(sqlQuery);   
        }

        //// POST api/values for stored procedure
        //[HttpPost]
        //public void Post(object value)
        //{
        //    new DBHelper().StoreProcedureOtherQuery("InsertValue", value);
        //}

        // PUT api/values/5

        [HttpGet(Name = "EditValueDetails")]
        public List<DBDataModel> EditValueDetails([FromQuery] DBDataModel dBDataModel)
        {
            DataTable Studenttbl = new DatabaseHelper().GetResults("SELECT * FROM UserRegistration where name = '" + dBDataModel.Name + "'");

            List<DBDataModel> listObj = new List<DBDataModel>();
            DBDataModel obj = null;
            foreach (DataRow DRow in Studenttbl.Rows)
            {
                obj = new DBDataModel()
                {
                    Name = DRow["Name"].ToString(),
                    Address = DRow["Address"].ToString(),
                    PhoneNo = Convert.ToInt64(DRow["PhoneNo"]),
                    Email = DRow["Email"].ToString(),
                    State = DRow["State"].ToString(),
                    Approved = Convert.ToBoolean(DRow["Approved"])
                };
                listObj.Add(obj);
            }

            return listObj;
        }

        [HttpPut("{id}")]
        public void Put(String Name, [FromBody] string value)
        {
            new DatabaseHelper().StoreProcedureOtherQuery("UpdateValue", value );
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(object value)
        {
            new DatabaseHelper().StoreProcedureOtherQuery("InsertValue", value);
        }

        [HttpPost(Name = "Upload"), DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Brouchure");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { fullPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(Name = "GetCategory")]
        public DataTable GetCategory()
        {
            DataTable dtUsers = new DatabaseHelper().GetResults("SELECT * FROM ItemMaster");            return dtUsers;
        }

        [HttpPost(Name = "SaveCategoryAddress")]
        public void SaveCategoryAddress([FromBody] ItemAddressModel data)
        {
            DataTable dtUsers = new DatabaseHelper().PostValues("INSERT INTO ItemsAddressTable VALUES ('" + data.ItemName + "' , '" + data.Address  + "')");
            return;
        }
    }
}