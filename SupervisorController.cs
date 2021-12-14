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
    public class SupervisorController : Controller
    {
        readonly SupervisorClass supervisorClass  = new SupervisorClass();
        readonly WorkerClass workerClass = new WorkerClass();
        readonly EmployeeClass employeeClass = new EmployeeClass();
        public static int val;

        DBConn conn = new DBConn();


        // GET: AbsenseList

        public ActionResult AbsenseList()
        {
            AbsenseLog objs = new AbsenseLog();
            objs.absence = supervisorClass.GetAbsenseLogWorkers().ToList();
            return View(objs);

        }

        public ActionResult AbsenseListArchive()
        {
            AbsenseLog objs = new AbsenseLog();
            objs.absence = supervisorClass.GetAbsenseLogWorkersArchive().ToList();
            return View(objs);

        }

        // GET: SupervisorController/AbsenseConfirm/5
        public ActionResult AbsenseConfirm(int id, int emp)
        {
            AbsenseLog objs = new AbsenseLog();

            objs.absence = workerClass.GetAbsenseLogByWorkerID(emp).ToList();
            return View(objs);

        }

        // POST: SupervisorController/AbsenseConfirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AbsenseConfirm([Bind] AbsenseLog obj)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    supervisorClass.UpdateAbsense(obj);
                    return RedirectToAction("AbsenseList");


                }
                return View(obj);



            }
            catch
            {
                return View();
            }
        }



        // GET: HolidayList
        public ActionResult HolidayList()
        {
            Holiday objs = new Holiday();
            objs.holidays = supervisorClass.GetHolidayWorkers().ToList();
            return View(objs);

        }

        public ActionResult HolidayListArchive()
        {
            Holiday objs = new Holiday();
            objs.holidays = supervisorClass.GetHolidayWorkersArchive().ToList();
            return View(objs);

        }

        // GET: SupervisorController/HolidayConfirm/5
        public ActionResult HolidayConfirm(int id, int emp)
        {
            Holiday objs = new Holiday();

            objs.holidays = workerClass.GetHolidayByWorkerID(emp).ToList();
            return View(objs);

        }

        // POST: SupervisorController/HolidayConfirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HolidayConfirm([Bind] Holiday obj)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    supervisorClass.UpdateHoliday(obj);
                    return RedirectToAction("HolidayList");


                }
                return View(obj);



            }
            catch
            {
                return View();
            }
        }


    }
}