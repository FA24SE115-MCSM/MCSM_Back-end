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

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishIngredient> DishIngredients { get; set; }

    public virtual DbSet<DishType> DishTypes { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07504288FE");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053498B3B0BA").IsUnique();

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

        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC07852C515B");

            entity.ToTable("Allergy");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Allergies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Account__0D7A0286");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Allergies)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Ingredi__0C85DE4D");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC073F8F3B26");

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
                .HasConstraintName("FK__Article__Created__3B75D760");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07D5283FE1");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostId__2BFE89A6");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC071D4D8961");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__33D4B598");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC074AECB070");

            entity.ToTable("Dish");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__CreatedBy__123EB7A3");

            entity.HasOne(d => d.DishType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__1332DBDC");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__Ingredient__14270015");
        });

        modelBuilder.Entity<DishIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishIngr__3214EC0770B794CA");

            entity.ToTable("DishIngredient");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__DishI__17F790F9");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__Ingre__18EBB532");
        });

        modelBuilder.Entity<DishType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishType__3214EC07B72E70CC");

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC0753192592");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Create__3587F3E0");

            entity.HasOne(d => d.Retreat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Retrea__367C1819");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC079E1D54C1");

            entity.ToTable("Ingredient");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC0793C3458F");

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
                .HasConstraintName("FK__Lesson__CreatedB__5DCAEF64");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Level__349DA5A641E0E725");

            entity.ToTable("Level");

            entity.HasIndex(e => e.AccountId, "UQ__Level__349DA5A7AD4B5474").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.RankName).HasMaxLength(50);
            entity.Property(e => e.RoleType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithOne(p => p.Level)
                .HasForeignKey<Level>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Level__AccountId__2C3393D0");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC077950443D");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Menu__CreatedBy__1BC821DD");

            entity.HasOne(d => d.Dish).WithMany(p => p.Menus)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Menu__DishId__1CBC4616");
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuDish__3214EC07481A1563");

            entity.ToTable("MenuDish");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__DishId__2180FB33");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__MenuId__208CD6FA");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC070926476E");

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
                .HasConstraintName("FK__Notificat__Accou__37A5467C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07F7856123");

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
                .HasConstraintName("FK__Payment__Account__3A4CA8FD");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Retreat__3B40CD36");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07D063E4C0");

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
                .HasConstraintName("FK__Post__CreatedBy__245D67DE");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostImag__3214EC074F416B0C");

            entity.ToTable("PostImage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostImage__PostI__282DF8C2");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Profile__349DA5A62AA28953");

            entity.ToTable("Profile");

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A74C1EC435").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Profile__85FB4E38CED7B186").IsUnique();

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
                .HasConstraintName("FK__Profile__Account__30F848ED");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reaction__3214EC074D34577D");

            entity.ToTable("Reaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__Accoun__31B762FC");

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__PostId__30C33EC3");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Retreat__3214EC07881AFD48");

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
                .HasConstraintName("FK__Retreat__Created__403A8C7D");
        });

        modelBuilder.Entity<RetreatFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatF__3214EC07C6C3274A");

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
                .HasConstraintName("FK__RetreatFi__Retre__47DBAE45");
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07B116EF48");

            entity.ToTable("RetreatGroup");

            entity.HasIndex(e => e.RoomId, "UQ__RetreatG__328639384722701D").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__MonkI__6A30C649");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Retre__693CA210");

            entity.HasOne(d => d.Room).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__RoomI__6B24EA82");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07BEF9A21A");

            entity.ToTable("RetreatGroupMember");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__6E01572D");

            entity.HasOne(d => d.Member).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Membe__6EF57B66");
        });

        modelBuilder.Entity<RetreatGroupMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC0730D5B582");

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
                .HasConstraintName("FK__RetreatGr__Creat__7C4F7684");

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMessages)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__7D439ABD");
        });

        modelBuilder.Entity<RetreatLearningOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC0778B9F684");

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
                .HasConstraintName("FK__RetreatLe__Retre__440B1D61");
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC07FAB51A09");

            entity.ToTable("RetreatLesson");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Lesso__72C60C4A");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__71D1E811");
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatM__3214EC079DC17CD6");

            entity.ToTable("RetreatMonk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__MonkI__4BAC3F29");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__Retre__4CA06362");
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC071443B7A5");

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
                .HasConstraintName("FK__RetreatRe__Creat__4F7CD00D");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__5070F446");
        });

        modelBuilder.Entity<RetreatRegistrationParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC072CF03670");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Participant).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Parti__5535A963");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__5629CD9C");
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatS__3214EC0759F19CB9");

            entity.ToTable("RetreatSchedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Group__76969D2E");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Retre__75A278F5");

            entity.HasOne(d => d.RetreatLesson).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatLessonId)
                .HasConstraintName("FK__RetreatSc__Retre__778AC167");

            entity.HasOne(d => d.UsedRoom).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.UsedRoomId)
                .HasConstraintName("FK__RetreatSc__UsedR__787EE5A0");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0723FB5EB9");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC07B1D8DAF4");

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
                .HasConstraintName("FK__Room__RoomTypeId__6477ECF3");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC0719F885B7");

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tool__3214EC07AE350112");

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
            entity.HasKey(e => e.Id).HasName("PK__ToolHist__3214EC0773A4E893");

            entity.ToTable("ToolHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Creat__04E4BC85");

            entity.HasOne(d => d.Retreat).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Retre__05D8E0BE");

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolI__06CD04F7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
