# Custumer-service-handler
A simple poc for a document customer service application

# requirements
- ASP.NET core framework
- .Net framework v6
- Entity framework core
- identity framework
- Sync fusion library


# sync fusion :-
Syncfusion is a leading provider of enterprise-class development components and libraries, delivering a broad range of UI, reporting, and business intelligence functionality on every major Windows platform. Known for high performance, elegant user interface controls, sophisticated reporting, and an extremely comprehensive array of functionality, since its 2001 founding Syncfusion has established itself as a trusted partner in the creation of mission-critical applications. With quarterly releases and a dedicated support team backing its products, Syncfusion meets the needs of major financial institutions, Fortune 100 companies, and large IT consultancies around the globe. Whether you’re developing WPF, mobile MVC, Windows Forms, ASP.NET, Silverlight, ASP.NET MVC, or Windows Phone applications, Syncfusion is ready to help you deliver true business innovation.


# how does it work
Sync fusion components are can be easily embedded to any workspace as will be illustrated as simple as copying and pasting some code.
All components are binded automatically with the back end using json for data transmission

# major components used in sync fusion (UI is powered by asp.net razor views)
- Grid view
  - the source of the json data is passed in the attribute called datasource in the ejs tag
  - each column is now placed where the property of the json object it represents name is filled in the attriubte field
- Map
  - the map is a number of layers that are placed on top of each other
  - those maps are drawn according to input given throught geojson data that represents the the location shape and coordinates
  - those shapes are then linked to a data source containing each location properties using some binding variable
  - the markers can be then displayed providing another data source that contain 2 properties
    - lattitude
    - longitude
    - maptooltip (optional)
  

# Simple example
UI
```
@using Syncfusion.EJ2.Maps;
@{
    ViewData["Title"] = "Home Page";
    var tooltip = new MapsTooltipSettings
    {
        Visible = true,
        ValuePath = "maptooltip"
    };
    var border = new MapsBorder
    {
        Width = 2,
        Color = "green",
        Opacity = 1
    };
        var margin = new Syncfusion.EJ2.Maps.MapsMargin
    {
        Bottom = 0,
        Top = 0,
        Left = 0,
        Right = 0
    };
}

<div class="container">
    <div class="row">
        <div class="col-6">
            <ejs-grid id="Grid" dataSource="@ViewData["locations"]" allowPaging="true" allowSorting="true" allowFiltering="true" allowGrouping="true" >
               <e-grid-pagesettings pageSize="8d"></e-grid-pagesettings>
                <e-grid-columns>
                   <e-grid-column field="Name" headerText="Location name" onclick="mapRefresh()" width="120"></e-grid-column>
                   <e-grid-column field="numberofservices" headerText="number of services" width="120"></e-grid-column>
                  </e-grid-columns>
            </ejs-grid>
        </div>
        <div class="col-6">
            @using Syncfusion.EJ2;
         <ejs-maps id="maps" theme="Material" background="#006994" margin="margin">
        <e-maps-layers>
            <e-maps-layer shapeData="@ViewData["Governorates"]" shapeDataPath="name" shapePropertyPath='new[] { "ADM2" }' dataSource="@ViewData["locations"]">
                        <e-layersettings-markers>
                            <e-layersettings-marker visible="true" shape="Circle" fill="white" 
                                                    width="2" animationDuration="0" tooltipSettings="tooltip" 
                                                    border="border" dataSource="@ViewData["locations"]">
                            </e-layersettings-marker>
                        </e-layersettings-markers>
            </e-maps-layer>
        </e-maps-layers>
        </ejs-maps>
        </div>
    </div>
</div>
```
Back end
```
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
```
