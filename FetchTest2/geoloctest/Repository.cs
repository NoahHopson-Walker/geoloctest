using System.Text.Json.Serialization;

namespace geoloctest
{
    public record class GenericResponse(
        [property: JsonPropertyName("name")] string name,
        [property: JsonPropertyName("lat")] float lat,
        [property: JsonPropertyName("lon")] float lon
        );

}
