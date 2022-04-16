﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scholarship
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ScholarshipEntities : DbContext
    {
        public ScholarshipEntities()
            : base("name=ScholarshipEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblException> tblExceptions { get; set; }
        public virtual DbSet<tblStudentDetail> tblStudentDetails { get; set; }
        public virtual DbSet<tblStdPaymentDetail> tblStdPaymentDetails { get; set; }
        public virtual DbSet<tblScholarship> tblScholarships { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserActivation> tblUserActivations { get; set; }
        public virtual DbSet<tblQuestion> tblQuestions { get; set; }
        public virtual DbSet<tblStudentResult> tblStudentResults { get; set; }
    
        public virtual int Validate_User(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Validate_User", usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> ValidateUser(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ValidateUser", usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> Validate_Student(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("Validate_Student", usernameParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> ValidateStudent(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("ValidateStudent", usernameParameter, passwordParameter);
        }
    }
}
