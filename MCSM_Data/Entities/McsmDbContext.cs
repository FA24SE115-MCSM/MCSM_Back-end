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

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationParticipant> ConversationParticipants { get; set; }

    public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishType> DishTypes { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuDish> MenuDishes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07A4839DC3");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053494A2F31B").IsUnique();

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
                .HasConstraintName("FK__Account__RoleId__276EDEB3");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC0724D0EFC5");

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
                .HasConstraintName("FK__Article__Created__3C69FB99");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07AAA330B6");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Account__503BEA1C");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__51300E55");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostId__4F47C5E3");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC075698DE72");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ConversationParticipant>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.AccountId }).HasName("PK__Conversa__B319022DE51BDE10");

            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.ConversationParticipants)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Accou__3493CFA7");

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationParticipants)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Conve__339FAB6E");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC07DB0E29C6");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__34C8D9D1");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC07838D8612");

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
                .HasConstraintName("FK__Dish__CreatedBy__0B91BA14");

            entity.HasOne(d => d.DishType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__0C85DE4D");
        });

        modelBuilder.Entity<DishType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishType__3214EC07216C1AFA");

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07E4637A21");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Create__2BFE89A6");

            entity.HasOne(d => d.Retreat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Retrea__2CF2ADDF");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC07BA23F395");

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
                .HasConstraintName("FK__Lesson__CreatedB__6383C8BA");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Level__349DA5A6B6959CB8");

            entity.ToTable("Level");

            entity.HasIndex(e => e.AccountId, "UQ__Level__349DA5A774F76AEF").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.RankName).HasMaxLength(50);
            entity.Property(e => e.RoleType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithOne(p => p.Level)
                .HasForeignKey<Level>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Level__AccountId__2D27B809");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC0753793B12");

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
                .HasConstraintName("FK__Menu__CreatedBy__10566F31");
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuDish__3214EC07A7DF0AFD");

            entity.ToTable("MenuDish");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.DishId)
                .HasConstraintName("FK__MenuDish__DishId__151B244E");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__MenuDish__MenuId__14270015");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC079D12F415");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SendAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Conver__3864608B");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Sender__395884C4");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07EA3C541C");

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
                .HasConstraintName("FK__Notificat__Accou__38996AB5");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC077900977F");

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
                .HasConstraintName("FK__Payment__Account__5EBF139D");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Retreat__5FB337D6");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07D34EBDF9");

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
                .HasConstraintName("FK__Post__CreatedBy__17F790F9");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostImag__3214EC07C2B14045");

            entity.ToTable("PostImage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostImage__PostI__1BC821DD");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Profile__349DA5A62EA85980");

            entity.ToTable("Profile");

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A72E2357D8").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Profile__85FB4E38F99C398F").IsUnique();

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
                .HasConstraintName("FK__Profile__Account__31EC6D26");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reaction__3214EC0799D0A23D");

            entity.ToTable("Reaction");

            entity.HasIndex(e => new { e.PostId, e.AccountId }, "UQ__Reaction__D95BBA430DFA92A4").IsUnique();

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
                .HasConstraintName("FK__Reaction__Accoun__2739D489");

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__PostId__2645B050");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Refund__3214EC07B396932D");

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
                .HasConstraintName("FK__Refund__Particip__5AEE82B9");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refund__RetreatR__59FA5E80");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Retreat__3214EC07388C2716");

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
                .HasConstraintName("FK__Retreat__Created__412EB0B6");
        });

        modelBuilder.Entity<RetreatFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatF__3214EC07DB26FA36");

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
                .HasConstraintName("FK__RetreatFi__Retre__48CFD27E");
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07E393B632");

            entity.ToTable("RetreatGroup");

            entity.HasIndex(e => e.RoomId, "UQ__RetreatG__3286393865CBD9BA").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.MonkId)
                .HasConstraintName("FK__RetreatGr__MonkI__6FE99F9F");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Retre__6EF57B66");

            entity.HasOne(d => d.Room).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__RoomI__70DDC3D8");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07EA9FED98");

            entity.ToTable("RetreatGroupMember");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__73BA3083");

            entity.HasOne(d => d.Member).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Membe__74AE54BC");
        });

        modelBuilder.Entity<RetreatLearningOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC0710AAE22B");

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
                .HasConstraintName("FK__RetreatLe__Retre__44FF419A");
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC0752556255");

            entity.ToTable("RetreatLesson");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Lesso__787EE5A0");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__778AC167");
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatM__3214EC079A04174C");

            entity.ToTable("RetreatMonk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__MonkI__4CA06362");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__Retre__4D94879B");
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC070FECFD4E");

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
                .HasConstraintName("FK__RetreatRe__Creat__5070F446");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__5165187F");
        });

        modelBuilder.Entity<RetreatRegistrationParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC074D5BC609");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Participant).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Parti__5629CD9C");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__571DF1D5");
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatS__3214EC07713CAE70");

            entity.ToTable("RetreatSchedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Retre__7B5B524B");

            entity.HasOne(d => d.RetreatLesson).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatLessonId)
                .HasConstraintName("FK__RetreatSc__Retre__7C4F7684");

            entity.HasOne(d => d.UsedRoom).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.UsedRoomId)
                .HasConstraintName("FK__RetreatSc__UsedR__7D439ABD");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC079F6ED1FC");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC07C48DD018");

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
                .HasConstraintName("FK__Room__RoomTypeId__6A30C649");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC07C079BE98");

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tool__3214EC07B0FC79DD");

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
            entity.HasKey(e => e.Id).HasName("PK__ToolHist__3214EC0768EE7B60");

            entity.ToTable("ToolHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Creat__03F0984C");

            entity.HasOne(d => d.Retreat).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Retre__04E4BC85");

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolI__05D8E0BE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
