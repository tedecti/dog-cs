﻿namespace Curs.Models
{
	public class Pet
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string PassportNumber { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
