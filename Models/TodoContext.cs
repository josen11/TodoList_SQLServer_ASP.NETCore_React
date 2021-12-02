using Microsoft.EntityFrameworkCore;

namespace APIExample.Models
{
    public class TodoContext:DbContext
    {
        //File that interact with DB - Using Entity Framework
        //Initial Configuration
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<Todo> Todos { get; set; }
    }
}
