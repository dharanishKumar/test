using System;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using MT.CSGPortal.Entities.ViewModels;
using MT.CSGPortal.Services.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.CSGPortal.Portable.Entities;

namespace MT.CSGPortal.Services.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTests
    {
        [TestMethod]
        public void SearchMindProfileTest()
        {
            ProfileController controllerObj = new ProfileController();
            JsonResult jsonRes = new JsonResult();
            
            string searchParamStr = "ban";
            int currentPageNumber = 1;
            jsonRes = controllerObj.SearchMinds(searchParamStr, currentPageNumber);

            Assert.IsNotNull(jsonRes.Data);                        
        }

        [TestMethod]
        public void GetMindByIDTest()
        {
            ProfileController controller = new ProfileController();
            Mind mind = new Mind();
            string id = "M1009232";
            mind = controller.GetMindByID(id);
            Assert.AreEqual(id, mind.MID);
        }

        [TestMethod]
        public void GetAllMindsTest()
        {
            ProfileController controllerObj = new ProfileController();
            var result = controllerObj.GetAllMinds;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllMindsBasicProfileTest()
        {
            ProfileController controllerObj = new ProfileController();
            var result = controllerObj.GetAllMinds;
            var profiles = result.ConvertAll<MindBasicProfile>(m => new MindBasicProfile(m));
            Assert.IsNotNull(profiles);
        }

        [TestMethod]
        public void ManageMindProfileTest()
        {
            ProfileController controllerObj = new ProfileController();
            MindFullProfile profile = new MindFullProfile()
            {
                MindDetails = new Mind()
                {
                    MID = "M1026111",
                    Name = "Tharun Kapoor",
                    ProfessionalSummary = "Summary",
                    ExperienceInMonths = 3,
                    Designation = "Developer",
                    BaseLocation = "Bangalore",

                    JoinedDateDD = 1,
                    JoinedDateMM = 11,
                    JoinedDateYYYY = 2013,
                    InactiveDateDD = 0,
                    InactiveDateMM = 0,
                    InactiveDateYYYY = 0
                },
                MindContacts = new List<MindContact>()
                {
                    new MindContact(){
                        MindContactType=new ContactType(){
                            ContactTypeId=4,
                        },
                        ContactText="tharun_mindtree.com"
                    },
                    new MindContact(){
                        MindContactType=new ContactType(){
                            ContactTypeId=5,
                        },
                        ContactText="Tharun_gmail.com"
                    }
                }
            };
            controllerObj.ManageMindProfile(profile);
        }

        [TestMethod]
        public void GetAllMindContactsnyMidTest()
        {
            ProfileController controllerObj = new ProfileController();
            var result = controllerObj.GetMindContactsByMid("M1021650");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UpdateProfilesTest()
        {
            ProfileController controllerObj = new ProfileController();
            controllerObj.SyncAllProfileDetails();
        }
    }
}
