using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : ApiController
    {
        private EmployeeAppContext db = new EmployeeAppContext();

        // GET api/Employee
        /*
        public IEnumerable<Employee> GetEmployees()
        {
            return db.Employees.AsEnumerable();
        }
         * */

        //--------------------------
        public IEnumerable<Employee> GetEmployees(string q = null, string sort = null, bool desc = false,
                                                        int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Employee>();

            IQueryable<Employee> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.FName)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") items = items.Where(t => t.FName.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
        }

        //--------------------------

        // GET api/Employee/5
        public Employee GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return employee;
        }

        // PUT api/Employee/5
        public HttpResponseMessage PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != employee.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Employee
        public HttpResponseMessage PostEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, employee);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = employee.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Employee/5
        public HttpResponseMessage DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Employees.Remove(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}