using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MT.CSGPortal.Portable.Entities;

namespace MT.CSGPortal.Entities.ViewModels
{
    public class AddArchitect
    {
        private Messages messages = new Messages();
        private Mind mind;
        private IDictionary<int, string> contacts = new Dictionary<int, string>();

        public AddArchitect()
        {
            mind = new Mind();
            var enumArry = Enum.GetValues(typeof(Enumerations.ContactType));
            foreach (var item in enumArry)
            {
                int i = (int)item;
                contacts.Add(i, string.Empty);
            }
        }

        public AddArchitect(Mind m)
        {
            this.mind = m;
        }

        public AddArchitect(MindFullProfile profile):this(profile.MindDetails)
        {
            if (profile.MindContacts!=null)
            {
                var enumArry = Enum.GetValues(typeof(Enumerations.ContactType));
                foreach (var item in enumArry)
                {
                    int i = (int)item;
                    var contactItem= profile.MindContacts.FirstOrDefault(c=>c.MindContactType.ContactTypeId==i);
                    if (contactItem!=null)
                    {
                        contacts.Add(i,contactItem.ContactText);
                    }
                    else
                    {
                        contacts.Add(i,string.Empty);
                    }
                }
            }
        }

        public Mind MindDetails { get { return mind;}}

        public List<MindContact> MindContacts
        {
            get
            {
                return contacts.Select(c => new MindContact() { ContactText = c.Value, MindContactType = new ContactType() { ContactTypeId = c.Key } }).ToList<MindContact>(); 
            }
        }

        [Required]
        public string MID { get { return mind.MID; } set { mind.MID=value;} }

        [Required]
        public string Name { get { return mind.Name; } set { mind.Name = value; } }

        [DisplayName("Professional Summary")]
        public string ProfessionalSummary { get { return mind.ProfessionalSummary; } set { mind.ProfessionalSummary = value; } }

        [DisplayName("Experience in months")]
        public int ExperienceInMonths { get { return mind.ExperienceInMonths; } set { mind.ExperienceInMonths = value; } }

        public string Designation { get { return mind.Designation; } set { mind.Designation = value; } }

        [DisplayName("Base location")]
        public string BaseLocation { get { return mind.BaseLocation; } set { mind.BaseLocation = value; } }

        public string Qualification { get { return mind.Qualification; } set { mind.Qualification = value; } }

        [DisplayName("Joined Date")]
        public string JoinedDate { get { return string.Format("{0} {1} {2}", mind.JoinedDateDD == 0 ? string.Empty : mind.JoinedDateDD.ToString(), (mind.JoinedDateMM > 0 && mind.JoinedDateMM < 13) ? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(mind.JoinedDateMM) : string.Empty, mind.JoinedDateYYYY == 0 ? string.Empty : mind.JoinedDateYYYY.ToString()); } }

        [DisplayName("Extension Number")]
        [Phone(ErrorMessage="Enter a valid phone number")]
        public string ExtensionNumber { get { return contacts[(int)Enumerations.ContactType.DeskPhone]; } set { contacts[(int)Enumerations.ContactType.DeskPhone] = value; } }

        [Phone(ErrorMessage = "Enter a valid phone number")]
        [DisplayName("Cell Phone Number")]
        public string CellPhoneNumber { get { return contacts[(int)Enumerations.ContactType.MobilePhone]; } set { contacts[(int)Enumerations.ContactType.MobilePhone] = value; } }

        [Phone(ErrorMessage = "Enter a valid phone number")]
        [DisplayName("Residence Phone Number")]
        public string ResidencePhoneNumber { get { return contacts[(int)Enumerations.ContactType.HomePhone]; } set { contacts[(int)Enumerations.ContactType.HomePhone] = value; } }

        [DisplayName("Work e-mail")]
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string WorkEmail { get { return contacts[(int)Enumerations.ContactType.WorkEmail]; } set { contacts[(int)Enumerations.ContactType.WorkEmail] = value; } }

        [DisplayName("Personal e-mail")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string PersonalEMail { get { return contacts[(int)Enumerations.ContactType.PersonalEmail]; } set { contacts[(int)Enumerations.ContactType.PersonalEmail] = value; } }

        public Messages FormSubmissionMessages { get { return messages;} set { messages=value;} }
    }
}
