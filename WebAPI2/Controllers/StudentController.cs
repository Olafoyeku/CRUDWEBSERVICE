using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class StudentController : ApiController
    {

        // [Route("api/student/GetAll")]

      

        [HttpGet]
        [ActionName("GetAllStudents")]
        public IHttpActionResult GetAllStudents()
    {
            try
            {



                IList<StudentViewModel> students = null;

                using (var x = new PracticeEntities())
                {
                    students = x.Students.Select(s => new StudentViewModel
                    {


                        StudentID = s.StudentID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Age = s.Age

                    }).ToList<StudentViewModel>();

                    if (students.Count() == 0)
                    {
                        return NotFound();
                    }
                    StudentViewModel stx = new StudentViewModel();
                   stx = students.Last();

                    

                    return Ok(stx);

                }
            } 
            catch(Exception ex)
            {
                throw ex;
            }

    }
        [HttpGet]
        [ActionName("GetStudentByID")]
        public IHttpActionResult GetStudentByID(int? id)
        {
            try
            {



                if (id != null)
                {

                    StudentViewModel student = null;


                    using (var x = new PracticeEntities())
                    {
                        student = x.Students.Where(z => z.StudentID == id).Select(c => new StudentViewModel
                        {
                            StudentID = c.StudentID,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Age = c.Age

                        }).FirstOrDefault<StudentViewModel>();

                        if (student == null)
                        {
                            return NotFound();
                        }

                        return Ok(student);
                    }

                }

                return BadRequest("Id cannot be null");

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [ActionName("GetByName")]
        public IHttpActionResult GetByName()
        {
            try
            {

                string name = "hannah";

                if (name != null)
                {

                    StudentViewModel student = null;


                    using (var x = new PracticeEntities())
                    {
                        student = x.Students.Where(z => z.FirstName.ToLower().Contains(name.ToLower()) || z.LastName.ToLower().Contains(name.ToLower()))
                            .Select(c => new StudentViewModel
                        {
                            StudentID = c.StudentID,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Age = c.Age

                        }).FirstOrDefault<StudentViewModel>();

                        if (student == null)
                        {
                            return NotFound();
                        }

                        return Ok(student);
                    }

                }

                return BadRequest("Id cannot be null");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ActionName("CreateStudent")]
        public IHttpActionResult CreateStudent(StudentViewModel student)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");

                using (var x = new PracticeEntities())
                {
                    x.Students.Add(new Student()
                    {
                        StudentID = student.StudentID,
                        Age = student.Age,
                        FirstName = student.FirstName,
                        LastName = student.LastName
                    });
                    x.SaveChanges();

                }

                return Ok();

            }catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [ActionName("UpdateStudent")]
        public IHttpActionResult UpdateStudent(int? id, StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }
            if(id == null)
            {
                return BadRequest("Id cannot be null");
            }
            using (var x = new PracticeEntities())
            {
                var studentexist = x.Students.Where(n => n.StudentID == id).FirstOrDefault();

                if(studentexist != null)
                {
                    studentexist.FirstName = student.FirstName;
                    studentexist.LastName = student.LastName;
                    studentexist.Age = student.Age;

                    x.SaveChanges();

                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();

        }

        [HttpPost]
        [ActionName("DeleteStudent")]
        public IHttpActionResult DeleteStudent(int? id)
        {
            if(id > 0)
            {
                using (var x  = new PracticeEntities())
                {

                    var todelete = x.Students.Where(s => s.StudentID == id).FirstOrDefault();

                    if(todelete != null)
                    {
                        x.Entry(todelete).State = System.Data.Entity.EntityState.Deleted;
                        x.SaveChanges();
                    }

                }
                return Ok();


            }

            return BadRequest("Id cannot be null");


        }
    }
}
