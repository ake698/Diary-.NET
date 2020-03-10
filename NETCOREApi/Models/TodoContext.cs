using Microsoft.EntityFrameworkCore;
using NETCOREApi.Models;
using System;

public class TodoContext:DbContext
{
	public TodoContext(DbContextOptions o) : base(o)
	{
	}
	public DbSet<User> User { get; set; }

	public DbSet<Diary> Diary { get; set; }

	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	var diary = modelBuilder.Entity<Diary>();
	//	diary.HasOne(r => r.User).WithMany(p => p.Diaries).HasForeignKey(r => r.UserId);
	//}
}
