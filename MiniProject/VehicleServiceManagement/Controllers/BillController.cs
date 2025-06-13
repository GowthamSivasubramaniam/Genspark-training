using Microsoft.AspNetCore.Mvc;
using VSM.Interfaces;
using VSM.DTO;
using VSM.Misc;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace VSM.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IFileLogger _logger;


        public BillController(IBillService billService,IFileLogger logger)
        {
            _billService = billService;
             _logger = logger;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<BillDisplayDto>> Add([FromBody] BillAddDto dto)
        {
            try
            {
                var result = await _billService.Add(dto);
               _logger.LogData($"Bill Added {result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                 _logger.LogError($"Error Adding bill", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("{billId}")]
        public async Task<ActionResult<BillDisplayDto>> Get(Guid billId)
        {
            var result = await _billService.Get(billId);
            if (result == null)
                return NotFound(new { message = "Bill not found" });
            return Ok(result);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDisplayDto>>> GetAll()
        {try
            {
                var result = await _billService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Downlodaing bill");
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("serviceRecord/{serviceRecordId}")]
        public async Task<ActionResult<IEnumerable<BillDisplayDto>>> GetByServiceRecordId(Guid serviceRecordId)
        { try
            {
                var result = await _billService.GetByServiceRecordId(serviceRecordId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Downlodaing bill");
                  return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{billId}/download-pdf")]
        public async Task<IActionResult> DownloadBillPdf(Guid billId)
        {
            try
            {
                var pdfBytes = await _billService.DownloadBillPdf(billId);
                var fileName = $"Bill_{billId}.pdf";
                 _logger.LogData($"Bill Downloaded {billId}");
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                 _logger.LogError($"Error Downlodaing bill", ex);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}