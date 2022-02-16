using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POC.Models;



namespace POC.Controllers
{


    public class ServiceController : Controller
    {

        private PocContext database = new PocContext();
        private UserManager<Employee> userManager;

        public ServiceController(UserManager<Employee> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var branch = (from emp in database.Employees
                          where emp.Id == userManager.GetUserId(User)
                          select new
                          {
                              Branchid = emp.Branchid,
                              data = (from Branch in database.Branches
                                      where Branch.Branchid == emp.Branchid
                                      select new
                                      {
                                          brachName = Branch.Name,
                                          locationName = (from location in database.Locations
                                                          where location.LocationId == Branch.Locationid
                                                          select location.Name).FirstOrDefault()
                                      }).FirstOrDefault()
                          }).FirstOrDefault();

            var data = new Dictionary<string, string>();
            data.Add("BranchName", branch.data.brachName);
            data.Add("LocationName", branch.data.locationName.ToString());
            ViewData["data"] = data;
            ViewData["servicetypes"] = (from service in database.ServiceTypes
                                        select new
                                        {
                                            number = service.ServiceTypeId,
                                            name = service.Name
                                        }).ToDictionary(t => t.number, t => t.name);
            ViewData["resolved"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Index(int Id, int Type)
        {
            string empid = userManager.GetUserId(User);
            int branchid;
            bool rs = int.TryParse(
                    (from emp in database.Employees
                     where emp.Id == empid
                     select emp.Branchid).FirstOrDefault().ToString(), out branchid);
            if (rs && Id < 100000)
            {
                database.Services.Add(
                            new Service()
                            {
                                ServicetypeId = Type,
                                Clientid = Id,
                                EmployeeId = empid
                            }
                            );


                database.Branches.Where(p => p.Branchid == branchid)
                    .First().Numberofservices++;
            }
            else
            {
                ViewData["resolved"] = true;
                ViewData["status"] = "Client number must be only number with maximum of 6 digits";
                return View();
            }
            ViewData["status"] = (database.SaveChanges() != 0) ? "Service added succesfully" : "Failed to add service";
            var branch = (from emp in database.Employees
                          where emp.Id == userManager.GetUserId(User)
                          select new
                          {
                              Branchid = emp.Branchid,
                              data = (from Branch in database.Branches
                                      where Branch.Branchid == emp.Branchid
                                      select new
                                      {
                                          brachName = Branch.Name,
                                          locationName = (from location in database.Locations
                                                          where location.LocationId == Branch.Locationid
                                                          select location.Name).FirstOrDefault()
                                      }).FirstOrDefault()
                          }).FirstOrDefault();
            var data = new Dictionary<string, string>();
            data.Add("BranchName", branch.data.brachName);
            data.Add("LocationName", branch.data.locationName.ToString());
            ViewData["data"] = data;
            ViewData["servicetypes"] = (from service in database.ServiceTypes
                                        select new
                                        {
                                            number = service.ServiceTypeId,
                                            name = service.Name
                                        }).ToDictionary(t => t.number, t => t.name);
            ViewData["resolved"] = true;
            return View();
        }

        public IActionResult GridView()
        {
            var services = from service in database.Services
                           select new
                           {
                               Type = service.Servicetype.Name,
                               ClientID = service.Clientid,
                               EmployeeId = service.EmployeeId,
                               DateCreated = service.DateCreated.ToString("yyyy-MMM-dd")
                           };
            ViewData["services"] = services;
            return View();
        }


        public IActionResult GeoDashboard()
        {
            RedirectToAction(controllerName: "Login", actionName: "index");
            string allText = System.IO.File.ReadAllText("./wwwroot/EGY.json");
            string allText2 = System.IO.File.ReadAllText("./wwwroot/lakes.json");
            ViewData["Governorates"] = JsonConvert.DeserializeObject(allText);
            var locations = from Location in database.Locations
                             select new
                             {
                                 Name = Location.Name.ToString(),
                                 numberofservices = (from Branch in database.Branches
                                                     where Branch.Locationid == Location.LocationId
                                                     select Branch.Numberofservices).Sum(),
                                 Latitude = Location.Latitude,
                                 Longitude = Location.Longitude
                             };
            var data = new List<Dictionary<string, string>>();
            foreach (var item in locations)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                temp.Add("Name", item.Name);
                temp.Add("numberofservices", item.numberofservices.ToString());
                temp.Add("Latitude", item.Latitude.ToString());
                temp.Add("Longitude", item.Longitude.ToString());
                temp.Add("maptooltip", item.Name + " : " + item.numberofservices.ToString() + " " + (item.numberofservices == 1 ? "service" : "services"));
                data.Add(temp);
            }
            ViewData["locations"] = data;
            return View();
        }
    }
}