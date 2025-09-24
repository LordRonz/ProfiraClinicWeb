using BootstrapBlazor.Components;
using Excubo.Generators.Blazor.ExperimentalDoNotUseYet;
using ProfiraClinic.Models.Core;

namespace ProfiraClinicRME.Model
{
    public class MDiagnosaExt:MasterDiagnosa
    {
        public override int GetHashCode()
        {
            return System.HashCode.Combine(KodeDiagnosa);
        }
        public override bool Equals(object? obj)
        {
            return obj is MDiagnosaExt other && KodeDiagnosa == other.KodeDiagnosa;
        }

        public override string ToString()
        {
            return $"{KodeDiagnosa} - {NamaDiagnosa}";
        }
    }
}
