using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProfiraClinicWebAPI.Data.Converters
{

    public class TrimStringConverter : ValueConverter<string, string>
    {
        public TrimStringConverter(ConverterMappingHints? mappingHints = null)
            : base(
                v => v, // On write: leave the value as is (or you could add a .Trim() if desired)
                v => v == null ? null : v.Trim(), // On read: trim the value
                mappingHints)
        {
        }
    }

}
