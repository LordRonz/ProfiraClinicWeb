using ProfiraClinic.Models;
using ProfiraClinic.Models.Api;
using ProfiraClinic.Models.Core;
using ProfiraClinicRME.Helpers;
using ProfiraClinicRME.Infra;
using ProfiraClinicRME.Model;
using ProfiraClinicRME.Utils;
using System;
using System.Text.Json;

namespace ProfiraClinicRME.Services
{
    public interface ITRMPerawatanService
    {

        
        public Task<ServiceResult<Pagination<TRMPerawatanHeader>>> GetListAsync(string KodeCustomer);

        public void CopyHeader(TRMPerawatanHeader source, TRMPerawatanHeader target);

        public void CopyDetail(TRMPerawatanDetail source, TRMPerawatanDetail target);

    }
}
