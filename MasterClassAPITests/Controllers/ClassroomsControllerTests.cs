using Microsoft.VisualStudio.TestTools.UnitTesting;
using MasterClassAPI.Controllers;
using MasterClassAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterClassAPI.Controllers.Tests
{
    [TestClass()]
    public class ClassroomsControllerTests
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //this test method should return true as long as the returned classroom count is the same for the given function
        //this means that if the userid is bad, it will still return true, as both the functions will return 0
        //basically this method tells us nothing, other than the function is running the same in the controller as it is here
        [TestMethod()]
        public void GetClassroomsTest()
        {
            //arrange
            string user_id = "06c554d3-5f80-4602-a522-b4b0709960a2";
            var controller = new ClassroomsController();


            //act 
            var result = controller.GetClassrooms(user_id);


            //assert
            var expected = from c in db.Classrooms
                         where c.User_Id == user_id
                         select c;

            Assert.AreEqual(expected.Count(), result.Count());
        }
    }
}