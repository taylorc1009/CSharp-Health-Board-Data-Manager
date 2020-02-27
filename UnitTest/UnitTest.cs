using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        // The aim of the test is to check if we can successully store the staff data in a Dictionary within the HealthFacade class, to test this
        // properly it compares the string we expect to be returned the string we get from HealthFacade once members have been added by "addStaff" and
        // retrieved by "getStaffList".
        [TestMethod]
        public void addStaffTest()
        {
            //Creates a test HealthFacade and adds staff members to it
            HealthFacade systemTest = new HealthFacade();
            systemTest.addStaff(1, "Martha", "Rigg", "21 Accia Road", "Edinburgh", "Community Nurse", 55.932221, -3.214164);
            systemTest.addStaff(2, "Mike", "Heathcoat", "21 Accia Road", "Edinburgh", "Care Worker", 55.932221, -3.214164);
            systemTest.addStaff(3, "Jo", "Shaw", "21 Accia Road", "Edinburgh", "Care Worker", 55.932221, -3.214164);
            //Creates the string we expect HealthFacade to return
            string resultExpected = "\n1, Martha Rigg, 21 Accia Road, Edinburgh, Community Nurse, 55.932221, -3.214164\n2, Mike Heathcoat, 21 Accia Road, Edinburgh, Care Worker, 55.932221, -3.214164\n3, Jo Shaw, 21 Accia Road, Edinburgh, Care Worker, 55.932221, -3.214164";
            //Stores the string HealthFacade actually returns
            string resultActual = systemTest.getStaffList();
            //Compares the result to what we expect
            Assert.AreEqual(resultExpected, resultActual);
        }
    }
}