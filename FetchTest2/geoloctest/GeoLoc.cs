using System.Reflection;
using System.Net.Http.Headers;
using System.Text.Json;
using geoloctest;

internal class GeoLoc
{
    static GeoLocApiInterface geoLoc = new();

    static void Main(string[] args)
    {
        // Process each argument individually
        foreach (var arg in args)
        {
            if ((arg == null) || (arg.Length == 0)) { Console.WriteLine("Location string is null or empty."); continue; }
            geoLoc.ProcessArgumentsAsync(arg.Trim()).GetAwaiter().GetResult();
        }

    }
}

public class GeoLocApiInterface
{
    public HttpClient client = new();
    public HttpResponseMessage result = new();

    public async Task ProcessArgumentsAsync(string loc)
    {
        // Error/edge case checks
        if ((loc == null) || (loc.Length <= 0)) { return; }

        // Call the appropriate API endpoint
        if ((loc[0] >= '0') && (loc[0] <= '9'))
        {
            await PrintGeoLocInfoByZip(loc);
        }
        else
        {
            await PrintGeoLocInfoByName(loc);
        }
    }

    public async Task PrintGeoLocInfoByZip(string loc)
    {
        string url = GetURL(loc);

        try
        {
            // Call the endpoint and check for a successful status
            await ConnectClient(url);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine("Zip '" + loc + "' Returned Unsuccessful Status Code: '" + result.StatusCode + "'");
                return;
            }

            // Deserialize the json packet as a stream.  
            Stream stream = GetStream();
            var response = await JsonSerializer.DeserializeAsync<GenericResponse>(stream);

            if (response != null)
            {
                Console.WriteLine(GetResponseString_Zip(response, loc));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public async Task PrintGeoLocInfoByName(string loc)
    {
        string url = GetURL(loc);

        try
        {
            // Call the endpoint and check for a successful status
            await ConnectClient(url);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine("Name '" + loc + "' Returned Unsuccessful Status Code: '" + result.StatusCode + "'");
                return;
            }

            // Deserialize the json packet as a stream.  
            Stream stream = GetStream();
            var response = await JsonSerializer.DeserializeAsync<List<GenericResponse>>(stream);

            // Print the results
            if (response != null)
            {
                Console.WriteLine(GetResponseString_Name(response, loc));
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public string GetURL(string loc)
    {
        if ((loc[0] >= '0') && (loc[0] <= '9'))
        {
            return "http://api.openweathermap.org/geo/1.0/zip?zip=" + loc + ",US&limit=1&appid=f897a99d971b5eef57be6fafa0d83239";
        }
        else
        {
            return "http://api.openweathermap.org/geo/1.0/direct?q=" + loc + ",US&limit=1&appid=f897a99d971b5eef57be6fafa0d83239";
        }
    }


    public async Task ConnectClient(string url)
    {
        result = await client.GetAsync(url);
    }

    public Stream GetStream()
    {
        return result.Content.ReadAsStream();
    }

    public string GetResponseString_Name(List<GenericResponse> response, string loc)
    {
        if (response.Count > 0)
        {
            return "Name '" + loc + "': Name = " + response[0].name + "; Latitude = " + response[0].lat + "; Longitude = " + response[0].lon;
        }
        else
        {
            return "Name '" + loc + "': No location found.";
        }
    }

    public string GetResponseString_Zip(GenericResponse response, string loc)
    {
        return "Zip Code '" + loc + "': Name = " + response.name + "; Latitude = " + response.lat + "; Longitude = " + response.lon;
    }

}
