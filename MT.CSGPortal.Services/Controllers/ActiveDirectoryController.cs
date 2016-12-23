using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Entities.ViewModels;
using MT.CSGPortal.Entities.DTOs;
using MT.CSGPortal.BL;
using MT.CSGPortal.Services;
using MT.CSGPortal.Utility;

#region MT.CSGPortal.Services.Controllers
namespace MT.CSGPortal.Services.Controllers
{
    #region MindADController : ApiController
    public class ActiveDirectoryController : ApiController, IActiveDirectoryController
    {


        #region Get
        //GET api/mindad/M1009999
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Get(string id)
        {
            ActiveDirectoryManager mindAdManager = new ActiveDirectoryManager();
            JsonResult result = new JsonResult();
            result.Data = mindAdManager.GetUser(id);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        #endregion Get

        #region ValidateUser
        // POST api/mindad/ValidateUser
        /// <summary>
        /// Validate a user against AD using MID (Username) and password
        /// </summary>
        /// <param name="param">JSON object which has a value with key "data", data.mid:MID, data.password:Password</param>
        /// <returns>Valid or invalid</returns>
        [System.Web.Http.HttpPost]
        public JsonResult ValidateUser(Newtonsoft.Json.Linq.JObject param)
        {
            string val = param.GetValue("data").ToString();
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(val);

            ActiveDirectoryManager mindAdManager = new ActiveDirectoryManager();
            JsonResult result = new JsonResult();
            result.Data = mindAdManager.ValidateUser((string)data.mid, (string)data.password, (string)data.domain);
            result.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            return result;
        }
        #endregion ValidateUser


        #region Search
        // POST api/mindad/Search

        /// <summary>
        /// Searches AD based on MID or Name 
        /// </summary>
        /// <param name="param">JSON object which has a value with key "data", data.CN:Name or MID, data.PageNumber: 0 or empty for all result,data.domain:Domain (mindtree) </param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult Search(Newtonsoft.Json.Linq.JObject param)
        {
            string val = param.GetValue("data").ToString();
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(val);

            IActiveDirectoryManager mindAdManager = new ActiveDirectoryManager();
            if (ApplicationSettingsReader.IsADMocked > 0)
            {
                mindAdManager = new ActiveDirectoryManagerMocked();
            }
            JsonResult result = new JsonResult();
            try
            {
                if(data.PageNumber != null && !String.IsNullOrWhiteSpace((string)data.PageNumber) && (byte)data.PageNumber > 0)
                    result.Data = mindAdManager.Search((string)data.CN, (byte)data.PageNumber);
                else
                    result.Data = mindAdManager.Search((string)data.CN);
            }
            catch (Exception)
            {
                result.Data = mindAdManager.Search((string)data.CN);
            }
            result.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            return result;
        }
        #endregion Search

        public Byte[] GetImage(string mid)
        {
            IActiveDirectoryManager activeObj = new ActiveDirectoryManager();
            if (ApplicationSettingsReader.IsADMocked > 0)
            {
                activeObj = new ActiveDirectoryManagerMocked();
            }
            return activeObj.GetImage(mid);
        }

        #region Non-Action Methods

        [System.Web.Http.NonAction]
        public MindFullProfile GetMindProfileById(string mid)
        {
            IActiveDirectoryManager mindAdManager = new ActiveDirectoryManager();
            if (ApplicationSettingsReader.IsADMocked > 0)
            {
                mindAdManager = new ActiveDirectoryManagerMocked();
            }
            return mindAdManager.GetMindFullProfileById(mid);
        }

        /// <summary>
        /// Searches AD for a mind based on MID or Name
        /// </summary>
        /// <param name="searchTerm">Should be Name or MID of the person</param>
        /// <param name="pageNumber">Page Number starting with 1</param>
        /// <returns>DTO with list of MindBasic Profile</returns>
        [System.Web.Http.NonAction]
        public SearchResult<MindBasicProfile> SearchMindsInAD(string searchTerm,byte pageNumber)
        {
            IActiveDirectoryManager mindAdManager = new ActiveDirectoryManager();
            if (ApplicationSettingsReader.IsADMocked > 0)
            {
                mindAdManager = new ActiveDirectoryManagerMocked();
            }
            return mindAdManager.Search(searchTerm, pageNumber);
        }

        #endregion Non-Action Methods 

    }
    #endregion MindADController : ApiController
}
#endregion MT.CSGPortal.Services.Controllers
