﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace practika.BD
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BDEntities : DbContext
    {
        public BDEntities()
            : base("name=BDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Academics> Academics { get; set; }
        public virtual DbSet<Animals_Mustafina> Animals_Mustafina { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Countries_Mustafina> Countries_Mustafina { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Discipline> Discipline { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Engineer> Engineer { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<Flowers_Mustafina> Flowers_Mustafina { get; set; }
        public virtual DbSet<Head_of_Department> Head_of_Department { get; set; }
        public virtual DbSet<Hight_School_student> Hight_School_student { get; set; }
        public virtual DbSet<Management_Mustafina> Management_Mustafina { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
    }
}
