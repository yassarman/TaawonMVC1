using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaawonMVC.Applications.DTO;
using TaawonMVC.Buildings.DTO;
using TaawonMVC.BuildingType.DTO;
using TaawonMVC.BuildingUnitContents.DTO;
using TaawonMVC.BuildingUnits.DTO;
using TaawonMVC.BuildingUses.DTO;
using TaawonMVC.InterventionType.DTO;
using TaawonMVC.Neighborhood.DTO;
using TaawonMVC.PropertyOwnership.DTO;
using TaawonMVC.RestorationType.DTO;
using TaawonMVC.UploadApplicationFiles.DTO;

namespace TaawonMVC.Web.Models.Applications
{
public class ApplicationsViewModel
    {
        public IEnumerable<GetApplicationsOutput> Applications { get; set; }
        public string[] fullNameArray { get; set; }
        public GetBuildingsOutput buildingOutput { get; set; }
        public GetApplicationsOutput applicationsOutput { get; set; }
        public SelectList YesOrNo { get; set; }
        public IEnumerable<GetPropertyOwnershipOutput> PropertyOwnerShips { get; set; }
        public IEnumerable<GetInterventionTypeOutput>  InterventionTypes { get; set; }
        public IEnumerable<GetRestorationTypeOutput>   RestorationTypes { get; set; }
        public  IEnumerable<GetBuildingsOutput> Buildings { get; set; }
        public CreateBuildingsInput Building { get; set; }
        public IEnumerable<GetBuildingTypeOutput> BuildingTypes { get; set; }
        public IEnumerable<getNeighborhoodOutput> Neighborhoods { get; set; }
        public IEnumerable<GetBuildingUsesOutput> BuildingUses { get; set; }
        public IEnumerable<GetBuildingUnitsOutput> BuildingUnits { get; set; }
        public IEnumerable<SelectListItem> BuildingUnitList { get; set; }
        public GetBuildingUnitsOutput BuildingUnit { get; set; }
        public IEnumerable<GetBuildingUnitContentsOutput> BuildingUnitContents { get; set; }
        //------to upload files 
        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }
        public int ApplicationId { get; set; }
        public IEnumerable<GetUploadApplicationFilesOutput> uploadApplicationFilesOutputs { get; set; }
        //-----------


    }
}