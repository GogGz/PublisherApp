using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PublisherData;
using PublisherDomain;
using System.Threading.Channels;

namespace PublisherConsole
{

    internal class Program
    {

        static void Main(string[] args)
        {

            using (PubContext context = new PubContext())
            {
                context.Database.EnsureCreated();
            }


            //GetAuthors();
            //AddAuthor();
            GetAuthors();

            //AddAuthorWithBook();
            //GetAuthorsWithBooks();


            //QueryFilter();

            //AddSomeMoreAuthors();

            //SKipAndTakeAuthors();

            //SortAuthors();

            // RetrieveAndUpdateAuthor();
            //DeleteAuthor();
            //GetAuthors();

            //AddBookToAuthor();
            // LoadBooksWithAuthor();

            ModifyingData();

        }

        public static void ModifyingData()
        {
            var _context = new PubContext();

            var author = _context.Authors.Include(a => a.Books)
                .FirstOrDefault(a => a.Id == 4);
            author.Books[1].PublishDate = "New publish date";
            _context.ChangeTracker.DetectChanges();

            var state = _context.ChangeTracker.DebugView.ShortView;
        }

        public static void Projectoins()
        {
            using var _context = new PubContext();
            var anknown = _context.Authors
                .Select(a => new
                {
                    AuthorId = a.Id,
                    Name = a.LastName + " " + a.LastName
                })
                .ToList();
        }

        public static void LoadBooksWithAuthor()
        {
            using var _context = new PubContext();
            var authors = _context.Authors.Include(a => a.Books).ToList();

            authors.ForEach(a =>
            {
                Console.WriteLine($"{a.LastName} ({a.Books.Count})");
                a.Books.ForEach(b => Console.WriteLine($"             {b.Title}"));
            });
        }  


            public static void AddBookToAuthor()
        {
            using var _context = new PubContext();
            var author = _context.Authors.FirstOrDefault(a => a.LastName == "Lerman");

            if(author != null)
            {
                author.Books.Add(new Book { Title = "Wool", PublishDate = "12.05.2014" });
            }
            _context.SaveChanges();
        }


        public static void DeleteAuthor()
        {
            using var _context = new PubContext();
            var extraJL = _context.Authors.Find(1);

            _context.Authors.Remove(extraJL);
            _context.SaveChanges();
        }


        public static void SaveThatAuthur(Author author)
        {
            using var anotherShortLveContext = new PubContext();

            anotherShortLveContext.Authors.Update(author);
            anotherShortLveContext.SaveChanges();



        }
        public static Author FindThatAuthor(int authorID)
        {
           using var shortLiveContext = new PubContext();
            return shortLiveContext.Authors.Find(authorID);

        }
        public static void SortAuthors()
        {
            using var _context = new PubContext();
            var authorsByLastName = _context.Authors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
            foreach (var auth in authorsByLastName)
            {
                Console.WriteLine(auth.LastName + " " + auth.FirstName);
            }

        }
        public static void RetrieveAndUpdateAuthor()
        {
            using var _context = new PubContext();

            var author = _context.Authors
                .FirstOrDefault(a => a.FirstName == "Julie" && a.LastName == "Lerman");
            if(author != null) 
            {
                author.FirstName = "Julia";
                _context.SaveChanges();
            
            }
                
        }


        public static void SKipAndTakeAuthors()
        {
            using var _context = new PubContext();
            var groupsize = 2;
           

            for (int i = 0; i < 5; i++)
            {
                var authors = _context.Authors.Skip(groupsize * i).Take(groupsize).ToList();
                Console.WriteLine("Group " + i + ": ");

                foreach (var auth in authors)
                {
                    Console.WriteLine(auth.LastName + " " + auth.FirstName);
                }

            }
        }



        public static void AddSomeMoreAuthors()
        {
            using var _context = new PubContext();

            _context.Authors.Add(new Author { FirstName = "Rhoda", LastName = "Lerman" });
            _context.Authors.Add(new Author { FirstName = "Don", LastName = "Jones" });
            _context.Authors.Add(new Author { FirstName = "Jim", LastName = "Christopher" });
            _context.Authors.Add(new Author { FirstName = "Stephen", LastName = "Haunts" });

            _context.SaveChanges();
        }


        public static void QueryFilter()
        {
            using PubContext _context = new();
            //var authors = _context.Authors.Where( a => a.FirstName == "Josie").ToList();

            var authors = _context.Authors.Where(a => EF.Functions.Like(a.LastName, "L%")).ToList();
        }

        public static void AddAuthorWithBook()
        {
            var author = new Author { FirstName = "Julie", LastName = "Lerman" };
            author.Books.Add(new Book { Title = "Programming EF", PublishDate = "01.01.2009" });
            author.Books.Add(new Book { Title = "Programming EF New Edition", PublishDate = "08.1.2010" });

            using var context = new PubContext();
            context.Authors.Add(author);
            context.SaveChanges();
                
        }

        public static void GetAuthorsWithBooks()
        {
            using var context = new PubContext();
            var authors = context.Authors.Include(author => author.Books).ToList();      
            foreach(var author in authors)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName);
                foreach(var book in author.Books)
                {
                    Console.WriteLine(book.Title);
                }
            }
                
         }


        public static void GetAuthors()
        {
            using var context = new PubContext();
            var Authors = context.Authors.ToList();

            foreach(var auth in Authors)
            {
                Console.WriteLine(auth.FirstName + " "+ auth.LastName);
            }
        }
        public static void AddAuthor()
        {
            var author = new Author { FirstName = "Josie", LastName = "Newf" };
            using var context = new PubContext();
            context.Authors.Add(author);
            context.SaveChanges();

        }


    }
}