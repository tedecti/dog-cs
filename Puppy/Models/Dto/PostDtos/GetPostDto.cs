﻿using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.PostDtos;

public class GetPostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Imgs { get; set; }
    public DateTime UploadDate { get; set; }
    public ShortUserDto User { get; set; }
}