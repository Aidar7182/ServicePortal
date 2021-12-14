using Portal.Controllers;
using Portal.DB;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace Portal.Context
{
    public class SupervisorClass
    {
        readonly DBConn conn = new DBConn();

        //ABSENCE
        public List<AbsenseLog> GetAbsenseLogWorkers()
        {
            try
            {

                List<AbsenseLog> list = new List<AbsenseLog>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select AbsenceLog.ID, AbsenceLog.EmployeeID, Employee.FullName, Department.DepartName, AbsenceLog.Reason, AbsenceLog.Date, AbsenceLog.Status  from AbsenceLog
                                        JOIN Employee ON Employee.ID = AbsenceLog.EmployeeID
                                        JOIN Department ON Department.ID = Employee.DepartmentID
                                        where AbsenceLog.Status = " + "'На рассмотрении'";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        AbsenseLog obj = new AbsenseLog();
                        obj.ID = Convert.ToInt32(r["ID"]);
                        obj.EmployeeID = Convert.ToInt32(r["EmployeeID"]);
                        obj.FullName = (Convert.ToString(r["FullName"]));
                        obj.DepartName = (Convert.ToString(r["DepartName"]));
                        obj.Reason = (Convert.ToString(r["Reason"]));
                        obj.Date = (Convert.ToString(r["Date"]));
                        obj.Status = (Convert.ToString(r["Status"]));
                        list.Add(obj);
                    }

                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        public List<AbsenseLog> GetAbsenseLogWorkersArchive()
        {
            try
            {

                List<AbsenseLog> list = new List<AbsenseLog>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select AbsenceLog.ID, AbsenceLog.EmployeeID, Employee.FullName, Department.DepartName, AbsenceLog.Reason, AbsenceLog.Date, AbsenceLog.Status  from AbsenceLog
                                        JOIN Employee ON Employee.ID = AbsenceLog.EmployeeID
                                        JOIN Department ON Department.ID = Employee.DepartmentID
                                        where AbsenceLog.Status = " + "'Отклонено'"+ " or AbsenceLog.Status = " + "'Утверждено'";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        AbsenseLog obj = new AbsenseLog();
                        obj.ID = Convert.ToInt32(r["ID"]);
                        obj.EmployeeID = Convert.ToInt32(r["EmployeeID"]);
                        obj.FullName = (Convert.ToString(r["FullName"]));
                        obj.DepartName = (Convert.ToString(r["DepartName"]));
                        obj.Reason = (Convert.ToString(r["Reason"]));
                        obj.Date = (Convert.ToString(r["Date"]));
                        obj.Status = (Convert.ToString(r["Status"]));
                        list.Add(obj);
                    }

                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        public void UpdateAbsense(AbsenseLog obj)
        {


            using (SQLiteConnection connection = conn.connect())
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "update AbsenceLog set Status =:status where ID=:id";
                    command.Parameters.Add("status", DbType.String).Value = obj.Status;
                    command.Parameters.Add("id", DbType.String).Value = obj.ID;
             

                    command.ExecuteNonQuery();
                }

            }

        }

        //HOLIDAY
        public List<Holiday> GetHolidayWorkers()
        {
            try
            {

                List<Holiday> list = new List<Holiday>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select Holiday.ID, Holiday.EmployeeID, Employee.FullName, Department.DepartName, Holiday.FromDate, Holiday.Day, Holiday.Status  from Holiday
                                        JOIN Employee ON Employee.ID = Holiday.EmployeeID
                                        JOIN Department ON Department.ID = Employee.DepartmentID
                                        where Holiday.Status = " + "'На рассмотрении'";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        Holiday obj = new Holiday();
                        obj.ID = Convert.ToInt32(r["ID"]);
                        obj.EmployeeID = Convert.ToInt32(r["EmployeeID"]);
                        obj.FullName = (Convert.ToString(r["FullName"]));
                        obj.DepartName = (Convert.ToString(r["DepartName"]));
                        obj.FromDate = (Convert.ToString(r["FromDate"]));
                        obj.Day = (Convert.ToInt32(r["Day"]));
                        obj.Status = (Convert.ToString(r["Status"]));
                        list.Add(obj);
                    }

                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        public List<Holiday> GetHolidayWorkersArchive()
        {
            try
            {

                List<Holiday> list = new List<Holiday>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select Holiday.ID, Holiday.EmployeeID, Employee.FullName, Department.DepartName, Holiday.FromDate, Holiday.Day, Holiday.Status  from Holiday
                                        JOIN Employee ON Employee.ID = Holiday.EmployeeID
                                        JOIN Department ON Department.ID = Employee.DepartmentID
                                        where Holiday.Status = " + "'Отклонено'" + " or Holiday.Status = " + "'Утверждено'";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        Holiday obj = new Holiday();
                        obj.ID = Convert.ToInt32(r["ID"]);
                        obj.EmployeeID = Convert.ToInt32(r["EmployeeID"]);
                        obj.FullName = (Convert.ToString(r["FullName"]));
                        obj.DepartName = (Convert.ToString(r["DepartName"]));
                        obj.FromDate = (Convert.ToString(r["FromDate"]));
                        obj.Day = (Convert.ToInt32(r["Day"]));
                        obj.Status = (Convert.ToString(r["Status"]));
                        list.Add(obj);
                    }

                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.connect().Close();
            }
        }

        public void UpdateHoliday(Holiday obj)
        {


            using (SQLiteConnection connection = conn.connect())
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "update Holiday set Status =:status where ID=:id";
                    command.Parameters.Add("status", DbType.String).Value = obj.Status;
                    command.Parameters.Add("id", DbType.String).Value = obj.ID;


                    command.ExecuteNonQuery();
                }

            }

        }

    }
}