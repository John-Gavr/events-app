﻿namespace Events.Application.DTOs.Events.Responces;
public class EventResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsFull { get; set; } = false;
    public int MaxParticipants { get; set; }
    public byte[]? Image { get; set; }
}
