using Portal.Context;
using Portal.DB;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class EmployeeController : Controller
    {
        readonly EmployeeClass employeeClass = new EmployeeClass();
        public static int val;

        DBConn conn = new DBConn();
        private List<SelectListItem> PopulateDepartment()
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from Department";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getDepartment = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        getDepartment.Add(new SelectListItem
                        {
                            Text = @dr["DepartName"].ToString(),
                            Value = @dr["ID"].ToString()
                        });
                    }

                    return getDepartment;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        private List<SelectListItem> PopulatePosition()
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from Position";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getPosition = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        getPosition.Add(new SelectListItem
                        {
                            Text = @dr["PosName"].ToString(),
                            Value = @dr["ID"].ToString()
                        });
                    }

                    return getPosition;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        private List<SelectListItem> PopulateExtNumber()
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from ExtNumber";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getExtNumber = new List<SelectListItem>();

                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        if (Convert.ToUInt32(@dr["NumbStatus"]) == 0)
                        {
                            getExtNumber.Add(new SelectListItem
                            {
                                Text = @dr["Number"].ToString(),
                                Value = @dr["ID"].ToString()
                            });
                        }
                       
                    }

                    return getExtNumber;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        private List<SelectListItem> PopulateExtNumberEdit(int id)
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from ExtNumber";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    EmployeeClass empClass = new EmployeeClass();
                    Employee emp = empClass.GetEmployeeByID(id);
                    List<SelectListItem> getExtNumber = new List<SelectListItem>();
                    val = emp.ExtNumberID.ID;

                    getExtNumber.Add(new SelectListItem
                    {
                        Text = emp.ExtNumberID.Number.ToString(),
                        Value = emp.ExtNumberID.ID.ToString()
                    });
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        if (Convert.ToUInt32(@dr["NumbStatus"]) == 0)
                        {
                            getExtNumber.Add(new SelectListItem
                            {
                                Text = @dr["Number"].ToString(),
                                Value = @dr["ID"].ToString()
                            });
                        }

                    }

                    return getExtNumber;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        private List<SelectListItem> PopulateEmpStatus()
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from EmployeeStatus";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getEmpStatus = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                       getEmpStatus.Add(new SelectListItem
                       {
                            Text = @dr["Status"].ToString(),
                            Value = @dr["ID"].ToString()
                       });
   
                    }

                    return getEmpStatus;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }


        // GET: Employee

        public ActionResult Index()
        {
            List<Employee> employeeList = employeeClass.GetEmployees().ToList();
            return View(employeeList);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            Employee emp = new Employee();
            emp.LDepartment = PopulateDepartment();
            emp.LPosition = PopulatePosition();
            emp.LExtNumber = PopulateExtNumber();
            return View(emp);
        }

        //POST: LoginsController/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Employee emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<Employee> loginsList = employeeClass.GetEmployees().ToList();
                    emp.LDepartment = PopulateDepartment();
                    emp.LPosition = PopulatePosition();
                    emp.LExtNumber = PopulateExtNumber();
                    var selectedDepartment = emp.LDepartment.Find(p => p.Value == emp.DepartmentID.ID.ToString());
                    var selectedPosition = emp.LPosition.Find(x => x.Value == emp.PositionID.ID.ToString());
                    var selectedExtNumber = emp.LExtNumber.Find(x => x.Value == emp.ExtNumberID.ID.ToString());
                    if (selectedDepartment != null && selectedPosition != null && selectedExtNumber != null)
                    {
                        selectedDepartment.Selected = true;
                        selectedPosition.Selected = true;
                        selectedExtNumber.Selected = true;
                        emp.DepartmentID.ID = Convert.ToInt32(selectedDepartment.Value);
                        emp.PositionID.ID = Convert.ToInt32(selectedPosition.Value);
                        emp.ExtNumberID.ID = Convert.ToInt32(selectedExtNumber.Value);
                        employeeClass.CreateEmployee(emp);
                    }
                    return RedirectToAction("Index");



                }
                return View(emp);
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {

            Employee emp = employeeClass.GetEmployeeByID(id);
            emp.LDepartment = PopulateDepartment();
            emp.LPosition = PopulatePosition();
            emp.LExtNumber = PopulateExtNumberEdit(id);
            emp.LEmpStatus = PopulateEmpStatus();
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);

        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind] Employee emp)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    emp.LDepartment = PopulateDepartment();
                    emp.LPosition = PopulatePosition();
                    emp.LExtNumber = PopulateExtNumberEdit(emp.ID);
                    emp.LEmpStatus = PopulateEmpStatus();
                    var selectedDepartment = emp.LDepartment.Find(p => p.Value == emp.DepartmentID.ID.ToString());
                    var selectedPosition = emp.LPosition.Find(x => x.Value == emp.PositionID.ID.ToString());
                    var selectedExtNumber = emp.LExtNumber.Find(x => x.Value == emp.ExtNumberID.ID.ToString());
                    var selectedEmpStatus = emp.LEmpStatus.Find(x => x.Value == emp.StatusID.ID.ToString());
                    if (selectedDepartment != null && selectedPosition != null && selectedExtNumber != null && selectedEmpStatus != null)
                    {
                        selectedDepartment.Selected = true;
                        selectedPosition.Selected = true;
                        selectedExtNumber.Selected = true;
                        selectedEmpStatus.Selected = true;
                        emp.DepartmentID.ID = Convert.ToInt32(selectedDepartment.Value);
                        emp.PositionID.ID = Convert.ToInt32(selectedPosition.Value);
                        emp.ExtNumberID.ID = Convert.ToInt32(selectedExtNumber.Value);
                        emp.StatusID.ID = Convert.ToInt32(selectedEmpStatus.Value);
                        employeeClass.UpdateEmployee(emp);
                    }
                    return RedirectToAction("Index");


                }
                return View(emp);
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id < 0)
            {
                return HttpNotFound();
            }
            Employee emp = employeeClass.GetEmployeeByID(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                employeeClass.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}