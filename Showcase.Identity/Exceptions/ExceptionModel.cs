using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Showcase.Identity.Exceptions;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ExceptionModel
{
    [StringLength(40)]
    public string TraceId { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Message { get; set; }

    public string? Detail { get; set; }
    
    public string? Location { get; set; }
    
}

public class MultiExceptionModel
{
    [StringLength(40)]
    public string TraceId { get; set; } = null!;

    public List<string> Codes { get; set; } = null!;

    public string? Message { get; set; }

    public string? Detail { get; set; }
}