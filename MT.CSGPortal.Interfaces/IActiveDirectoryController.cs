using System;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using MT.CSGPortal.Entities.DTOs;
using MT.CSGPortal.Entities.ViewModels;
using MT.CSGPortal.Portable.Entities;

namespace MT.CSGPortal.Services
{
    public interface IActiveDirectoryController
    {
        JsonResult Get(string id);
        MindFullProfile GetMindProfileById(string mId);
        JsonResult Search(JObject param);
        SearchResult<MindBasicProfile> SearchMindsInAD(string searchTerm, byte pageNumber);
        JsonResult ValidateUser(JObject param);
    }
}
