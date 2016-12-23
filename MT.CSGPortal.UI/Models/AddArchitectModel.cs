using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MT.CSGPortal.UI.Models
{
    
    public partial class AddArchitectModel
    {

        [Required]
        public string MID { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Professional Summary")]
        public string ProfessionalSummary { get; set; }
        
        [DisplayName("Experience in months")]
        public int ExperienceInMonths { get; set; }
        
        public string Designation { get; set; }
        
        [DisplayName("Base location")]
        public string BaseLocation { get; set; }
        
        public string Qualification { get; set; }

        [DisplayName("Joined Date")]
        public DateTime JoinedDate { get; set; }

        [DisplayName("Extension Number")]
        public string ExtensionNumber { get; set; }

        [DisplayName("Cell Phone Number")]
        public string CellPhoneNumber { get; set; }

        [DisplayName("Residence Phone Number")]
        public string ResidencePhoneNumber { get; set; }

        [DisplayName("Work e-mail")]
        [Required(ErrorMessage="Error")]
        public string WorkEmail { get; set; }

        [DisplayName("Personal e-mail")]
        public string PersonalEMail { get; set; }

        public IEnumerable<SelectListItem> DesignationList { get { return new SelectList(new List<String>() { "Manager","Developer"}); } }

        public IEnumerable<SelectListItem> QualificationList { get { return new SelectList(new List<String>() { "BE", "BTech" }); } }
      
    }
}