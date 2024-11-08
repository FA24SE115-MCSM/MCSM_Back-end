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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC070D346E09");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053418F3FE17").IsUnique();

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
                .HasConstraintName("FK__Account__RoleId__3864608B");
        });

        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC07D041F330");

            entity.ToTable("Allergy");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Allergies)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Account__4B7734FF");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.Allergies)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Ingredi__4A8310C6");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC07A211E559");

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
                .HasConstraintName("FK__Article__Created__5224328E");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC075DF61928");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostId__3A179ED3");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC073DC72503");

            entity.ToTable("DeviceToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DeviceTokens)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__44CA3770");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC07FEC036FF");

            entity.ToTable("Dish");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__CreatedBy__2057CCD0");

            entity.HasOne(d => d.DishType).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__214BF109");
        });

        modelBuilder.Entity<DishIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishIngr__3214EC070CB485F5");

            entity.ToTable("DishIngredient");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__DishI__2610A626");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.DishIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__Ingre__2704CA5F");
        });

        modelBuilder.Entity<DishType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishType__3214EC07D106A8AE");

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07FA2A05CE");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Create__43A1090D");

            entity.HasOne(d => d.Retreat).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Retrea__44952D46");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC079A3248BE");

            entity.ToTable("Ingredient");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC072CE59AF9");

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
                .HasConstraintName("FK__Lesson__CreatedB__7B264821");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Level__349DA5A647DD4F98");

            entity.ToTable("Level");

            entity.HasIndex(e => e.AccountId, "UQ__Level__349DA5A7AB9920C8").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.RankName).HasMaxLength(50);
            entity.Property(e => e.RoleType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithOne(p => p.Level)
                .HasForeignKey<Level>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Level__AccountId__3D2915A8");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC0758D4983F");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Menu__CreatedBy__29E1370A");
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuDish__3214EC07FC242A79");

            entity.ToTable("MenuDish");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__DishId__2F9A1060");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDishes)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__MenuId__2EA5EC27");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC077AEFD4C0");

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
                .HasConstraintName("FK__Notificat__Accou__4E53A1AA");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC0719618521");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.PaypalOrderId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Retreat__589C25F3");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07E1392975");

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
                .HasConstraintName("FK__Post__CreatedBy__32767D0B");
        });

        modelBuilder.Entity<PostImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostImag__3214EC0726A3A62B");

            entity.ToTable("PostImage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostImages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PostImage__PostI__36470DEF");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Profile__349DA5A62179C092");

            entity.ToTable("Profile");

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A74F5569F2").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Profile__85FB4E385C50602C").IsUnique();

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
                .HasConstraintName("FK__Profile__Account__41EDCAC5");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reaction__3214EC07838548E5");

            entity.ToTable("Reaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__Accoun__3FD07829");

            entity.HasOne(d => d.Post).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reaction__PostId__3EDC53F0");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Retreat__3214EC070577A3A5");

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
                .HasConstraintName("FK__Retreat__Created__56E8E7AB");
        });

        modelBuilder.Entity<RetreatFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatF__3214EC07E600E1D0");

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
                .HasConstraintName("FK__RetreatFi__Retre__5E8A0973");
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07162529F9");

            entity.ToTable("RetreatGroup");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__MonkI__74794A92");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatGroups)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Retre__73852659");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC0757173518");

            entity.ToTable("RetreatGroupMember");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__7755B73D");

            entity.HasOne(d => d.Member).WithMany(p => p.RetreatGroupMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Membe__7849DB76");
        });

        modelBuilder.Entity<RetreatGroupMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07D34D992C");

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
                .HasConstraintName("FK__RetreatGr__Creat__10216507");

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatGroupMessages)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__11158940");
        });

        modelBuilder.Entity<RetreatLearningOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC070529376E");

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
                .HasConstraintName("FK__RetreatLe__Retre__5AB9788F");
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC0795FFFA7F");

            entity.ToTable("RetreatLesson");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Lesso__0697FACD");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatLessons)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__05A3D694");
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatM__3214EC0770916160");

            entity.ToTable("RetreatMonk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__MonkI__625A9A57");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatMonks)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__Retre__634EBE90");
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC079317CF00");

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
                .HasConstraintName("FK__RetreatRe__Creat__662B2B3B");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatRegistrations)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__671F4F74");
        });

        modelBuilder.Entity<RetreatRegistrationParticipant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC07A53449EF");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Participant).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Parti__6BE40491");

            entity.HasOne(d => d.RetreatReg).WithMany(p => p.RetreatRegistrationParticipants)
                .HasForeignKey(d => d.RetreatRegId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Retre__6CD828CA");
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatS__3214EC078A13ACBF");

            entity.ToTable("RetreatSchedule");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Group__0A688BB1");

            entity.HasOne(d => d.Retreat).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Retre__09746778");

            entity.HasOne(d => d.RetreatLesson).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.RetreatLessonId)
                .HasConstraintName("FK__RetreatSc__Retre__0B5CAFEA");

            entity.HasOne(d => d.UsedRoom).WithMany(p => p.RetreatSchedules)
                .HasForeignKey(d => d.UsedRoomId)
                .HasConstraintName("FK__RetreatSc__UsedR__0C50D423");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC075163F8DD");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC0731EC79B6");

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
                .HasConstraintName("FK__Room__RoomTypeId__01D345B0");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC07427CD4D9");

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tool__3214EC07A866CA2B");

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
            entity.HasKey(e => e.Id).HasName("PK__ToolHist__3214EC07AA386EFE");

            entity.ToTable("ToolHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Creat__18B6AB08");

            entity.HasOne(d => d.Retreat).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Retre__19AACF41");

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolHistories)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolI__1A9EF37A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
