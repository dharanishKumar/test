using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MT.CSGPortal.BL;
using MT.CSGPortal.Entities.ViewModels;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Services;

namespace MT.CSGPortal.Services.Controllers
{
    public class ProfileController : ApiController, IProfileController
    {
        private IProfileManager profileMgr;

        #region Non Action Methods

        /// <summary>
        /// Search in Portal DB using key words (Name,MID,Location,Designation)
        /// </summary>
        /// <param name="searchParameter">keyword</param>
        /// <param name="pageNumber">current page number(minimum 1)</param>
        /// <returns>DTO with basic mind details</returns>
        [System.Web.Http.NonAction]
        public JsonResult SearchMinds(string searchParam, int currentPageNumber)
        {
            profileMgr = new ProfileManager();
            JsonResult resObj = new JsonResult();
            var resultObj = profileMgr.SearchMinds(searchParam, currentPageNumber);
            resObj.Data = resultObj;
            resObj.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return resObj;
        }
   
        [System.Web.Http.NonAction]
        public Mind GetMindByID(string id)
        {
            profileMgr = new ProfileManager();
            Mind mind=profileMgr.GetMindByID(id);
            return mind;
        }

        [System.Web.Http.NonAction]
        public MindFullProfile GetMindFullProfileByID(string id)
        {
            profileMgr = new ProfileManager();
            return profileMgr.GetMindFullProfileByMid(id);
        }

        //[System.Web.Http.NonAction]
        public List<Mind> GetAllMinds
        {
            get
            {
                profileMgr = new ProfileManager();
                return profileMgr.GetAllMinds;
            }
        }

        [System.Web.Http.NonAction]
        public void ManageMindProfile(MindFullProfile profile)
        {
            profileMgr = new ProfileManager();
            profileMgr.ManageMindProfile(profile);
        }

        [System.Web.Http.NonAction]
        public List<MindContact> GetMindContactsByMid(string mid)
        {
            profileMgr = new ProfileManager();
            return profileMgr.GetMindContactsByMid(mid);
        }

        [System.Web.Http.NonAction]
        public void SyncAllProfileDetails()
        {
            profileMgr = new ProfileManager();
            profileMgr.SyncAllProfileDetails();
        }

        [System.Web.Http.NonAction]
        public bool IsMindInPortal(string id)
        {
            profileMgr = new ProfileManager();
            return profileMgr.IsMindInPortal(id);
        }
        #endregion
    }
}