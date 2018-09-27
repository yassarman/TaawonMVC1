using Abp.Web.Security.AntiForgery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaawonMVC.Applications;
using TaawonMVC.Applications.DTO;
using TaawonMVC.Buildings;
using TaawonMVC.Buildings.DTO;
using TaawonMVC.BuildingType;
using TaawonMVC.BuildingUnits;
using TaawonMVC.BuildingUnits.DTO;
using TaawonMVC.BuildingUses;
using TaawonMVC.InterventionType;
using TaawonMVC.Neighborhood;
using TaawonMVC.PropertyOwnership;
using TaawonMVC.RestorationType;
using TaawonMVC.UploadApplicationFiles;
using TaawonMVC.UploadApplicationFiles.DTO;
using TaawonMVC.Web.Models.AntiForgery;
using TaawonMVC.Web.Models.Applications;
using System.Configuration;
using System.Data.SqlClient;
using Abp.UI;
using TaawonMVC.BuildingUnitContents;

namespace TaawonMVC.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IApplicationsAppService _applicationsAppService;
        private readonly IBuildingsAppService _buildingsAppService;
        private readonly IBuildingUnitsAppService _buildingUnitsAppService;
        private readonly IPropertyOwnershipAppService _propertyOwnershipAppService;
        private readonly IInterventionTypeAppService _interventionTypeAppService;
        private readonly IRestorationTypeAppService _restorationTypeAppService;
        private readonly INeighborhoodAppService _neighborhoodAppService;
        private readonly IBuildingTypeAppService _buildingTypeAppService;
        private readonly IBuildingUsesAppService _buildingUsesAppService;
        private readonly IUploadApplicationFilesAppService _uploadApplicationFilesAppService;
        private readonly IBuildingUnitContentsAppService _buildingUnitContentsAppService;

        public ApplicationsController(IApplicationsAppService applicationsAppServic,
            IBuildingsAppService buildingsAppService,
            IBuildingUnitsAppService buildingUnitsAppService, 
            IPropertyOwnershipAppService propertyOwnershipAppService, 
            IInterventionTypeAppService interventionTypeAppService, 
            IRestorationTypeAppService restorationTypeAppService,
            INeighborhoodAppService neighborhoodAppService,
            IBuildingTypeAppService buildingTypeAppService,
            IBuildingUsesAppService buildingUsesAppService,
            IUploadApplicationFilesAppService uploadApplicationFilesAppService,
            IBuildingUnitContentsAppService buildingUnitContentsAppService)
        {
            _applicationsAppService = applicationsAppServic;
            _buildingsAppService = buildingsAppService;
            _buildingUnitsAppService = buildingUnitsAppService;
            _propertyOwnershipAppService = propertyOwnershipAppService;
            _interventionTypeAppService = interventionTypeAppService;
            _restorationTypeAppService = restorationTypeAppService;
            _neighborhoodAppService = neighborhoodAppService;
            _buildingTypeAppService = buildingTypeAppService;
            _buildingUsesAppService = buildingUsesAppService;
            _uploadApplicationFilesAppService = uploadApplicationFilesAppService;
            _buildingUnitContentsAppService = buildingUnitContentsAppService;



        }
        // GET: Applications
        public ActionResult Test()
        {

            var buildingUnitContents = _buildingUnitContentsAppService.getAllBuildingUnitContents().ToList();
            var applicationViewModel = new ApplicationsViewModel()
            {
              BuildingUnitContents= buildingUnitContents
            };

            return View("Test", applicationViewModel);
        }
        public ActionResult Index()
        {
            var applications = _applicationsAppService.getAllApplications();
            var applicationsViewModel = new ApplicationsViewModel()
            {
             Applications = applications
            };
            return View("Applications", applicationsViewModel);
        }

        public ActionResult ApplicationForm()
        {
            // get list of building unit content 
            var buildingUnitContents = _buildingUnitContentsAppService.getAllBuildingUnitContents();
            // get the list of building uses
            var buildingUses = _buildingUsesAppService.getAllBuildingUses();
            //get the list of buildingTypes
            var buildingTypes = _buildingTypeAppService.getAllBuildingtype().ToList();
            // get the list of neighborhoods
            var neighborhoods = _neighborhoodAppService.GetAllNeighborhood().ToList();
            // get all of buildings 
            var buildings = _buildingsAppService.getAllBuildings();
            // get all of restoration types 
            var restorationTypes = _restorationTypeAppService.getAllResorationTypes();
            // get all of intervention types 
            var interventionTypes = _interventionTypeAppService.getAllInterventionTypes();
            // get all applications 
            var applications = _applicationsAppService.getAllApplications().ToList();
            // get all property ownerships 
            var propertyOwnerships = _propertyOwnershipAppService.getAllPropertyOwnerships();
            // populate yes no drop down list 
            var yesOrNo = new List<string>
            {
                "True",
                "False"
            };
            var fullNameList = new List<string>();
            foreach(var application in applications)
            {
                if (!String.IsNullOrWhiteSpace(application.fullName))
                {
                    fullNameList.Add(application.fullName);
                }
            }
            var fullNameArray = fullNameList.Distinct().ToArray();
            var applicationsViewModel = new ApplicationsViewModel()
            {
                fullNameArray = fullNameArray,
                buildingOutput = new GetBuildingsOutput(),
                PropertyOwnerShips= propertyOwnerships,
                YesOrNo= new SelectList(yesOrNo),
                InterventionTypes= interventionTypes,
                RestorationTypes = restorationTypes ,
                Applications = applications,
                Buildings = buildings,
                Building = new CreateBuildingsInput(),
                Neighborhoods = neighborhoods,
                BuildingTypes = buildingTypes,
                BuildingUses = buildingUses ,
                BuildingUnitContents = buildingUnitContents




            };

            return View("ApplicationForm", applicationsViewModel);
        }
        public ActionResult PopulateBuildingUnit(int BuildingUnitId)
        {
            var getBuildingUnitInput = new GetBuildingUnitsInput()
            {
                Id = BuildingUnitId
            };
            var buildingUnit = _buildingUnitsAppService.GetBuildingUnitsById(getBuildingUnitInput);

            var applicationBuildingUnitViewModel = new ApplicationsViewModel()
            {
                BuildingUnit= buildingUnit
            };

            return Json(applicationBuildingUnitViewModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopulateApplicationForm(int buildingId)
        { 
            
            
            //instantiate object GetBuidlingsInput to get the building entity with given id 
            var getBuildingInput = new GetBuidlingsInput()
            {
              Id= buildingId
            };
            // retrieve the building with givin id 
            var building = _buildingsAppService.getBuildingsById(getBuildingInput);
            //var buildingUnits = _buildingUnitsAppService.getAllBuildingUnits().ToList();
            //var BuildingUnits = from BU in buildingUnits where BU.BuildingId == buildingId select BU;
            // declare viewmodel object to pass data to view 
            var applicationViewModel = new ApplicationsViewModel()
            {
                 buildingOutput = building
                
            };

             return Json(applicationViewModel, JsonRequestBehavior.AllowGet);
          //  return View("ApplicationForm", applicationViewModel);
        }
        public ActionResult DropDownList (int buildingId)
        {
            var buildingUnitsApp = _buildingUnitsAppService.getAllBuildingUnits();
            var buildingUnits = (from BU in buildingUnitsApp where BU.BuildingId == buildingId select BU);

            var DropDownListViewModel = new ApplicationsViewModel()
            {
              BuildingUnits = buildingUnits
            };

            return PartialView("_DropDownListView", DropDownListViewModel);

        }
        public ActionResult PopulateDropDownListBuildingUnits(int buildingId)
        {
            var buildingUnitsApp = _buildingUnitsAppService.getAllBuildingUnits();
            var buildingUnits = (from BU in buildingUnitsApp where BU.BuildingId == buildingId select BU).ToList();
            List<SelectListItem> buildingUnitsList = new List<SelectListItem>();
            
              foreach (var buildingUnit in buildingUnits)
                 {
                     buildingUnitsList.Add(new SelectListItem { Text =buildingUnit.ResidentName, Value = buildingUnit.Id.ToString() });
                 }
        
            //var ListOfBuildingUnits = new List<string>();
            //foreach (var BuildingUnit in BuildingUnits)
            //{
            //    ListOfBuildingUnits.Add(BuildingUnit.ResidentName);
            //}
            //var applicationViewModelPL = new ApplicationsViewModel()
            //{
            //  // BuildingUnitList = ListOfBuildingUnits
            //   //  BuildingUnits = buildingUnits
            //      BuildingUnitList= buildingUnitsList,
            //    BuildingUnit = new GetBuildingUnitsOutput()
            //};
                return Json(buildingUnitsList, JsonRequestBehavior.AllowGet);
           // return View("ApplicationForm", applicationViewModelPL);
            
        }

        public ActionResult CreateApplication(CreateApplicationsInput model )
        {
            var application = new CreateApplicationsInput();
             application.phoneNumber1 = model.phoneNumber1;
             application.fullName = model.fullName;
             application.phoneNumber2 = model.phoneNumber2;
             application.isThereFundingOrPreviousRestoration = model.isThereFundingOrPreviousRestoration;
             application.isThereInterestedRepairingEntity = model.isThereInterestedRepairingEntity;
             application.housingSince = model.housingSince;
             application.previousRestorationSource = model.previousRestorationSource;
             application.interestedRepairingEntityName = model.interestedRepairingEntityName;
             application.PropertyOwnerShipId =Convert.ToInt32(Request["PropertyOwnerShip"]) ;
             application.otherOwnershipType = model.otherOwnershipType;
             application.interventionTypeId= Convert.ToInt32(Request["interventionTypeName"]);
             application.otherRestorationType = model.otherRestorationType;
             application.propertyStatusDescription = model.propertyStatusDescription;
             application.requiredRestoration = model.requiredRestoration;
             application.buildingId = Convert.ToInt32(Request["BuildingId2"]);
            //  application.buildingUnitId = Convert.ToInt32(Request["buildingUnitId"]);
             application.buildingUnitId = Convert.ToInt32(Request["dropDownBuildingUnitApp"]);
            // ==== get of restoration types which it is multi select drop down list ======
            var restorationTypes = Request["example-getting-started"];
             string[] restorationTypesSplited = restorationTypes.Split(',');
             byte[] restorationTypesArray = new byte[restorationTypesSplited.Length];
             for (var i = 0; i < restorationTypesArray.Length; i++)
             {
                restorationTypesArray[i] =Convert.ToByte(restorationTypesSplited[i]) ;
             }

              application.restorationTypeIds = restorationTypesArray;
            // ====== end of RestorationTypes

            _applicationsAppService.Create(application);
            // ==== get list of applications ==============
            var applications = _applicationsAppService.getAllApplications();
            var applicationsViewModel = new ApplicationsViewModel()
            {
                Applications = applications
            };

            return View("Applications", applicationsViewModel);

        }

        public ActionResult EditApplication(int appId)

        {
            var yesOrNo = new List<string>
            {
                "True",
                "False"
            };

            var getApplicationInput = new GetApplicationsInput()
            {
              Id=appId
            };
            
            
            // get application according to givin application Id  
            var application = _applicationsAppService.GetApplicationById(getApplicationInput);
            // get the list of buildings 
            var buildings = _buildingsAppService.getAllBuildings();
            // get the list of building units
            var buildingUnits = _buildingUnitsAppService.getAllBuildingUnits();
            var buildingUnitsByBuildingId = from BU in buildingUnits where BU.BuildingId == application.buildingId select BU;
            // get building information by buildingId in application
            var getBuildingInput = new GetBuidlingsInput()
            {
                Id = application.buildingId
            };
            // get the building information by BuildingId
            var building = _buildingsAppService.getBuildingsById(getBuildingInput);
            // get the information of spicific building unit 
            var getBuildingUnitInput = new GetBuildingUnitsInput()
            {
                Id = application.buildingUnitId
            };
            var buildingUnit = _buildingUnitsAppService.GetBuildingUnitsById(getBuildingUnitInput);
            // get list of propertyOwnerships 
            var propertyOwnerships = _propertyOwnershipAppService.getAllPropertyOwnerships();
            // get list of interventionTypes
            var interventionTypes = _interventionTypeAppService.getAllInterventionTypes();
            // get list of restorationTypes
            var restorationType = _restorationTypeAppService.getAllResorationTypes();


            var ApplicationViewModel = new ApplicationsViewModel()
            {
                applicationsOutput = application,
                Buildings = buildings,
                BuildingUnits = buildingUnitsByBuildingId,
                buildingOutput = building,
                YesOrNo = new SelectList(yesOrNo),
                PropertyOwnerShips=propertyOwnerships,
                BuildingUnit= buildingUnit,
                InterventionTypes= interventionTypes,
                RestorationTypes= restorationType

            };



            return View("_EditApplicationsModal", ApplicationViewModel);
        }

        public ActionResult UpdateApplication (UpdateApplicationsInput model)
        {
           
            var updateApplication = new UpdateApplicationsInput();
            updateApplication.buildingId =Convert.ToInt32(Request["buildingnumber"]);
            updateApplication.buildingUnitId= Convert.ToInt32(Request["dropDownBuildingUnitApp"]);
            //==== get building and unit related to application for update ======
            //var buildingInput = new GetBuidlingsInput()
            //{
            //    Id = updateApplication.buildingId
            //};
            //var buildingUnitInput = new GetBuildingUnitsInput()
            //{
            //    Id = updateApplication.buildingUnitId
            //};
           
            // using task and async method 
            // var buildingApp = _buildingsAppService.getBuildingsByIdAsync(buildingInput).Result;
            // var buildingApp = _buildingsAppService.getBuildingsById(buildingInput);
            // var buildingUnitApp = _buildingUnitsAppService.GetBuildingUnitsById(buildingUnitInput);
              // buildingUnitApp.BuildingId = updateApplication.buildingId;
               //  buildingUnitApp.ResidenceStatus= Request["residentstatus"];


            // copy object getBuildingUnitInput to updateBuildingUnitInput
            var updateBuildingUnitInput = new UpdateBuildingUnitsInput();
            //{
            //    BuildingId = buildingUnitApp.BuildingId,
            //    ResidentName=buildingUnitApp.ResidentName,
            //    ResidenceStatus=buildingUnitApp.ResidenceStatus,
            //    NumberOfFamilyMembers=buildingUnitApp.NumberOfFamilyMembers,
            //    Floor=buildingUnitApp.Floor,
            //    UnitContentsIds=buildingUnitApp.UnitContentsIds
            //};
            //============================================
            // copy object from getBuildingOutput to updateBuildingInput
            var updateBuildingInput = new UpdateBuidlingsInput();
            //{
            //   Id = buildingApp.Id,
            //   buildingID = buildingApp.buildingID,
            //   numOfBuildingUnits = buildingApp.numOfBuildingUnits,
            //   numOfFloors = buildingApp.numOfFloors,
            //   streetName = buildingApp.streetName,
            //   buildingNo =buildingApp.buildingNo,
            //   neighborhoodID= buildingApp.neighborhoodID,
            //   buildingTypeID=buildingApp.buildingTypeID,
            //   GISMAP=buildingApp.GISMAP,
            //   houshProperty=buildingApp.houshProperty,
            //   houshName= buildingApp.houshName,
            //   X=buildingApp.X,
            //   Y=buildingApp.Y, 
            //   buildingName=buildingApp.buildingName,
            //   isInHoush =buildingApp.isInHoush,
            //   buildingUsesID=buildingApp.buildingUsesID

            //};




            //======================================================
            // connect to database using ADO.net to retreive building unit details ========
            string construnit = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            using (SqlConnection con = new SqlConnection(construnit))
            {
                string query = "SELECT Id," +
                    " BuildingId," +
                    "ResidentName," +
                    "ResidenceStatus," +
                    "NumberOfFamilyMembers," +
                    "Floor," +
                    "UnitContentsIds" +
                    " FROM BuildingUnits Where Id=" + updateApplication.buildingUnitId;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    //cmd.Parameters.Add(new SqlParameter("@Id", paramValue));
                    SqlDataReader reader = cmd.ExecuteReader();
                    // reader.GetString(1)
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            updateBuildingUnitInput.Id = reader.GetInt32(0);
                            updateBuildingUnitInput.BuildingId = reader.GetInt32(1);
                            updateBuildingUnitInput.ResidentName = reader.GetString(2);
                            updateBuildingUnitInput.ResidenceStatus = reader.GetString(3);
                            updateBuildingUnitInput.NumberOfFamilyMembers = reader.GetInt32(4);
                            updateBuildingUnitInput.Floor = reader.GetString(5);
                            updateBuildingUnitInput.UnitContentsIds =(byte[])reader.GetValue(6);
                           



                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("No rows found.");
                    }
                    reader.Close();
                    con.Close();
                }
            }
            // read any changes made on building unit through the form .
            updateBuildingUnitInput.ResidenceStatus = Request["residentstatus"];
            //======================================================
            // connect to database using ADO.net to retreive building details ========
            string constr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
              using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT buildingID," +
                    " Id," +
                    "numOfBuildingUnits," +
                    "numOfFloors," +
                    "streetName," +
                    "buildingNo," +
                    "neighborhoodID," +
                    "buildingTypeID," +
                    "GISMAP," +
                    "houshProperty," +
                    "houshName," +
                    "X," +
                    "Y," +
                    "buildingName," +
                    "isInHoush," +
                    "buildingUsesID" +
                    " FROM Buildings Where Id=" + updateApplication.buildingId;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    //cmd.Parameters.Add(new SqlParameter("@Id", paramValue));
                    SqlDataReader reader = cmd.ExecuteReader();
                    // reader.GetString(1)
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            updateBuildingInput.buildingID =  reader.GetInt32(0);
                            updateBuildingInput.Id= reader.GetInt32(1);
                            updateBuildingInput.numOfBuildingUnits= reader.GetInt32(2);
                            updateBuildingInput.numOfFloors= reader.GetInt32(3);
                            updateBuildingInput.streetName = reader.GetString(4);
                            updateBuildingInput.buildingNo = reader.GetInt32(5);
                            updateBuildingInput.neighborhoodID= reader.GetInt32(6);
                            updateBuildingInput.buildingTypeID= reader.GetInt32(7);
                            updateBuildingInput.GISMAP = reader.GetString(8);
                            updateBuildingInput.houshProperty = reader.GetByte(9);
                            updateBuildingInput.houshName = reader.GetString(10);
                            updateBuildingInput.X = reader.GetDouble(11);
                            updateBuildingInput.Y = reader.GetDouble(12);
                            updateBuildingInput.buildingName = reader.GetString(13);
                            updateBuildingInput.isInHoush = reader.GetBoolean(14);
                            updateBuildingInput.buildingUsesID = reader.GetInt32(15);



                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("No rows found.");
                    }
                    reader.Close();
                    con.Close();
                }
            }
              // read any changes made on building through the form 
            updateBuildingInput.streetName = Request["buildingaddress"];
            updateBuildingInput.isInHoush = Convert.ToBoolean(Request["buildingOutput.isInHoush"]);
            updateBuildingInput.houshName = Request["HoushName"];
            //===================================================================================
            //=====================================================
            updateApplication.Id = Convert.ToInt32(Request["applicationId"]);  
            updateApplication.fullName = model.fullName;
            updateApplication.phoneNumber1 = model.phoneNumber1;
            updateApplication.phoneNumber2 = model.phoneNumber2;
            updateApplication.isThereFundingOrPreviousRestoration = model.isThereFundingOrPreviousRestoration;
            updateApplication.isThereInterestedRepairingEntity = model.isThereInterestedRepairingEntity;
            updateApplication.housingSince = model.housingSince;
            updateApplication.previousRestorationSource = model.previousRestorationSource;
            updateApplication.interestedRepairingEntityName = model.interestedRepairingEntityName;
            updateApplication.PropertyOwnerShipId = Convert.ToInt32(Request["PropertyOwnerShip"]);
            updateApplication.otherOwnershipType = model.otherOwnershipType;
            updateApplication.interventionTypeId = Convert.ToInt32(Request["interventionTypeName"]);
            updateApplication.otherRestorationType = model.otherRestorationType;
            updateApplication.propertyStatusDescription = model.propertyStatusDescription;
            updateApplication.requiredRestoration = model.requiredRestoration;
            updateApplication.buildingId = Convert.ToInt32(Request["buildingnumber"]);
            updateApplication.buildingUnitId = Convert.ToInt32(Request["dropDownBuildingUnitApp"]);
            // ==== get of restoration types which it is multi select drop down list ======
            var restorationTypes = Request["example-getting-started"];
            string[] restorationTypesSplited = restorationTypes.Split(',');
            byte[] restorationTypesArray = new byte[restorationTypesSplited.Length];
            for (var i = 0; i < restorationTypesArray.Length; i++)
            {
                restorationTypesArray[i] = Convert.ToByte(restorationTypesSplited[i]);
            }

            updateApplication.restorationTypeIds = restorationTypesArray;

            // ====== end of RestorationTypes


            // _buildingsAppService.updateAsync(updateBuildingInput);
             _buildingsAppService.update(updateBuildingInput);
            _applicationsAppService.Update(updateApplication);
            _buildingUnitsAppService.Update(updateBuildingUnitInput);

            // ==== get list of applications ==============
            var applicationsUpdate = _applicationsAppService.getAllApplications();
            var applicationsViewModel = new ApplicationsViewModel()
            {
                Applications = applicationsUpdate
            };

            return View("Applications", applicationsViewModel);
        }

        public ActionResult UploadFileModal(int applicationId)
        {
            var UploadFileViewModel = new ApplicationsViewModel()
            {
                ApplicationId = applicationId
            };
            return View("_UploadFileView", UploadFileViewModel);
        }

        // upload files using ajax call to prevent form refresh or reload 
        [HttpPost]
        public ActionResult SaveUploadedFiles(int applicationId, int filenumber, int noOfFiles)

        {

            var UploadedFile = new CreateUploadApplicationFilesInput();
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {

                    var file = System.Web.HttpContext.Current.Request.Files["HelpSectionImages" + filenumber];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(file);
                    //var fileName = Path.GetFileName(filebase.FileName);
                    //var path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                    //filebase.SaveAs(path);
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        UploadedFile.FileData = binaryReader.ReadBytes(file.ContentLength);
                    }
                    UploadedFile.applicationId = applicationId;
                    UploadedFile.Type = file.ContentType;
                    UploadedFile.FileName = file.FileName;
                    UploadedFile.NoOfFiles = noOfFiles;


                    _uploadApplicationFilesAppService.Create(UploadedFile);


                    return Json("File Saved Successfully.");
                }
                else { return Json("No File Saved."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }


        }
        // end of uploadfile using ajax ===========================================

        // show uploaded files ===============================

        [HttpPost]
        [OutputCache(Duration = 0)]
        [DisableAbpAntiForgeryTokenValidation]
        [ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult ShowUploadedFiles(int applicationId)

        {
            var uploadedFiles = _uploadApplicationFilesAppService.GetAllUploadedApplicationFiles();
            var applicationUploadedFiles = from UF in uploadedFiles where UF.applicationId == applicationId orderby UF.Id descending select UF;
            var uploadedFilesViewModel = new ApplicationsViewModel()
            {
                uploadApplicationFilesOutputs = applicationUploadedFiles
            };

            return PartialView("_UploadedFilesView", uploadedFilesViewModel);
        }

        // end of uploaded files 

        // download file from database 
        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            var getUploadedFileInput = new GetUploadApplicationFilesInput()
            {
                Id = id
            };

            var uploadedFileById = _uploadApplicationFilesAppService.GetUploadApplicationFileById(getUploadedFileInput);


            return File(uploadedFileById.FileData, uploadedFileById.Type, uploadedFileById.FileName);

        }


        // end of download file 
    }
}