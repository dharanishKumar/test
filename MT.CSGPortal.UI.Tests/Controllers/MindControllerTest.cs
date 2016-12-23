using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.CSGPortal.UI;
using MT.CSGPortal.UI.Controllers;


namespace MT.CSGPortal.UI.Tests.Controllers
{
    [TestClass]
    public class MindControllerTest
    {
        [TestMethod()]
        public void GetImageAjaxTest()
        {
            MindController controllerObj = new MindController();
            string mid = "M1022679";
            String result = controllerObj.GetImageAjax(mid);
            Assert.IsNotNull(result);
        }
    }
}
