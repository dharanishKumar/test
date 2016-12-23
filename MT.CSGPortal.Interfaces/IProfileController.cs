using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MT.CSGPortal.Portable.Entities;

namespace MT.CSGPortal.Services
{
    public interface IProfileController
    {
        List<Mind> GetAllMinds { get; }
        Mind GetMindByID(string id);
        List<MindContact> GetMindContactsByMid(string mid);
        MindFullProfile GetMindFullProfileByID(string id);
        void ManageMindProfile(MindFullProfile profile);
        JsonResult SearchMinds(string searchParam, int currentPageNumber);
        void SyncAllProfileDetails();
        bool IsMindInPortal(string id);
    }
}
