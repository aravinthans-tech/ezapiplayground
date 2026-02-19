using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace QRCodeAPI.Models;

public class KycVerificationRequest
{
    [Required]
    public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
    
    [Required]
    public string ExpectedAddress { get; set; } = string.Empty;
    
    public string ModelChoice { get; set; } = "Mistral"; // "Mistral" or "OpenAI"
    
    public double ConsistencyThreshold { get; set; } = 0.82;
    
    public IFormFile? LicenseImage { get; set; }
    
    public IFormFile? SelfieImage { get; set; }
}

