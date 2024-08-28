using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace FooBooLooGameAPI.Helpers;

public class DictionaryValueComparer : ValueComparer<Dictionary<int, string>>
{
    public DictionaryValueComparer() : base(
        (d1, d2) => JsonConvert.SerializeObject(d1) == JsonConvert.SerializeObject(d2),
        d => d.GetHashCode(),
        d => JsonConvert.DeserializeObject<Dictionary<int, string>>(JsonConvert.SerializeObject(d)))
    {
    }
}
