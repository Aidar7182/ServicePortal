using Portal.Context;
using Portal.DB;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class LoginController : Controller
    {
        readonly LoginClass loginclass = new LoginClass();
        DBConn conn = new DBConn();

        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);

            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        public void GetUserLogin(string login, string password)
        {
            //InterVar intervar = new InterVar();
            string fio = "";
            string role = "";
            string id = "";
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select Employee.FullName, Role.UserRole, Employee.ID from Employee 
                                        JOIN Login ON Employee.ID = Login.EmployeeID
                                        JOIN Role ON Login.UserRoleID = Role.ID
                                        where  Login.Login = '" + login + "' and Login.Password = '" + password+"'";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
   
                        fio = (Convert.ToString(r["FullName"]));
                        role = (Convert.ToString(r["UserRole"]));
                        id = (Convert.ToString(r["ID"]));

                    }
                }
                AddOrUpdateAppSettings("fio", fio);
                AddOrUpdateAppSettings("role", role);
                AddOrUpdateAppSettings("id", id);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.connect().Close();
            }
        }
        private List<SelectListItem> PopulateRole()
        {
            try
            {
                List<Role> rolelist = new List<Role>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from Role";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getRole = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        getRole.Add(new SelectListItem
                        {
                            Text = @dr["UserRole"].ToString(),
                            Value = @dr["ID"].ToString()
                        });
                    }

                    return getRole;
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
        private List<SelectListItem> PopulateStatus()
        {
            try
            {
                List<Status> rolestatus = new List<Status>();
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select * from Status";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getStatus = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        getStatus.Add(new SelectListItem
                        {
                            Text = @dr["UserStatus"].ToString(),
                            Value = @dr["ID"].ToString()
                        });
                    }

                    return getStatus;
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
        private List<SelectListItem> PopulateEmployee()
        {
            try
            {
                conn.connect().Open();
                using (SQLiteCommand fmd = conn.connect().CreateCommand())
                {
                    fmd.CommandText = @"select Employee.ID, Employee.FullName from Employee where not exists (select * from Login where Login.EmployeeID = Employee.ID)";
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(fmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ViewBag.text = ds.Tables[0];

                    List<SelectListItem> getStatus = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.text.Rows)
                    {
                        getStatus.Add(new SelectListItem
                        {
                            Text = @dr["FullName"].ToString(),
                            Value = @dr["ID"].ToString()
                        });
                    }

                    return getStatus;
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

        // GET:
        public ActionResult Login()
        {
            return View();
        }


        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login thelogin)
        {
            InterVar intervar = new InterVar();
            try
            {
                if (ModelState.IsValid)
                {
                   
                    List<Login> loginsList = loginclass.GetLogins().ToList();
                    Login result = null;
                    result = loginsList.Find(x => x.Loginn == thelogin.Loginn && x.Password == thelogin.Password);
                    if (result != null)
                    {
                        FormsAuthentication.SetAuthCookie(thelogin.Loginn, true);
                        GetUserLogin(thelogin.Loginn, thelogin.Password);


                        return RedirectToRoute(new { controller = "Login", action = "Login" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль");
                    }

                }
                return View(thelogin);
            }
            catch
            {
                return View();
            }
        }

        //[Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        // GET: Login
        [Authorize]
        public ActionResult Index()
        {
            List<Login> loginList = loginclass.GetLogins().ToList();
            return View(loginList);
        }

        //// GET: LoginsController/Create
        public ActionResult Create()
        {
            Login login = new Login();
            login.LRole = PopulateRole();
            login.LStatus = PopulateStatus();
            login.LEmployee = PopulateEmployee();
            
            
            return View(login);
        }

        // POST: LoginsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Login thelogin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<Login> loginsList = loginclass.GetLogins().ToList();
                    Login result = null;
                    result = loginsList.Find(x => x.Loginn == thelogin.Loginn);
                    if (result == null)
                    {
                        thelogin.LRole = PopulateRole();
                        thelogin.LStatus = PopulateStatus();
                        thelogin.LEmployee = PopulateEmployee();
                        var selectedRole = thelogin.LRole.Find(p => p.Value == thelogin.RoleID.ID.ToString());
                        var selectedStatus = thelogin.LStatus.Find(x => x.Value == thelogin.StatusID.ID.ToString());
                        var selectedEmployee = thelogin.LEmployee.Find(x => x.Value == thelogin.EmployeeID.ID.ToString());
                        if (selectedRole != null && selectedStatus != null && selectedEmployee != null)
                        {
                            selectedRole.Selected = true;
                            selectedStatus.Selected = true;
                            selectedEmployee.Selected = true;
                            thelogin.RoleID.ID = Convert.ToInt32(selectedRole.Value);
                            thelogin.StatusID.ID = Convert.ToInt32(selectedStatus.Value);
                            thelogin.EmployeeID.ID = Convert.ToInt32(selectedEmployee.Value);
                            loginclass.CreateLogin(thelogin);
                            //FormsAuthentication.SetAuthCookie(thelogin.Loginn, true);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                    }

                }
                return View(thelogin);
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginsController/Edit/5
        public ActionResult Edit(int id)
        {
            //if (id < 0)
            //{
            //    return HttpNotFound();
            //}
            //Login login = new Login();
            Login login = loginclass.GetLoginByID(id);
            
            login.LRole = PopulateRole();
            login.LStatus = PopulateStatus();
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);

        }

        // POST: LoginsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind] Login thelogin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                        thelogin.LRole = PopulateRole();
                        thelogin.LStatus = PopulateStatus();
                        var selectedRole = thelogin.LRole.Find(p => p.Value == thelogin.RoleID.ID.ToString());
                        var selectedStatus = thelogin.LStatus.Find(x => x.Value == thelogin.StatusID.ID.ToString());
                        if (selectedRole != null || selectedStatus != null)
                        {
                            selectedRole.Selected = true;
                            selectedStatus.Selected = true;
                            thelogin.RoleID.ID = Convert.ToInt32(selectedRole.Value);
                            thelogin.StatusID.ID = Convert.ToInt32(selectedStatus.Value);
                            loginclass.UpdateLogin(thelogin);
                            //FormsAuthentication.SetAuthCookie(thelogin.Loginn, true);
                        }
                        return RedirectToAction("Index");
                   

                }
                return View(thelogin);
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginsController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id < 0)
            {
                return HttpNotFound();
            }
            Login login = loginclass.GetLoginByID(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: LoginsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                loginclass.DeleteLogin(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}