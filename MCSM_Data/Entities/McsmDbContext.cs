using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Data.Entities;

public partial class McsmDbContext : DbContext
{
    public McsmDbContext()
    {
    }

    public McsmDbContext(DbContextOptions<McsmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishType> DishTypes { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuDish> MenuDishes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostImage> PostImages { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Retreat> Retreats { get; set; }

    public virtual DbSet<RetreatFile> RetreatFiles { get; set; }

    public virtual DbSet<RetreatGroup> RetreatGroups { get; set; }

    public virtual DbSet<RetreatGroupMember> RetreatGroupMembers { get; set; }

    public virtual DbSet<RetreatGroupMessage> RetreatGroupMessages { get; set; }

    public virtual DbSet<RetreatLearningOutcome> RetreatLearningOutcomes { get; set; }

    public virtual DbSet<RetreatLesson> RetreatLessons { get; set; }

    public virtual DbSet<RetreatMonk> RetreatMonks { get; set; }

    public virtual DbSet<RetreatRegistration> RetreatRegistrations { get; set; }

    public virtual DbSet<RetreatRegistrationParticipant> RetreatRegistrationParticipants { get; set; }

    public virtual DbSet<RetreatSchedule> RetreatSchedules { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Tool> Tools { get; set; }

    public virtual DbSet<ToolHistory> ToolHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC079EFE6174");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105344AEE3FBA").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HashPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.VerifyToken).IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__RoleId__398D8EEE");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC07DB94675A");

            entity.ToTable("Article");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Banner)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Created__4D94879B");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07DEF9A77B");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Account__634EBE90");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostId__625A9A57");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC073F521E02");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__45F365D3");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC079F67C95D");

            entity.ToTable("Dish");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__CreatedBy__22751F6C");

            entity.HasOne(d => d.DishType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__236943A5");
        });

        modelBuilder.Entity<DishType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishType__3214EC071E23FB05");

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC0755F2570C");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Create__40058253");

            entity.HasOne(d => d.Retreat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Retrea__40F9A68C");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC07309A317F");

            entity.ToTable("Lesson");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lesson__CreatedB__74AE54BC");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Level__349DA5A69668F62A");

            entity.ToTable("Level");

            entity.HasIndex(e => e.AccountId, "UQ__Level__349DA5A73C6DA6C0").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.RankName).HasMaxLength(50);
            entity.Property(e => e.RoleType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithOne(p => p.Level)
                .HasForeignKey<Level>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Level__AccountId__3E52440B");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC071FA007F2");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.MenuName).HasMaxLength(250);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Menu__CreatedBy__2739D489");
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuDish__3214EC07D5FA1DB3");

            entity.ToTable("MenuDish");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK__MenuDish__DishId__2BFE89A6");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__MenuDish__MenuId__2B0A656D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC074290A2F0");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Accou__49C3F6B7");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC074BE7C1B2");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Amount).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.PaypalPaymentId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Account__6FE99F9F");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Retreat__70DDC3D8");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07890F80EE");

            entity.ToTable("Post");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__CreatedBy__2EDAF651");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostImag__3214EC07A1892993");

            entity.ToTable("PostImage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostImage__PostI__32AB8735");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Profile__349DA5A604A7BEF2");

            entity.ToTable("Profile");

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A7C8E9B47D").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Profile__85FB4E385B2B1522").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Profile)
                .HasForeignKey<Profile>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Profile__Account__4316F928");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reaction__3214EC0760CAB195");

            entity.ToTable("Reaction");

            entity.HasIndex(e => new { e.PostId, e.AccountId }, "UQ__Reaction__D95BBA43AE92C918").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.ReactionType)
                .HasMaxLength(50)
                .HasDefaultValue("Like");

            entity.HasOne(d => d.Account).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__Accoun__5224328E");

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__PostId__51300E55");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Refund__3214EC0799EB0038");

            entity.ToTable("Refund");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailPaypal).HasMaxLength(255);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(16, 2)");

            entity.HasOne(d => d.Participant).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refund__Particip__6C190EBB");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refund__RetreatR__6B24EA82");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Retreat__3214EC079CDF803E");

            entity.ToTable("Retreat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.DharmaNamePrefix).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Retreats)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retreat__Created__52593CB8");
        });

        modelBuilder.Entity<RetreatFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatF__3214EC07BA9EA169");

            entity.ToTable("RetreatFile");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatFiles)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatFi__Retre__59FA5E80");
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC077A45E8A3");

            entity.ToTable("RetreatGroup");

            entity.HasIndex(e => e.RoomId, "UQ__RetreatG__328639389076344D").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.MonkId)
                .HasConstraintName("FK__RetreatGr__MonkI__01142BA1");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Retre__00200768");

            entity.HasOne(d => d.Room).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__RoomI__02084FDA");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC077642F194");

            entity.ToTable("RetreatGroupMember");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__04E4BC85");

            entity.HasOne(d => d.Member).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Membe__05D8E0BE");
        });

        modelBuilder.Entity<RetreatGroupMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC0789497ED0");

            entity.ToTable("RetreatGroupMessage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RetreatGroupMessages)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Creat__123EB7A3");

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMessages)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__1332DBDC");
        });

        modelBuilder.Entity<RetreatLearningOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC07FFF8C1CE");

            entity.ToTable("RetreatLearningOutcome");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.SubTitle).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLearningOutcomes)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__5629CD9C");
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC07752413EE");

            entity.ToTable("RetreatLesson");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Lesso__09A971A2");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__08B54D69");
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatM__3214EC0707C79AD7");

            entity.ToTable("RetreatMonk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__MonkI__5DCAEF64");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__Retre__5EBF139D");
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC0718207F11");

            entity.ToTable("RetreatRegistration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Creat__619B8048");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__628FA481");
        });

        modelBuilder.Entity<RetreatRegistrationParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC07B78BDD7E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Participant).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Parti__6754599E");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__68487DD7");
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatS__3214EC07D516AC60");

            entity.ToTable("RetreatSchedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Retre__0C85DE4D");

            entity.HasOne(d => d.RetreatLesson).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatLessonId)
                .HasConstraintName("FK__RetreatSc__Retre__0D7A0286");

            entity.HasOne(d => d.UsedRoom).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.UsedRoomId)
                .HasConstraintName("FK__RetreatSc__UsedR__0E6E26BF");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0717868DB7");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC078EF4EFAC");

            entity.ToTable("Room");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__RoomTypeId__7B5B524B");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC072870A4C5");

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tool__3214EC07EA306972");

            entity.ToTable("Tool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<ToolHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ToolHist__3214EC0716FB07BD");

            entity.ToTable("ToolHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Creat__1AD3FDA4");

            entity.HasOne(d => d.Retreat).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Retre__1BC821DD");

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolI__1CBC4616");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
