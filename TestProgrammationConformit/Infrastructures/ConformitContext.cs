using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProgrammationConformit.Entities;

namespace TestProgrammationConformit.Infrastructures
{
    public class ConformitContext : DbContext
    {
        public ConformitContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(new Event()
            {
                Id = Guid.Parse("99c36725-04ec-4336-815c-c6206ca40384"),
                Title = "Grammys 2021",
                Description = "On Sunday, March 14, 2021, the 63rd GRAMMY Awards will be happening.",
                InvolvedPerson = "John Legend"
            },
            new Event()
            {
                Id = Guid.Parse("fc8fde5a-d62e-41d0-a740-372c31cdeb5c"),
                Title = "Oscars 2021",
                Description = "The 93rd Academy Awards ceremony, presented by the Academy of Motion Picture Arts and Sciences (AMPAS), will honor the best films released between January 1, 2020, and February 28, 2021.",
                InvolvedPerson = "Ashley Fox"
            });

            modelBuilder.Entity<Comment>().HasData(new Comment()
            {
                Id = Guid.Parse("004a4887-4011-44b8-940a-cbac81bade25"),
                Description = "Breath taking performance by Adele",
                DateOfCreation = DateTimeOffset.Now,
                EventId = Guid.Parse("99c36725-04ec-4336-815c-c6206ca40384")

            },
            new Comment()
            {
                Id = Guid.Parse("10dfede3-a6dc-4e88-b191-66b9d3ed6fae"),
                Description = "Demi Lovato was just Amazing !!!",
                DateOfCreation = DateTimeOffset.Now,
                EventId = Guid.Parse("99c36725-04ec-4336-815c-c6206ca40384")

            },
            new Comment()
            {
                Id = Guid.Parse("14f25238-fb47-4c3c-9962-7f051141bfd9"),
                Description = " The choice is so hard this time ! everybody deserve to win !",
                DateOfCreation = DateTimeOffset.Now,
                EventId = Guid.Parse("fc8fde5a-d62e-41d0-a740-372c31cdeb5c")

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
