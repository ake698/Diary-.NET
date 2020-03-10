using System;

public class TodoContext:DbContext
{
	public TodoContext(DbContextOptions o) : base(o)
	{
	}
	public DbSet<Student> Students { get; set; }
}
