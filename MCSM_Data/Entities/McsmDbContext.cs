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

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuDish> MenuDishes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Retreat> Retreats { get; set; }

    public virtual DbSet<RetreatGroup> RetreatGroups { get; set; }

    public virtual DbSet<RetreatGroupMember> RetreatGroupMembers { get; set; }

    public virtual DbSet<RetreatGroupMessage> RetreatGroupMessages { get; set; }

    public virtual DbSet<RetreatLesson> RetreatLessons { get; set; }

    public virtual DbSet<RetreatMonk> RetreatMonks { get; set; }

    public virtual DbSet<RetreatRegistration> RetreatRegistrations { get; set; }

    public virtual DbSet<RetreatSchedule> RetreatSchedules { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Tool> Tools { get; set; }

    public virtual DbSet<ToolHistory> ToolHistories { get; set; }

    public virtual DbSet<ToolOperation> ToolOperations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC072F44D93C");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105340F551771").IsUnique();

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
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.VerifyToken).IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__RoleId__398D8EEE");
        });

        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allergy__3214EC07954DA594");

            entity.ToTable("Allergy");

            entity.HasIndex(e => e.AccountId, "UQ__Allergy__349DA5A789BB8821").IsUnique();

            entity.HasIndex(e => e.IngredientId, "UQ__Allergy__BEAEB25BDBCD4CEE").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Allergy)
                .HasForeignKey<Allergy>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Account__4CA06362");

            entity.HasOne(d => d.Ingredient).WithOne(p => p.Allergy)
                .HasForeignKey<Allergy>(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allergy__Ingredi__4BAC3F29");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC07099C9C79");

            entity.ToTable("Article");

            entity.HasIndex(e => e.CreatedBy, "UQ__Article__655D054F57EDFA2E").IsUnique();

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

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Article)
                .HasForeignKey<Article>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Article__Created__5535A963");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07922C957E");

            entity.ToTable("Comment");

            entity.HasIndex(e => e.PostId, "UQ__Comment__AA12601936316001").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithOne(p => p.Comment)
                .HasForeignKey<Comment>(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__PostId__44CA3770");
        });

        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DeviceTo__3214EC07BF1BC864");

            entity.ToTable("DeviceToken");

            entity.HasIndex(e => e.AccountId, "UQ__DeviceTo__349DA5A7281147CB").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.DeviceToken)
                .HasForeignKey<DeviceToken>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeviceTok__Accou__440B1D61");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dish__3214EC071FF223DD");

            entity.ToTable("Dish");

            entity.HasIndex(e => e.DishTypeId, "UQ__Dish__074A02C341A83766").IsUnique();

            entity.HasIndex(e => e.CreatedBy, "UQ__Dish__655D054F63176AF1").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Dish)
                .HasForeignKey<Dish>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__CreatedBy__2739D489");

            entity.HasOne(d => d.DishType).WithOne(p => p.Dish)
                .HasForeignKey<Dish>(d => d.DishTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dish__DishTypeId__282DF8C2");
        });

        modelBuilder.Entity<DishIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishIngr__3214EC0705F58974");

            entity.ToTable("DishIngredient");

            entity.HasIndex(e => e.DishId, "UQ__DishIngr__18834F516CD90488").IsUnique();

            entity.HasIndex(e => e.IngredientId, "UQ__DishIngr__BEAEB25BBE776246").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithOne(p => p.DishIngredient)
                .HasForeignKey<DishIngredient>(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__DishI__2EDAF651");

            entity.HasOne(d => d.Ingredient).WithOne(p => p.DishIngredient)
                .HasForeignKey<DishIngredient>(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DishIngre__Ingre__2FCF1A8A");
        });

        modelBuilder.Entity<DishType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DishType__3214EC072C10024A");

            entity.ToTable("DishType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07B6B2CCDD");

            entity.ToTable("Ingredient");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC07C6EA47B6");

            entity.ToTable("Lesson");

            entity.HasIndex(e => e.CreatedBy, "UQ__Lesson__655D054FA679FC4C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Lesson)
                .HasForeignKey<Lesson>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lesson__CreatedB__75A278F5");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Like__3214EC07576439AE");

            entity.ToTable("Like");

            entity.HasIndex(e => e.AccountId, "UQ__Like__349DA5A77DC3B441").IsUnique();

            entity.HasIndex(e => e.PostId, "UQ__Like__AA126019117DB8F1").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Like)
                .HasForeignKey<Like>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Like__AccountId__4C6B5938");

            entity.HasOne(d => d.Post).WithOne(p => p.Like)
                .HasForeignKey<Like>(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Like__PostId__4B7734FF");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC072EEEFAAC");

            entity.ToTable("Menu");

            entity.HasIndex(e => e.CreatedBy, "UQ__Menu__655D054F28AFE405").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Menu)
                .HasForeignKey<Menu>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Menu__CreatedBy__339FAB6E");
        });

        modelBuilder.Entity<MenuDish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuDish__3214EC07CBB8EA14");

            entity.ToTable("MenuDish");

            entity.HasIndex(e => e.DishId, "UQ__MenuDish__18834F5187F3A5BC").IsUnique();

            entity.HasIndex(e => e.MenuId, "UQ__MenuDish__C99ED231BA1B40F8").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Dish).WithOne(p => p.MenuDish)
                .HasForeignKey<MenuDish>(d => d.DishId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__DishId__3B40CD36");

            entity.HasOne(d => d.Menu).WithOne(p => p.MenuDish)
                .HasForeignKey<MenuDish>(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuDish__MenuId__3A4CA8FD");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC079D2F7B15");

            entity.ToTable("Notification");

            entity.HasIndex(e => e.AccountId, "UQ__Notifica__349DA5A77856AF5E").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithOne(p => p.Notification)
                .HasForeignKey<Notification>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Accou__5070F446");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC07F9EA69AD");

            entity.ToTable("Post");

            entity.HasIndex(e => e.CreatedBy, "UQ__Post__655D054F21CB100A").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Post)
                .HasForeignKey<Post>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__CreatedBy__3F115E1A");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Profile__349DA5A6C380B91E");

            entity.ToTable("Profile");

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A797DDF5C4").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Profile__85FB4E38D56C8090").IsUnique();

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
                .HasConstraintName("FK__Profile__Account__403A8C7D");
        });

        modelBuilder.Entity<Retreat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Retreat__3214EC07CB514C67");

            entity.ToTable("Retreat");

            entity.HasIndex(e => e.CreatedBy, "UQ__Retreat__655D054F8039B208").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.Retreat)
                .HasForeignKey<Retreat>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retreat__Created__5AEE82B9");
        });

        modelBuilder.Entity<RetreatGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07F0CB35FE");

            entity.ToTable("RetreatGroup");

            entity.HasIndex(e => e.RetreatId, "UQ__RetreatG__80FFD45EE33FAA05").IsUnique();

            entity.HasIndex(e => e.MonkId, "UQ__RetreatG__C60A89A47757B326").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Monk).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__MonkI__6C190EBB");

            entity.HasOne(d => d.Retreat).WithOne(p => p.RetreatGroup)
                .HasForeignKey<RetreatGroup>(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Retre__6B24EA82");
        });

        modelBuilder.Entity<RetreatGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC07F1076A75");

            entity.ToTable("RetreatGroupMember");

            entity.HasIndex(e => e.MemberId, "UQ__RetreatG__0CF04B19B4249277").IsUnique();

            entity.HasIndex(e => e.GroupId, "UQ__RetreatG__149AF36B875E56B5").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithOne(p => p.RetreatGroupMember)
                .HasForeignKey<RetreatGroupMember>(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__70DDC3D8");

            entity.HasOne(d => d.Member).WithOne(p => p.RetreatGroupMember)
                .HasForeignKey<RetreatGroupMember>(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Membe__71D1E811");
        });

        modelBuilder.Entity<RetreatGroupMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatG__3214EC0729E5AE2E");

            entity.ToTable("RetreatGroupMessage");

            entity.HasIndex(e => e.GroupId, "UQ__RetreatG__149AF36B4645DE84").IsUnique();

            entity.HasIndex(e => e.CreatedBy, "UQ__RetreatG__655D054FEFD2107E").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.RetreatGroupMessage)
                .HasForeignKey<RetreatGroupMessage>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Creat__0F624AF8");

            entity.HasOne(d => d.Group).WithOne(p => p.RetreatGroupMessage)
                .HasForeignKey<RetreatGroupMessage>(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatGr__Group__10566F31");
        });

        modelBuilder.Entity<RetreatLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatL__3214EC070B3DD35B");

            entity.ToTable("RetreatLesson");

            entity.HasIndex(e => e.RetreatId, "UQ__RetreatL__80FFD45EF57D8014").IsUnique();

            entity.HasIndex(e => e.LessonId, "UQ__RetreatL__B084ACD156D5CC9C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Lesson).WithOne(p => p.RetreatLesson)
                .HasForeignKey<RetreatLesson>(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Lesso__01142BA1");

            entity.HasOne(d => d.Retreat).WithOne(p => p.RetreatLesson)
                .HasForeignKey<RetreatLesson>(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatLe__Retre__00200768");
        });

        modelBuilder.Entity<RetreatMonk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatM__3214EC070A8614B3");

            entity.ToTable("RetreatMonk");

            entity.HasIndex(e => e.RetreatId, "UQ__RetreatM__80FFD45EEBD5E774").IsUnique();

            entity.HasIndex(e => e.MonkId, "UQ__RetreatM__C60A89A420867FD3").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithOne(p => p.RetreatMonk)
                .HasForeignKey<RetreatMonk>(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__MonkI__5FB337D6");

            entity.HasOne(d => d.Retreat).WithOne(p => p.RetreatMonk)
                .HasForeignKey<RetreatMonk>(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatMo__Retre__60A75C0F");
        });

        modelBuilder.Entity<RetreatRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatR__3214EC076CCDA14A");

            entity.ToTable("RetreatRegistration");

            entity.HasIndex(e => e.PractitionerId, "UQ__RetreatR__91A595EC0514CCA6").IsUnique();

            entity.HasIndex(e => e.MonkId, "UQ__RetreatR__C60A89A4B963B739").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Monk).WithOne(p => p.RetreatRegistrationMonk)
                .HasForeignKey<RetreatRegistration>(d => d.MonkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__MonkI__656C112C");

            entity.HasOne(d => d.Practitioner).WithOne(p => p.RetreatRegistrationPractitioner)
                .HasForeignKey<RetreatRegistration>(d => d.PractitionerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatRe__Pract__66603565");
        });

        modelBuilder.Entity<RetreatSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RetreatS__3214EC07D15DB9B2");

            entity.ToTable("RetreatSchedule");

            entity.HasIndex(e => e.RetreatLessonId, "UQ__RetreatS__07AC403EEA4420CE").IsUnique();

            entity.HasIndex(e => e.GroupId, "UQ__RetreatS__149AF36B35725A3E").IsUnique();

            entity.HasIndex(e => e.UsedRoomId, "UQ__RetreatS__7E1730FC06FE0694").IsUnique();

            entity.HasIndex(e => e.RetreatId, "UQ__RetreatS__80FFD45E3379125A").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithOne(p => p.RetreatSchedule)
                .HasForeignKey<RetreatSchedule>(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Group__08B54D69");

            entity.HasOne(d => d.Retreat).WithOne(p => p.RetreatSchedule)
                .HasForeignKey<RetreatSchedule>(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RetreatSc__Retre__07C12930");

            entity.HasOne(d => d.RetreatLesson).WithOne(p => p.RetreatSchedule)
                .HasForeignKey<RetreatSchedule>(d => d.RetreatLessonId)
                .HasConstraintName("FK__RetreatSc__Retre__09A971A2");

            entity.HasOne(d => d.UsedRoom).WithOne(p => p.RetreatSchedule)
                .HasForeignKey<RetreatSchedule>(d => d.UsedRoomId)
                .HasConstraintName("FK__RetreatSc__UsedR__0A9D95DB");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07D9457924");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC079E0E496C");

            entity.ToTable("Room");

            entity.HasIndex(e => e.RoomTypeId, "UQ__Room__BCC89630A9BC5C47").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.RoomType).WithOne(p => p.Room)
                .HasForeignKey<Room>(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__RoomTypeId__7B5B524B");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomType__3214EC07573D8609");

            entity.ToTable("RoomType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tool__3214EC0707CCC459");

            entity.ToTable("Tool");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ToolHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ToolHist__3214EC078E7D4147");

            entity.ToTable("ToolHistory");

            entity.HasIndex(e => e.ToolOpId, "UQ__ToolHist__0C2A973EF420FBB3").IsUnique();

            entity.HasIndex(e => e.CreatedBy, "UQ__ToolHist__655D054F38AF0C87").IsUnique();

            entity.HasIndex(e => e.RetreatId, "UQ__ToolHist__80FFD45E6419F5EA").IsUnique();

            entity.HasIndex(e => e.ToolId, "UQ__ToolHist__CC0CEB90495DFA36").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(dateadd(hour,(7),getutcdate()))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithOne(p => p.ToolHistory)
                .HasForeignKey<ToolHistory>(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Creat__1CBC4616");

            entity.HasOne(d => d.Retreat).WithOne(p => p.ToolHistory)
                .HasForeignKey<ToolHistory>(d => d.RetreatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__Retre__1DB06A4F");

            entity.HasOne(d => d.Tool).WithOne(p => p.ToolHistory)
                .HasForeignKey<ToolHistory>(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolI__1EA48E88");

            entity.HasOne(d => d.ToolOp).WithOne(p => p.ToolHistory)
                .HasForeignKey<ToolHistory>(d => d.ToolOpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ToolHisto__ToolO__1F98B2C1");
        });

        modelBuilder.Entity<ToolOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ToolOper__3214EC0774FC58FC");

            entity.ToTable("ToolOperation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
