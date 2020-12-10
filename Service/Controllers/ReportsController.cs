using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Models;
using Microsoft.AspNetCore.Authorization;
using Service.Models.Dtos.Reports;
using System;
using Service.Controllers.Helpers;
using Service.Models.Dtos.Patients;
using Service.Models.Dtos.Treatments;
using Service.Models.Dtos.Conditions;
using Newtonsoft.Json;


namespace Service.Controllers
{

    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        #region Attributes
        private readonly docnetContext _context;
        private readonly AuthHelper _authHelper;
        #endregion


        #region Constructor
        public ReportsController(docnetContext context)
        {
            _context = context;
            _authHelper = new AuthHelper();
        }
        #endregion


        #region GetAll
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<ViewReportDto>> GetReports(
            [FromQuery] ReportParameters reportParameters
        )
        {
            if (!reportParameters.ValidRptRatingRange ||
                !reportParameters.ValidRptCreationDatetimeRange)
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            var query = _context.ReportsRpt
                .Select(report => new ViewReportDto
                {
                    RptId = report.RptId,
                    RptRating = report.RptRating,
                    RptComment = report.RptComment,
                    RptCreationDatetime = report.RptCreationDatetime,
                    RptEditionDatetime = report.RptEditionDatetime,
                    Pat = new ViewPatientDto
                    {
                        PatId = report.Pat.PatId,
                        PtcId = report.Pat.PtcId,
                        GdrId = report.Pat.GdrId,
                        PatFirstName = report.Pat.PatFirstName,
                        PatLastName = report.Pat.PatLastName,
                        PatDob = report.Pat.PatDob,
                        PatHeight = report.Pat.PatHeight,
                        PatWeight = report.Pat.PatWeight,
                        PatIsSmoker = report.Pat.PatIsSmoker,
                        PatIsPregnant = report.Pat.PatIsPregnant,
                    },
                    Tmt = new ViewTreatmentDto
                    {
                        TmtId = report.Tmt.TmtId,
                        TmtName = report.Tmt.TmtName,
                        TmtDescription = report.Tmt.TmtDescription,
                    },
                    Cdn = new ViewConditionDto
                    {
                        CdnId = report.Cdn.CdnId,
                        CdnName = report.Cdn.CdnName,
                        CdnDescription = report.Cdn.CdnDescription,
                    }
                });

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                var currentUsersPracticeId = _context.UsersUsr.Find(currentUserId).PtcId;
                query = query.Where(report => report.Pat.PtcId == currentUsersPracticeId);
            }

            query = ApplyFilters(query, reportParameters);

            PagedList<ViewReportDto> reports = PagedList<ViewReportDto>
                .ToPagedList(query, reportParameters.PageNumber, reportParameters.PageSize);

