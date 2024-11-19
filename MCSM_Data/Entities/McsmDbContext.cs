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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {

            entity.ToTable("Account");


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
                .HasConstraintName("FK__Account__RoleId__3A81B327");
        });

        modelBuilder.Entity<Article>(entity =>
        {

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
        });

        modelBuilder.Entity<Comment>(entity =>
        {

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC07BBC70912");

            entity.ToTable("Dish");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dish_Account");

            entity.HasOne(d => d.DishType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__245D67DE");
        });

        modelBuilder.Entity<DishType>(entity =>
        {

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Retreat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Retrea__47A6A41B");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {

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
        });

        modelBuilder.Entity<Level>(entity =>
        {

            entity.ToTable("Level");


            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.RankName).HasMaxLength(50);
            entity.Property(e => e.RoleType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithOne(p => p.Level)
                .HasForeignKey<Level>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC0769597841");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.MenuName).HasMaxLength(255);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {

            entity.ToTable("MenuDish");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK_MenuDish_Dish");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_MenuDish_Menu");
        });

        modelBuilder.Entity<Notification>(entity =>
        {

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
        });

        modelBuilder.Entity<Payment>(entity =>
        {

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

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<Post>(entity =>
        {

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
        });

        modelBuilder.Entity<PostImage>(entity =>
        {

            entity.ToTable("PostImage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<Profile>(entity =>
        {

            entity.ToTable("Profile");



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
        });

        modelBuilder.Entity<Reaction>(entity =>
        {

            entity.ToTable("Reaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Refund__3214EC07D53B3B44");

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
                .HasConstraintName("FK__Refund__Particip__50FB042B");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refund__RetreatR__5006DFF2");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {

            entity.ToTable("Retreat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cost).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Retreats)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatFile>(entity =>
        {

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
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {

            entity.ToTable("RetreatGroup");


            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.MonkId)

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Room).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__RoomI__370627FE");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {

            entity.ToTable("RetreatGroupMember");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Member).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatGroupMessage>(entity =>
        {

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

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMessages)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatLearningOutcome>(entity =>
        {

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
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {

            entity.ToTable("RetreatLesson");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {

            entity.ToTable("RetreatMonk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {

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

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatRegistrationParticipant>(entity =>
        {

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Participant).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {

            entity.ToTable("RetreatSchedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.RetreatLesson).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatLessonId)

            entity.HasOne(d => d.UsedRoom).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.UsedRoomId)
        });

        modelBuilder.Entity<Role>(entity =>
        {

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {

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
        });

        modelBuilder.Entity<RoomType>(entity =>
        {

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {

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

            entity.ToTable("ToolHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Retreat).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
