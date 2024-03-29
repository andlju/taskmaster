﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using Petite;
using Taskmaster.Domain;

namespace Taskmaster.DataAccess
{
    public class TaskmasterContextInitializer : DropCreateDatabaseIfModelChanges<TaskmasterContext>
    {
        protected override void Seed(TaskmasterContext context)
        {
            var defaultUser = new User() { Name = "Guest" };
            var testUser = new User() {Name = "Test"};
            context.UserItems.Add(defaultUser);
            context.UserItems.Add(testUser);
            
            context.SaveChanges();
        }
    }

    public class TaskmasterContext : DbContext
    {
        public IDbSet<TaskItem> TaskItems { get; set; }

        public IDbSet<User> UserItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new TaskmasterContextInitializer());

            modelBuilder.Entity<TaskItem>().HasRequired(t => t.CreatedBy).WithMany().HasForeignKey(t=>t.CreatedByUserId).WillCascadeOnDelete(false);
            modelBuilder.Entity<TaskItem>().HasOptional(t => t.AssignedTo).WithMany().HasForeignKey(t => t.AssignedToUserId).WillCascadeOnDelete(false);
            
        }
    }

    public class TaskItemRepository : RepositoryBase<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(IDbSetProvider objectSetProvider) : base(objectSetProvider)
        {
        }

        protected override IQueryable<TaskItem> Query
        {
            get { return base.Query.Include("Comments.CreatedBy").Include("CreatedBy").Include("AssignedTo"); }
        }
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbSetProvider objectSetProvider)
            : base(objectSetProvider)
        {
            
        }
    }
}