            var metadata = reports.GetMetaData();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return reports;
        }
        #endregion


        #region GetAllPublic
        [HttpGet("public/")]
        [Authorize]
        public ActionResult<IEnumerable<ViewReportDto>> GetReportsPublic(
            [FromQuery] ReportParameters reportParameters
        )
        {
            if (!reportParameters.ValidRptRatingRange ||
                !reportParameters.ValidRptCreationDatetimeRange)
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            var query = _context.ReportsRpt
                .Select(report => new ViewReportDto
                {
                    RptId = report.RptId,
                    RptRating = report.RptRating,
                    RptComment = report.RptComment,
                    RptCreationDatetime = report.RptCreationDatetime,
                    RptEditionDatetime = report.RptEditionDatetime,
                    Pat = new ViewPatientDto
                    {
                        PatId = report.Pat.PatId,
                        PtcId = report.Pat.PtcId,
                        GdrId = report.Pat.GdrId,
                        PatHeight = report.Pat.PatHeight,
                        PatWeight = report.Pat.PatWeight,
                        PatIsSmoker = report.Pat.PatIsSmoker,
                        PatIsPregnant = report.Pat.PatIsPregnant,
                    },
                    Tmt = new ViewTreatmentDto
                    {
                        TmtId = report.Tmt.TmtId,
                        TmtName = report.Tmt.TmtName,
                        TmtDescription = report.Tmt.TmtDescription,
                    },
                    Cdn = new ViewConditionDto
                    {
                        CdnId = report.Cdn.CdnId,
                        CdnName = report.Cdn.CdnName,
                        CdnDescription = report.Cdn.CdnDescription,
                    }
                });

            query = ApplyFilters(query, reportParameters);

            PagedList<ViewReportDto> reports = PagedList<ViewReportDto>
                .ToPagedList(query, reportParameters.PageNumber, reportParameters.PageSize);

            var metadata = reports.GetMetaData();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return reports;
        }
        #endregion


        #region GetOne
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ViewReportDto> GetReport(int id)
        {
            var report = _context.ReportsRpt
                .Select(report => new ViewReportDto
                {
                    RptId = report.RptId,
                    RptRating = report.RptRating,
                    RptComment = report.RptComment,
                    RptCreationDatetime = report.RptCreationDatetime,
                    RptEditionDatetime = report.RptEditionDatetime,
                    Pat = new ViewPatientDto
                    {
                        PatId = report.Pat.PatId,
                        PtcId = report.Pat.PtcId,
                        GdrId = report.Pat.GdrId,
                        PatFirstName = report.Pat.PatFirstName,
                        PatLastName = report.Pat.PatLastName,
                        PatDob = report.Pat.PatDob,
                        PatHeight = report.Pat.PatHeight,
                        PatWeight = report.Pat.PatWeight,
                        PatIsSmoker = report.Pat.PatIsSmoker,
                        PatIsPregnant = report.Pat.PatIsPregnant,
                    },
                    Tmt = new ViewTreatmentDto
                    {
                        TmtId = report.Tmt.TmtId,
                        TmtName = report.Tmt.TmtName,
                        TmtDescription = report.Tmt.TmtDescription,
                    },
                    Cdn = new ViewConditionDto
                    {
                        CdnId = report.Cdn.CdnId,
                        CdnName = report.Cdn.CdnName,
                        CdnDescription = report.Cdn.CdnDescription,
                    }
                })
                .Where(report => report.RptId == id)
                .FirstOrDefault();

            if (report == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAdmin(User))
            {
                int currentUserId = int.Parse(User.Identity.Name);
                var currentUsersPracticeId = _context.UsersUsr.Find(currentUserId).PtcId;

                if (report.Pat.PtcId != currentUsersPracticeId)
                {
                    return Forbid();
                }
            }

            return report;
        }
        #endregion


        #region PostOne
        [HttpPost]
        [Authorize]
        public ActionResult<ReportsRpt> PostReport(CreateReportDto report)
        {
            ReportsRpt reportToCreate = new ReportsRpt
            {
                PatId = report.PatId,
                CdnId = report.CdnId,
                TmtId = report.TmtId,
                RptRating = report.RptRating,
                RptComment = report.RptComment,
                RptCreationDatetime = DateTime.Now,
                RptEditionDatetime = DateTime.Now,
            };

            try
            {
                _context.ReportsRpt.Add(reportToCreate);
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return reportToCreate;
        }
        #endregion


        #region PutOne
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<ReportsRpt> PutReport(int id, UpdateReportDto report)
        {
            ReportsRpt reportToUpdate = _context.ReportsRpt
                .Include(report => report.Pat.Ptc.UsersUsr)
                .Where(report => report.RptId == id)
                .FirstOrDefault();

            if (reportToUpdate == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorizedList(User, reportToUpdate.Pat.Ptc.UsersUsr))
            {
                return Forbid();
            }

            reportToUpdate.PatId = report.PatId;
            reportToUpdate.CdnId = report.CdnId;
            reportToUpdate.TmtId = report.TmtId;
            reportToUpdate.RptRating = report.RptRating;
            reportToUpdate.RptComment = report.RptComment;
            reportToUpdate.RptEditionDatetime = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }

            catch
            {
                return BadRequest(new { message = "Please make sure that all provided values are correct." });
            }

            return reportToUpdate;
        }
        #endregion


        #region DeleteOne
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<ReportsRpt> DeleteReport(int id)
        {
            var reportToDelete = _context.ReportsRpt
                .Include(report => report.Pat.Ptc.UsersUsr)
                .Where(report => report.RptId == id)
                .FirstOrDefault();

            if (reportToDelete == null)
            {
                return NotFound();
            }

            if (!_authHelper.UserIsAuthorizedList(User, reportToDelete.Pat.Ptc.UsersUsr))
            {
                return Forbid();
            }

            _context.ReportsRpt.Remove(reportToDelete);
            _context.SaveChanges();

            return reportToDelete;
        }
        #endregion


        #region Filters
        private IQueryable<ViewReportDto> ApplyFilters(
            IQueryable<ViewReportDto> query,
            [FromQuery] ReportParameters reportParameters
        )
        {
            return query
                .Where(report => report.RptRating >= reportParameters.RptMinRating &&
                    report.RptRating <= reportParameters.RptMaxRating)
                .Where(report => report.RptCreationDatetime >= reportParameters.RptMinCreationDatetime &&
                    report.RptCreationDatetime <= reportParameters.RptMaxCreationDatetime)
                .Where(report => (report.Pat.GdrId == reportParameters.PatGdrId ||
                    reportParameters.PatGdrId == null))
                .Where(report => (report.Pat.PtcId == reportParameters.PatPtcId ||
                    reportParameters.PatPtcId == null))
                .Where(report => (report.Pat.PatIsSmoker == reportParameters.PatIsSmoker ||
                    reportParameters.PatIsSmoker == null))
                .Where(report => (report.Pat.PatIsPregnant == reportParameters.PatIsPregnant ||
                    reportParameters.PatIsPregnant == null))
                .Where(report => (report.Tmt.TmtId == reportParameters.TmtId ||
                    reportParameters.TmtId == null))
                .Where(report => (report.Cdn.CdnId == reportParameters.CdnId ||
                    reportParameters.CdnId == null));
        }
        #endregion
        
        #region StatsReports
        [HttpGet("stats/")]
        [Authorize]
        public ActionResult<List<StatReportDto>> StatsReports()
        {
            var conditions = _context.ConditionsCdn
                .Select(condition => new ViewConditionDto
                {
                    CdnId = condition.CdnId,
                    CdnName = condition.CdnName,
                    CdnDescription = condition.CdnDescription,
                })
                .ToList();

            List<StatReportDto> results = new List<StatReportDto>();
            
            foreach (var condition in conditions)
            {
                var reportConditions = _context.ReportsRpt
                    .Select(report => new ViewReportDto
                    {
                        RptId = report.RptId,
                        RptRating = report.RptRating,
                        RptComment = report.RptComment,
                        RptCreationDatetime = report.RptCreationDatetime,
                        RptEditionDatetime = report.RptEditionDatetime,
                        Pat = new ViewPatientDto
                        {
                            PatId = report.Pat.PatId,
                            PtcId = report.Pat.PtcId,
                            GdrId = report.Pat.GdrId,
                            PatFirstName = report.Pat.PatFirstName,
                            PatLastName = report.Pat.PatLastName,
                            PatDob = report.Pat.PatDob,
                            PatHeight = report.Pat.PatHeight,
                            PatWeight = report.Pat.PatWeight,
                            PatIsSmoker = report.Pat.PatIsSmoker,
                            PatIsPregnant = report.Pat.PatIsPregnant,
                        },
                        Tmt = new ViewTreatmentDto
                        {
                            TmtId = report.Tmt.TmtId,
                            TmtName = report.Tmt.TmtName,
                            TmtDescription = report.Tmt.TmtDescription,
                        },
                        Cdn = new ViewConditionDto
                        {
                            CdnId = report.Cdn.CdnId,
                            CdnName = report.Cdn.CdnName,
                            CdnDescription = report.Cdn.CdnDescription,
                        }
                    })
                    .Where(report => report.Cdn.CdnId == condition.CdnId);

                results.Add(new StatReportDto
                {
                    RefConditionsInReports = reportConditions.Count(),
                    Cdn = condition
                });
            }
/*
            if (!_authHelper.UserIsAuthorizedList(User, reportToDelete.Pat.Ptc.UsersUsr))
            {
                return Forbid();
            }*/

            return results;
        }
        #endregion
    }
}
