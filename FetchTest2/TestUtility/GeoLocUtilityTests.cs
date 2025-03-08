using geoloctest;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.IO;
using System.Text.Json;

namespace TestUtility
{
    [TestClass]
    public class GeoLocUtilityTests
    {
        [TestMethod]
        public void VerifyURL_Name()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "Seattle,WA";

            Assert.IsTrue(glai.GetURL(loc) == "http://api.openweathermap.org/geo/1.0/direct?q=" + loc + ",US&limit=1&appid=f897a99d971b5eef57be6fafa0d83239");
        }

        [TestMethod]
        public void VerifyURL_ZipCode()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "98178";

            Assert.IsTrue(glai.GetURL(loc) == "http://api.openweathermap.org/geo/1.0/zip?zip=" + loc + ",US&limit=1&appid=f897a99d971b5eef57be6fafa0d83239");
        }

        [TestMethod]
        public async Task VerifyClientConnection_Successful()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "Seattle,WA";
            String url = glai.GetURL(loc);

            await glai.ConnectClient(url);

            Assert.IsTrue(glai.result.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task VerifyClientConnection_Unsuccessful()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "Not A City,ZZ";
            String url = glai.GetURL(loc);

            await glai.ConnectClient(url);

            Assert.IsTrue(glai.result.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task VerifyResponse_Valid_Name()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "Seattle,WA";
            String url = glai.GetURL(loc);
            await glai.ConnectClient(url);
            Stream responseStream = glai.GetStream();
            var response = await JsonSerializer.DeserializeAsync<List<GenericResponse>>(responseStream);

            Assert.IsNotNull(response);
            Assert.IsTrue(glai.GetResponseString_Name(response, loc) ==
                "Name '" + loc + "': Name = " + response[0].name + "; Latitude = " + response[0].lat + "; Longitude = " + response[0].lon);
        }

        [TestMethod]
        public async Task VerifyResponse_Valid_Zip()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "98178";
            String url = glai.GetURL(loc);
            await glai.ConnectClient(url);
            Stream responseStream = glai.GetStream();
            var response = await JsonSerializer.DeserializeAsync<GenericResponse>(responseStream);

            Assert.IsNotNull(response);
            Assert.IsTrue(glai.GetResponseString_Zip(response, loc) ==
                "Zip Code '" + loc + "': Name = " + response.name + "; Latitude = " + response.lat + "; Longitude = " + response.lon);
        }
        [TestMethod]
        public async Task VerifyResponse_Invalid_Name()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "Not A City,ZZ";
            String url = glai.GetURL(loc);
            await glai.ConnectClient(url);
            Stream responseStream = glai.GetStream();
            var response = await JsonSerializer.DeserializeAsync<List<GenericResponse>>(responseStream);

            Assert.IsNotNull(response);
            Assert.IsTrue(glai.GetResponseString_Name(response, loc) ==
                "Name '" + loc + "': No location found.");
        }

        [TestMethod]
        public async Task VerifyResponse_Invalid_Zip()
        {
            GeoLocApiInterface glai = new GeoLocApiInterface();
            String loc = "981789";
            String url = glai.GetURL(loc);
            await glai.ConnectClient(url);
            Stream responseStream = glai.GetStream();
            var response = await JsonSerializer.DeserializeAsync<GenericResponse>(responseStream);

            Assert.IsNotNull(response);
            Assert.IsTrue((response.lat == 0) && (response.lon == 0) && (response.name == null));
        }
    }
}