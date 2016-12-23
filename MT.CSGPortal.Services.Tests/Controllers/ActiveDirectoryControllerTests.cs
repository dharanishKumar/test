using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Services.Controllers;
using System.Globalization;

namespace MT.CSGPortal.Services.Controllers.Tests
{
    [TestClass()]
    public class ActiveDirectoryControllerTests
    {
        [TestMethod()]
        public void GetUserTest()
        {
            ActiveDirectoryController controllerObj = new ActiveDirectoryController();
            JsonResult jsonRes = new JsonResult();

            string mid = "M1023876";

            jsonRes = controllerObj.Get(mid);

            Assert.IsNotNull(jsonRes.Data);
        }

        [TestMethod()]
        public void ValidateUserTest()
        {
            ActiveDirectoryController controllerObj = new ActiveDirectoryController();
            JsonResult jsonRes = new JsonResult();

            string mid = "M100xxxx";
            string password = "password123$";
            string domain = "mindtree";

            var jObj = JsonConvert.DeserializeObject<JObject>(string.Format(CultureInfo.InvariantCulture,"{{'data':{{'mid':'{0}', 'password':'{1}', 'domain':'{2}'}}}}", mid, password, domain));

            jsonRes = controllerObj.ValidateUser(jObj);

            Assert.IsNotNull(jsonRes.Data);
        }

        [TestMethod()]
        public void SearchUserByNameTest()
        {
            ActiveDirectoryController controllerObj = new ActiveDirectoryController();
            JsonResult jsonRes = new JsonResult();

            string name = "Arun";
            string domain = "mindtree";

            var jObj = JsonConvert.DeserializeObject<JObject>(string.Format(CultureInfo.InvariantCulture, "{{'data':{{'CN':'{0}', 'domain':'{1}'}}}}", name, domain));

            jsonRes = controllerObj.Search(jObj);

            Assert.IsNotNull(jsonRes.Data);
        }

        [TestMethod()]
        public void GetMindProfileByIDTest()
        {
            ActiveDirectoryController controllerObj = new ActiveDirectoryController();
            string mid = "M1000001";
            MindFullProfile profile = controllerObj.GetMindProfileById(mid);
            Assert.AreEqual(mid, profile.MindDetails.MID);
        }

        [TestMethod()]
        public void SearchADWithPaginationTest()
        {
            ActiveDirectoryController controllerobj = new ActiveDirectoryController();
            string searchTerm ="stasgjsklj";
            byte pageNumber = 0;
            JsonResult result = new JsonResult();
            result.Data = controllerobj.SearchMindsInAD(searchTerm, pageNumber);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod()]
        public void GetImageTest()
        {
            ActiveDirectoryController controllerObj = new ActiveDirectoryController();
            string mid = "M1022679";
            Byte[] result = controllerObj.GetImage(mid);
            Assert.IsNotNull(result);
        }

    }
}
