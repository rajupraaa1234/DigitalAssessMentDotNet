using System;
using Microsoft.EntityFrameworkCore;

namespace Digital_Assesment.Repository.DBCotenxt
{
	public class DBContextClass : DbContext
	{
        public DBContextClass(DbContextOptions<DBContextClass> options) : base(options) { }
        public DbSet<UserClass> Users { get; set; }
        
	}
}

