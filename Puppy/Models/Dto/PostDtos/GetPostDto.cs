﻿namespace Puppy.Models.Dto;

public class GetPostDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Imgs { get; set; }
    public ShortUserDto User { get; set; }
}