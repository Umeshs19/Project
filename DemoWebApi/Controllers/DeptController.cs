using DemoWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoWebApi.ViewModel;

namespace DemoWebApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        db1064Context db = new db1064Context();
        [HttpGet]
        [Route("ShowInfo")]
        public IActionResult GetDeptInfo()
        {
            var data = db.DeptInfo_VMs.FromSqlInterpolated<DeptInfo_VM>($"DeptInfo");
            return Ok(data);
        }

        [HttpGet]
        [Route("ListDept")]
        public IActionResult GetDept()
        {
            //var data = db.Depts.ToList();
            var data = from d in db.Depts select d;

            //var data = from d in db.Depts select new { Code = d.Id, DeptName = d.Name, City = d.Location };
            return Ok(data);
        }
        [HttpGet]
        [Route("ListDept/{id}")]
        public IActionResult GetDept(int? id)
        {
            if(id==null)
            {
                return BadRequest("Id cannot be Null");
            }
            var data = (from d in db.Depts where d.Id == id select d).FirstOrDefault();
            if(data==null)
            {
                return NotFound($"Department {id} not present");
            }
            return Ok(data);
        }
        //....dept/listcity?city=Mumbai
        [HttpGet]
        [Route("ListCity")]
        public IActionResult GetCity([FromQuery] string city)
        {
            var data = db.Depts.Where(d => d.Location == city);
            if(data.Count()==0)
            {
                return NotFound($"No Department Present in {city} ");
            }
            return Ok(data);
        }
        //....dept/listloc?dname=IT&city=Mumbai 
        [HttpGet]
        [Route("ListLoc")]
        public IActionResult GetCLoc([FromQuery] string dname,string city)
        {
            var data = from d in db.Depts where d.Name == dname &&  d.Location == city select d;
            if (data.Count() == 0)
            {
                return NotFound($"No Department {dname} Present in {city} ");
            }
            return Ok(data);

        }
        [HttpPost]
        [Route("AddDept")]
        public IActionResult PostDept(Dept dept)
        {
               if(ModelState.IsValid)
               {
                    try
                    {
                    //db.Depts.Add(dept);
                    //db.SaveChanges();
                    //Here we will be calling the Stored Procedures
                    db.Database.ExecuteSqlInterpolated($"deptadd {dept.Id}, {dept.Name}, {dept.Location}");
                    }
                    catch(System.Exception ex)
                    {
                    return BadRequest(ex.InnerException.Message);
                    }
               }
            return Created("Record Successfully Added", dept);
        }
        [HttpPut]
        [Route("EditDept/{id}")]
        public IActionResult PutDept(int id,Dept dept)
        {
            if(ModelState.IsValid)
            {
                Dept odept = db.Depts.Find(id);
                odept.Name = dept.Name;
                odept.Location = dept.Location;
                db.SaveChanges();
                return Ok();    
            }
            return BadRequest("Unable to Edit Record");
        }
        [HttpDelete]
        [Route("DeleteDept/{id}")]
        public IActionResult DeleteDept(int id)
        {
            try
            {
                var data = db.Depts.Find(id);
                db.Depts.Remove(data);
                db.SaveChanges();
                return Ok();
            }
            catch(System.Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

    }
}
