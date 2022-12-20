namespace FPTBookStore.Migrations
{
    using FPTBookStore.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Antlr.Runtime.Tree;
    using System.Web.WebSockets;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "FPTBookStore.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                string[] roleList = { "Store Owner", "Admin" };

                CreateSeveralRoles(context, roleList);
                CreateSeveralUsers(context);
                CreateSeveralBooks(context);
            }
        }

        private void CreateSeveralRoles(ApplicationDbContext context, string[] roleList)
        {
            foreach (string role in roleList)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var roleResult = roleManager.Create(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleResult.Errors));
                }
            }
        }

        private void CreateSeveralUsers(ApplicationDbContext context)
        {
            CreateUser(context, "admin@gmail.com", "admin@gmail.com", "System Administrator", "admin123", "Store Owner");
            CreateUser(context, "duyanh@gmail.com", "duyanh@gmail.com", "DuyAnh", "duyanh123", "Store Owner");
            CreateUser(context, "phuc@gmail.com", "phuc@gmail.com", "Phuc", "Phuc_123", "Customer");
            CreateUser(context, "thinh@gmail.com", "thinh@gmail.com", "Thinh", "Thinh_123", "Customer");
        }

        private void CreateUser(ApplicationDbContext context, string email, string userName, string fullName, string password, string role)
        {
            // create new user and set username, full name, email
            var user = new ApplicationUser()
            {
                UserName = userName,
                FullName = fullName,
                Email = email
            };

            // call user manager to hash the password and store the user in the database
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // no need to link role to customer
            if (role == "Customer") return;

            // link role to user
            var addAdminRoleResult = userManager.AddToRole(user.Id, role);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreateSeveralBooks(ApplicationDbContext context)
        {
            // add categories
            context.Categories.Add(new Category()
            {
                Name = "Comic Book",
                Description = "A book intended to be consulted for information on specific matters rather than read from beginning to end.",
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            });

            context.Categories.Add(new Category()
            {
                Name = "Novel",
                Description = "A style of young adult novel primarily targeting high school and middle school students.",
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            });

            context.Categories.Add(new Category()
            {
                Name = "Textbook",
                Description = "A book that teaches a particular subject and that is used especially in schools and colleges.",
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now
            });

            context.SaveChanges();

            // add books
            context.Books.Add(new Book()
            {
                Name = "abc",
                Author = "J. K. Rowling",
                Description = "Harry Potter is a series of seven fantasy novels written by British author J. K. Rowling. The novels chronicle the lives of a young wizard, Harry Potter, and his friends Hermione Granger and Ron Weasley, all of whom are students at Hogwarts School of Witchcraft and Wizardry. The main story arc concerns Harry's struggle against Lord Voldemort, a dark wizard who intends to become immortal, overthrow the wizard governing body known as the Ministry of Magic and subjugate all wizards and Muggles (non-magical people).",
                Category = context.Categories.First(c => c.Name == "Novel"),
                CoverUrl = "JusticeLeague.png",
                Price = 4,
                StockedQuantity = 3200,
                CreatedDateTime = new DateTime(2015, 12, 6),
                UpdatedDateTime = new DateTime(2015, 12, 6)
            });

            context.Books.Add(new Book()
            {
                Name = "Justice League",
                Author = "Gardner Fox",
                Description = "The Justice League is an all-star ensemble cast of established superhero characters from DC Comics' portfolio. Diegetically, these superheroes usually operate independently but occasionally assemble as a team to tackle especially formidable villains.",
                Category = context.Categories.First(c => c.Name == "Comic Book"),
                CoverUrl = "JusticeLeague.png",
                Price = 4.67M,
                StockedQuantity = 2100,
                CreatedDateTime = new DateTime(2019, 6, 26),
                UpdatedDateTime = new DateTime(2019, 6, 26)
            });

            context.Books.Add(new Book()
            {
                Name = "Computing",
                Author = "Alison Page & Karl Held",
                Description = "A complete three-year lower secondary computing course that takes a real-life, project-based approach to teaching young learners the vital computing skills they will need for the digital world. Each unit builds towards the creation of a final project, with topics ranging from to programming simple games to creating web pages.",
                Category = context.Categories.First(c => c.Name == "Textbook"),
                CoverUrl = "computing.png",
                Price = 0.42M,
                StockedQuantity = 2900,
                CreatedDateTime = new DateTime(2022, 2, 16),
                UpdatedDateTime = new DateTime(2022, 2, 16)
            });

            context.Books.Add(new Book()
            {
                Name = "The Avengers",
                Author = "Stan Lee",
                Description = "The Avengers are a team of superheroes appearing in American comic books published by Marvel Comics. The team made its debut in The Avengers #1 (cover-dated Sept. 1963), created by writer-editor Stan Lee and artist/co-plotter Jack Kirby. Labeled Earth's Mightiest Heroes, the original Avengers consisted of Iron Man, Ant-Man, Hulk, Thor and the Wasp. Captain America was discovered trapped in ice in issue #4, and joined the group after they revived him.",
                Category = context.Categories.First(c => c.Name == "Comic Book"),
                CoverUrl = "avenger.png",
                Price = 53.35M,
                StockedQuantity = 800,
                CreatedDateTime = new DateTime(2016, 4, 26),
                UpdatedDateTime = new DateTime(2016, 4, 26)
            });

            context.Books.Add(new Book()
            {
                Name = "English",
                Author = "Raymond Murphy",
                Description = "The world's best-selling grammar series for learners of English. English Grammar in Use Fourth edition is an updated version of the world's best-selling grammar title. It has a fresh, appealing new design and clear layout, with revised and updated examples, but retains all the key features of clarity and accessibility that have made the book popular with millions of learners and teachers around the world.",
                Category = context.Categories.First(c => c.Name == "Textbook"),
                CoverUrl = "English.png",
                Price = 11.99M,
                StockedQuantity = 1200,
                CreatedDateTime = new DateTime(2012, 6, 26),
                UpdatedDateTime = new DateTime(2012, 6, 26)
            });

            context.Books.Add(new Book()
            {
                Name = "The Whisper",
                Author = "Greg Howard",
                Description = "Riley, the main character of the story believes in this fairy tale known as whispers from these magical fairies, in the story we found out that they also grant wishes if you give them tributes. Riley makes wishes in the story to stop being bullied, wishes his crush liked him, and wishes he would stop wetting the bed, and for his mom to come home who disappeared. As riley starts to want to investigate further what is going on and where his mother is he takes matters into his own hands and embarks on a journey to figure this out. In doing this his life changes forever. This book is a good book to implement into an ELAR classroom because one it is a chapter book that it is a good read, but it also is very relatable for the students. Students go through bullying, wanting their crush to like them, and wish weird things like wetting the bed won’t happen to them. This is a fun read and would be very relatable for any 4-8 student.",
                Category = context.Categories.First(c => c.Name == "Novel"),
                CoverUrl = "TheWhisper.jpg",
                Price = 11.99M,
                StockedQuantity = 1200,
                CreatedDateTime = new DateTime(2012, 6, 26),
                UpdatedDateTime = new DateTime(2012, 6, 26)
            });
        }

    }
}
