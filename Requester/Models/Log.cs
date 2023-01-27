using System;
using System.Collections.Generic;

namespace Requester.Models;

public partial class Log
{
    public long Id { get; set; }

    public Guid? ProcGuid { get; set; }

    public short? EventType { get; set; }

    public string? Host { get; set; }

    public string? Path { get; set; }

    public string? QueryString { get; set; }

    public string? Headers { get; set; }

    public string? Cookies { get; set; }

    public string? Body { get; set; }

    public string? StatusCode { get; set; }

    public DateTime? Dt { get; set; }

    public TimeSpan? Duration { get; set; }

    public bool? IsParsed { get; set; }

    public string? ParsedType { get; set; }

    public string? Site { get; set; }

    public string? Method { get; set; }

    public string? Protocol { get; set; }
}
