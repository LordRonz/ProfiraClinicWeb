using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ProfiraClinicWebAPI.Controllers
{
    public class PatientController
    : BaseCrudController<MCustomer>
    {
        public PatientController(AppDbContext ctx) : base(ctx) { }

        protected override DbSet<MCustomer> DbSet
            => _context.MCustomer;

        protected override IQueryable<MCustomer> ApplySearch(
            IQueryable<MCustomer> q,
            string likeParam)
            => q.Where(d => (EF.Functions.Like(d.KodeCustomer, likeParam) ||
                             EF.Functions.Like(d.AlamatDomisili, likeParam) ||
                             EF.Functions.Like(d.NamaCustomer, likeParam) ||
                             EF.Functions.Like(d.NomorHP, likeParam)));

        protected override IOrderedQueryable<MCustomer> ApplyOrder(
            IQueryable<MCustomer> q)
            => q.OrderBy(d => d.KodeCustomer);

        protected override IQueryable<MCustomer> ApplyLastFilter(
        IQueryable<MCustomer> q,
        DateTime lastDate)
        {
            // Suppose your table actually uses  
            //    RecordDate      (instead of CreatedAt)  
            //    LastModified    (instead of UpdatedAt)

            return q.Where(p =>
                p.UPDDT > lastDate
            );
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<MCustomer>> GetItemByCode(string code)
        {
            return await FindOne(c => c.KodeCustomer == code);
        }

        // POST: api/Patient
        // Create a new patient record by executing a stored procedure with error handling.
        [HttpPost("add")]
        public async Task<ActionResult<MCustomer>> CreatePatient([FromBody] MCustomer newPatient)
        {
            if (newPatient == null)
            {
                return BadRequest("Patient data is null.");
            }

            // Prepare SQL parameters. For nullable fields, pass DBNull.Value.
            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", newPatient.KodeLokasi ?? (object)DBNull.Value),
        // The stored procedure generates the customer code, so pass an empty string.
        new SqlParameter("@KodeCustomer", string.Empty),
        new SqlParameter("@NamaCustomer", newPatient.NamaCustomer ?? (object)DBNull.Value),
        new SqlParameter("@JenisKelamin", newPatient.JenisKelamin ?? (object)DBNull.Value),
        new SqlParameter("@TempatLahir", newPatient.TempatLahir ?? (object)DBNull.Value),
        new SqlParameter("@TanggalLahir", newPatient.TanggalLahir ?? (object)DBNull.Value),
        new SqlParameter("@GolonganDarah", newPatient.GolonganDarah ?? (object)DBNull.Value),
        new SqlParameter("@AlamatDomisili", newPatient.AlamatDomisili ?? (object)DBNull.Value),
        new SqlParameter("@RTDomisili", newPatient.RTDomisili ?? (object)DBNull.Value),
        new SqlParameter("@RWDomisili", newPatient.RWDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanDomisili", newPatient.KelurahanDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanDomisili", newPatient.KecamatanDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KotaDomisili", newPatient.KotaDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KodePosDomisili", newPatient.KodePosDomisili ?? (object)DBNull.Value),
        new SqlParameter("@WargaNegara", newPatient.WargaNegara ?? (object)DBNull.Value),
        new SqlParameter("@EMAIL", newPatient.Email ?? (object)DBNull.Value),
        new SqlParameter("@NomorHP", newPatient.NomorHP ?? (object)DBNull.Value),
        new SqlParameter("@StatusMenikah", newPatient.StatusMenikah ?? (object)DBNull.Value),
        new SqlParameter("@AGAMA", newPatient.Agama ?? (object)DBNull.Value),
        new SqlParameter("@Hobbi", newPatient.Hobbi ?? (object)DBNull.Value),
        new SqlParameter("@Pendidikan", newPatient.Pendidikan ?? (object)DBNull.Value),
        new SqlParameter("@PROFESI", newPatient.Profesi ?? (object)DBNull.Value),
        new SqlParameter("@Referensi", newPatient.Referensi ?? (object)DBNull.Value),
        new SqlParameter("@NIK", newPatient.NIK ?? (object)DBNull.Value),
        new SqlParameter("@AlamatNIK", newPatient.AlamatNIK ?? (object)DBNull.Value),
        new SqlParameter("@RTNIK", newPatient.RTNIK ?? (object)DBNull.Value),
        new SqlParameter("@RWNIK", newPatient.RWNIK ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanNIK", newPatient.KelurahanNIK ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanNIK", newPatient.KecamatanNIK ?? (object)DBNull.Value),
        new SqlParameter("@KotaNIK", newPatient.KotaNIK ?? (object)DBNull.Value),
        new SqlParameter("@KodePosNIK", newPatient.KodePosNIK ?? (object)DBNull.Value),
        new SqlParameter("@HubunganKeluarga", newPatient.HubunganKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NamaKeluarga", newPatient.NamaKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@AlamatKeluarga", newPatient.AlamatKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@RTKeluarga", newPatient.RTKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@RWKeluarga", newPatient.RWKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanKeluarga", newPatient.KelurahanKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanKeluarga", newPatient.KecamatanKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KotaKeluarga", newPatient.KotaKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KodePosKeluarga", newPatient.KodePosKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NomorHPKeluarga", newPatient.NomorHPKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NOTE", newPatient.Note ?? (object)DBNull.Value),
        new SqlParameter("@AKTIF", newPatient.AKTIF ?? (object)DBNull.Value),
        new SqlParameter("@USRID", newPatient.USRID ?? (object)DBNull.Value),
        // Output parameter for the new generated KodeCustomer.
        new SqlParameter
        {
            ParameterName = "@NewKodeCustomer",
            SqlDbType = System.Data.SqlDbType.Char,
            Size = 10,
            Direction = System.Data.ParameterDirection.Output
        }
    };

            try
            {
                // Call the stored procedure using ExecuteSqlRawAsync.
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MCustomer_Add " +
                    "@KodeLokasi, @KodeCustomer, @NamaCustomer, @JenisKelamin, @TempatLahir, @TanggalLahir, " +
                    "@GolonganDarah, @AlamatDomisili, @RTDomisili, @RWDomisili, @KelurahanDomisili, @KecamatanDomisili, " +
                    "@KotaDomisili, @KodePosDomisili, @WargaNegara, @EMAIL, @NomorHP, @StatusMenikah, @AGAMA, @Hobbi, " +
                    "@Pendidikan, @PROFESI, @Referensi, @NIK, @AlamatNIK, @RTNIK, @RWNIK, @KelurahanNIK, @KecamatanNIK, " +
                    "@KotaNIK, @KodePosNIK, @HubunganKeluarga, @NamaKeluarga, @AlamatKeluarga, @RTKeluarga, @RWKeluarga, " +
                    "@KelurahanKeluarga, @KecamatanKeluarga, @KotaKeluarga, @KodePosKeluarga, @NomorHPKeluarga, @NOTE, " +
                    "@AKTIF, @USRID, @NewKodeCustomer OUTPUT",
                    sqlParameters);

                // Retrieve the value of the output parameter.
                var newKodeCustomer = sqlParameters.Last().Value?.ToString();
                newPatient.KodeCustomer = newKodeCustomer;

                // Return the newly created patient. Adjust properties as needed.
                return CreatedAtAction(nameof(GetItem), new { id = newPatient.IDCustomer }, newPatient);
            }
            catch (SqlException ex)
            {
                // This will catch SQL exceptions, including the errors raised from RAISERROR in the stored procedure.
                // Return the error message provided by the stored procedure.
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // For other exceptions, return a 500 error.
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // PUT: api/Patient/{kode}
        [HttpPut("edit/{kode}")]
        public async Task<IActionResult> UpdatePatient(string kode, [FromBody] MCustomer updatedPatient)
        {
            if (updatedPatient == null)
            {
                return BadRequest("Patient data is null.");
            }

            // Ensure that the provided route parameter matches the patient record's key.
            if (kode != updatedPatient.KodeCustomer)
            {
                return BadRequest("KodeCustomer mismatch between route and body.");
            }

            var sqlParameters = new[]
            {
        new SqlParameter("@KodeLokasi", updatedPatient.KodeLokasi ?? (object)DBNull.Value),
        new SqlParameter("@KodeCustomer", updatedPatient.KodeCustomer ?? (object)DBNull.Value),
        new SqlParameter("@NamaCustomer", updatedPatient.NamaCustomer ?? (object)DBNull.Value),
        new SqlParameter("@JenisKelamin", updatedPatient.JenisKelamin ?? (object)DBNull.Value),
        new SqlParameter("@TempatLahir", updatedPatient.TempatLahir ?? (object)DBNull.Value),
        new SqlParameter("@TanggalLahir", updatedPatient.TanggalLahir ?? (object)DBNull.Value),
        new SqlParameter("@GolonganDarah", updatedPatient.GolonganDarah ?? (object)DBNull.Value),
        new SqlParameter("@AlamatDomisili", updatedPatient.AlamatDomisili ?? (object)DBNull.Value),
        new SqlParameter("@RTDomisili", updatedPatient.RTDomisili ?? (object)DBNull.Value),
        new SqlParameter("@RWDomisili", updatedPatient.RWDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanDomisili", updatedPatient.KelurahanDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanDomisili", updatedPatient.KecamatanDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KotaDomisili", updatedPatient.KotaDomisili ?? (object)DBNull.Value),
        new SqlParameter("@KodePosDomisili", updatedPatient.KodePosDomisili ?? (object)DBNull.Value),
        new SqlParameter("@WargaNegara", updatedPatient.WargaNegara ?? (object)DBNull.Value),
        new SqlParameter("@EMAIL", updatedPatient.Email ?? (object)DBNull.Value),
        new SqlParameter("@NomorHP", updatedPatient.NomorHP ?? (object)DBNull.Value),
        new SqlParameter("@StatusMenikah", updatedPatient.StatusMenikah ?? (object)DBNull.Value),
        new SqlParameter("@AGAMA", updatedPatient.Agama ?? (object)DBNull.Value),
        new SqlParameter("@Hobbi", updatedPatient.Hobbi ?? (object)DBNull.Value),
        new SqlParameter("@Pendidikan", updatedPatient.Pendidikan ?? (object)DBNull.Value),
        new SqlParameter("@PROFESI", updatedPatient.Profesi ?? (object)DBNull.Value),
        new SqlParameter("@Referensi", updatedPatient.Referensi ?? (object)DBNull.Value),
        new SqlParameter("@NIK", updatedPatient.NIK ?? (object)DBNull.Value),
        new SqlParameter("@AlamatNIK", updatedPatient.AlamatNIK ?? (object)DBNull.Value),
        new SqlParameter("@RTNIK", updatedPatient.RTNIK ?? (object)DBNull.Value),
        new SqlParameter("@RWNIK", updatedPatient.RWNIK ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanNIK", updatedPatient.KelurahanNIK ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanNIK", updatedPatient.KecamatanNIK ?? (object)DBNull.Value),
        new SqlParameter("@KotaNIK", updatedPatient.KotaNIK ?? (object)DBNull.Value),
        new SqlParameter("@KodePosNIK", updatedPatient.KodePosNIK ?? (object)DBNull.Value),
        new SqlParameter("@HubunganKeluarga", updatedPatient.HubunganKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NamaKeluarga", updatedPatient.NamaKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@AlamatKeluarga", updatedPatient.AlamatKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@RTKeluarga", updatedPatient.RTKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@RWKeluarga", updatedPatient.RWKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KelurahanKeluarga", updatedPatient.KelurahanKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KecamatanKeluarga", updatedPatient.KecamatanKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KotaKeluarga", updatedPatient.KotaKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@KodePosKeluarga", updatedPatient.KodePosKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NomorHPKeluarga", updatedPatient.NomorHPKeluarga ?? (object)DBNull.Value),
        new SqlParameter("@NOTE", updatedPatient.Note ?? (object)DBNull.Value),
        new SqlParameter("@AKTIF", updatedPatient.AKTIF ?? (object)DBNull.Value),
        new SqlParameter("@USRID", updatedPatient.USRID ?? (object)DBNull.Value)
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.usp_MCustomer_Edit " +
                    "@KodeLokasi, @KodeCustomer, @NamaCustomer, @JenisKelamin, @TempatLahir, @TanggalLahir, " +
                    "@GolonganDarah, @AlamatDomisili, @RTDomisili, @RWDomisili, @KelurahanDomisili, @KecamatanDomisili, " +
                    "@KotaDomisili, @KodePosDomisili, @WargaNegara, @EMAIL, @NomorHP, @StatusMenikah, @AGAMA, @Hobbi, " +
                    "@Pendidikan, @PROFESI, @Referensi, @NIK, @AlamatNIK, @RTNIK, @RWNIK, @KelurahanNIK, @KecamatanNIK, " +
                    "@KotaNIK, @KodePosNIK, @HubunganKeluarga, @NamaKeluarga, @AlamatKeluarga, @RTKeluarga, @RWKeluarga, " +
                    "@KelurahanKeluarga, @KecamatanKeluarga, @KotaKeluarga, @KodePosKeluarga, @NomorHPKeluarga, @NOTE, " +
                    "@AKTIF, @USRID",
                    sqlParameters);

                return NoContent();
            }
            catch (SqlException ex)
            {
                // This catch block handles SQL exceptions including those raised by RAISERROR in the stored procedure.
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // General exception catch block.
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }


        // DELETE: api/Patient/{id}
        // Delete a patient record.
        [HttpDelete("del/{id}")]
        public async Task<IActionResult> DeletePatient(long id)
        {
            var patient = await _context.MCustomer.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.MCustomer.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to verify if a patient exists.
        private bool PatientExists(long id)
        {
            return _context.MCustomer.Any(e => e.IDCustomer == id);
        }
    }
}
