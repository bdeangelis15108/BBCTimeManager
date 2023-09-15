using Nucleus.EquipTimetable;
using Nucleus.Timetable;
using Nucleus.CostType;
using Nucleus.Account;
using Nucleus.UnionPayRate;
using Nucleus.PayperiodHistory;
using Nucleus.StatusUpdate;
using Nucleus.ShiftExpense;
using Nucleus.EmployeeUnion;
using Nucleus.JobUnion;
using Nucleus.Union;
using Nucleus.ShiftResource;
using Nucleus.JobCategory;
using Nucleus.JobPhaseCode;
using Nucleus.Shift;
using Nucleus.PayPeriod;
using Nucleus.Timesheet;
using Nucleus.Status;
using Nucleus.ExpenseType;
using Nucleus.PayType;
using Nucleus.ResourceReservation;
using Nucleus.Job;
using Nucleus.JobClass;
using Nucleus.Address;
using Nucleus.ResourceEquipmentInfo;
using Nucleus.ResourceWorkerInfo;
using Nucleus.WorkerClasee;
using Nucleus.Resource;
using Nucleus.ECCOSTS;
using Nucleus.EQUIPMENTS;
using Nucleus.PRDEDRATES;
using Nucleus.JCJOBS;
using Nucleus.PRCLASSES;
using Nucleus.JCUNIONS;
using Nucleus.JCCAT;
using Nucleus.PREMPLOYEES;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nucleus.Authorization.Roles;
using Nucleus.Authorization.Users;
using Nucleus.Chat;
using Nucleus.Editions;
using Nucleus.Friendships;
using Nucleus.MultiTenancy;
using Nucleus.MultiTenancy.Accounting;
using Nucleus.MultiTenancy.Payments;
using Nucleus.Storage;

namespace Nucleus.EntityFrameworkCore
{
    public class NucleusDbContext : AbpZeroDbContext<Tenant, Role, User, NucleusDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<EquipTimetables> EquipTimetables { get; set; }

        public virtual DbSet<Timetables> Timetables { get; set; }

        public virtual DbSet<CostTypes> CostTypese { get; set; }

        public virtual DbSet<Accounts> Accounts { get; set; }

        public virtual DbSet<UnionPayRates> UnionPayRates { get; set; }

        public virtual DbSet<PayperiodHistories> PayperiodHistories { get; set; }

        public virtual DbSet<StatusUpdates> StatusUpdates { get; set; }

        public virtual DbSet<ShiftExpenses> ShiftExpenses { get; set; }

        public virtual DbSet<EmployeeUnions> EmployeeUnions { get; set; }

        public virtual DbSet<JobUnions> JobUnions { get; set; }

        public virtual DbSet<Unions> Unions { get; set; }

        public virtual DbSet<ShiftResources> ShiftResources { get; set; }

        public virtual DbSet<JobCategories> JobCategories { get; set; }

        public virtual DbSet<JobPhaseCodes> JobPhaseCodes { get; set; }

        public virtual DbSet<Shifts> Shifts { get; set; }

        public virtual DbSet<PayPeriods> PayPeriods { get; set; }

        public virtual DbSet<Timesheets> Timesheets { get; set; }

        public virtual DbSet<Statuses> Statuses { get; set; }

        public virtual DbSet<ExpenseTypes> ExpenseTypeses { get; set; }

        public virtual DbSet<PayTypes> PayTypeses { get; set; }

        public virtual DbSet<ResourceReservations> ResourceReservationses { get; set; }

        public virtual DbSet<Jobs> Jobses { get; set; }

        public virtual DbSet<JobClasses> JobClasseses { get; set; }

        public virtual DbSet<Addresses> Addresseses { get; set; }

        public virtual DbSet<ResourceEquipmentInfos> ResourceEquipmentInfoses { get; set; }

        public virtual DbSet<ResourceWorkerInfos> ResourceWorkerInfoses { get; set; }

        public virtual DbSet<WorkerClasees> WorkerClaseeses { get; set; }

        public virtual DbSet<Resources> Resourceses { get; set; }

        public virtual DbSet<ECCOST> ECCOSTS { get; set; }

        public virtual DbSet<EQUIPMENT> EQUIPMENTS { get; set; }

        public virtual DbSet<PRDEDRATE> PRDEDRATES { get; set; }

        public virtual DbSet<JCJOB> JCJOBs { get; set; }

        public virtual DbSet<PRCLASS> PRCLASSs { get; set; }

        public virtual DbSet<JCUNION> JCUNIONs { get; set; }

        public virtual DbSet<JACCAT> JACCATs { get; set; }

        public virtual DbSet<PREMPLOYEE> PREMPLOYEES { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public NucleusDbContext(DbContextOptions<NucleusDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PRDEDRATE>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<JCJOB>(j =>
                       {
                           j.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PRCLASS>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<JCUNION>(j =>
                       {
                           j.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<JACCAT>(j =>
                       {
                           j.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PREMPLOYEE>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}