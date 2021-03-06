﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using GetItDone.DAL.Models;

namespace GetItDone.DAL
{
    public class GetItDoneContext : DbContext
    {
        public GetItDoneContext() : base("GetItDone") 
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Session> UserSessions { get; set; }
    }

}
