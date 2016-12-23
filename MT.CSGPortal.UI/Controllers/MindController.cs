using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MT.CSGPortal.Services;
using MT.CSGPortal.Services.Controllers;
using MT.CSGPortal.Entities.ViewModels;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.DAL;
using Elmah;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;



namespace MT.CSGPortal.UI.Controllers
{
    public class MindController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        public ActionResult Search()
        {
            IMindDataAccess daObject = new MindDataAccess();
            int count = daObject.GetArchitectCount();
            ViewBag.ArchitectCount = count;
            return View();      
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [ValidateInput(true)]
        public JsonResult GetSearchResultsAjax(byte pageNumber,string searchString,byte option)
        {

            JsonResult result = new JsonResult();
            switch (option)
            {
                case 0:
                    using (ActiveDirectoryController controller = new ActiveDirectoryController())
                    {
                        result.Data = controller.SearchMindsInAD(searchString, pageNumber);
                        result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    }
                    break;
                case 1:
                    using (ProfileController mindprofileApi = new ProfileController())
                    {
                        result = mindprofileApi.SearchMinds(searchString, pageNumber);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
     
        /// <summary>
        /// For retrieving the image
        /// </summary>
        /// <param name="mid"></param>
        /// <returns>Image url</returns>
        //public FileContentResult GetImageAjax(string mid)
        public String GetImageAjax(string mid)
        {
            Byte[] result;
            using (ActiveDirectoryController controller = new ActiveDirectoryController())
            {
                MindFullProfile profile = controller.GetMindProfileById(mid);
                if (profile != null)
                {
                    result = controller.GetImage(mid);
                    return result == null ? null : System.Convert.ToBase64String(result);
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult IsAlreadyAMember(string mId)
        {
            JsonResult res = new JsonResult();
            using (ProfileController controller = new ProfileController())
            {
                res.Data = controller.IsMindInPortal(mId);
            }
            return res;
        }

        // GET: /Mind/AddArchitect
        public ActionResult AddArchitect(string id)
        {
            AddArchitect model = new AddArchitect();
            using (ProfileController controller = new ProfileController())
            {                
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        var profile = controller.GetMindFullProfileByID(id);
                        if (profile != null)
                        {
                            model = new AddArchitect(profile);
                        }
                        else
                        {
                            using (ActiveDirectoryController activeDirectoryApiController = new ActiveDirectoryController())
                                profile = activeDirectoryApiController.GetMindProfileById(id);

                            if (profile != null)
                            {
                                model = new AddArchitect(profile);
                                model.FormSubmissionMessages.WarningMessages.Add("You are about to add a Mind to Portal");
                            }
                            else
                            {
                                model.FormSubmissionMessages.ErrorMessages.Add(string.Format("No details found for ID:{0}", id));
                            }                            
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        model.FormSubmissionMessages.ErrorMessages.Add("Error in fetching Details.");
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
            }
            return View(model);
        }

        //POST: /Mind/AddArchitect
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult AddArchitect(AddArchitect model)
        {
            using (ProfileController controller = new ProfileController())
                if (ModelState.IsValid)
                {
                    try
                    {
                        MindFullProfile profile = new MindFullProfile()
                        {
                            MindDetails = model.MindDetails,
                            MindContacts = model.MindContacts
                        };
                        controller.ManageMindProfile(profile);
                        model.FormSubmissionMessages.SuccessMessages.Add("Success...");
                    }
                    catch (Exception ex)
                    {
                        model.FormSubmissionMessages.ErrorMessages.Add("Error in Saving changes");
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
                else
                {
                    model.FormSubmissionMessages.ErrorMessages.Add("Error: Invalid entries or incomplete form");
                    model.FormSubmissionMessages.ErrorMessages.Add("Please try again");
                }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_FormMessages", model.FormSubmissionMessages);
            }
            else
            {
                return View(model);
            }
        }     

        public ActionResult SyncDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public async Task<ActionResult> SyncDBAjax()
        {
            Messages msgs=new Messages();
            try
            {
                 await SyncDBAsync();
                 msgs.SuccessMessages.Add("DB Synchronisation Successfull.");
            }
            catch (Exception ex)
            {
                 msgs.ErrorMessages.Add("Error in Synchronising data base.");
                 Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return PartialView("_FormMessages", msgs);
        }

        private async Task SyncDBAsync()
        {
            Task syncTask = Task.Run(()=>SyncDB());
            await syncTask;
        }

        private void SyncDB()
        {
            ProfileController controller = new ProfileController();
            Messages msgs = new Messages();
            controller.SyncAllProfileDetails();
        }
    }
}
