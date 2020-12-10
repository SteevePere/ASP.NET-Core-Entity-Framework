using Microsoft.EntityFrameworkCore;


namespace Service.Models
{

    public partial class docnetContext : DbContext
    {

        public docnetContext()
        {
        }


        public docnetContext(DbContextOptions<docnetContext> options)
            : base(options)
        {
        }


        public virtual DbSet<ConditionsCdn> ConditionsCdn { get; set; }
        public virtual DbSet<GendersGdr> GendersGdr { get; set; }
        public virtual DbSet<PatientsPat> PatientsPat { get; set; }
        public virtual DbSet<PracticesPtc> PracticesPtc { get; set; }
        public virtual DbSet<ReportsRpt> ReportsRpt { get; set; }
        public virtual DbSet<RolesRle> RolesRle { get; set; }
        public virtual DbSet<TreatmentsTmt> TreatmentsTmt { get; set; }
        public virtual DbSet<UsersUsr> UsersUsr { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=docnet;Trusted_Connection=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConditionsCdn>(entity =>
            {
                entity.HasKey(e => e.CdnId)
                    .HasName("PK__conditio__4B4D15F9AD9AADE4");

                entity.ToTable("conditions_cdn");

                entity.Property(e => e.CdnId).HasColumnName("CDN_ID");

                entity.Property(e => e.CdnDescription)
                    .HasColumnName("CDN_DESCRIPTION")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CdnName)
                    .IsRequired()
                    .HasColumnName("CDN_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GendersGdr>(entity =>
            {
                entity.HasKey(e => e.GdrId)
                    .HasName("PK__genders___DC934B30625417F1");

                entity.ToTable("genders_gdr");

                entity.Property(e => e.GdrId).HasColumnName("GDR_ID");

                entity.Property(e => e.GdrName)
                    .IsRequired()
                    .HasColumnName("GDR_NAME")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('F')");
            });

            modelBuilder.Entity<PatientsPat>(entity =>
            {
                entity.HasKey(e => e.PatId)
                    .HasName("PK__patients__E85F08811669B7AB");

                entity.ToTable("patients_pat");

                entity.HasIndex(e => e.GdrId)
                    .HasName("FK_PAT_GDR");

                entity.HasIndex(e => e.PtcId)
                    .HasName("FK_PAT_PTC");

                entity.Property(e => e.PatId).HasColumnName("PAT_ID");

                entity.Property(e => e.GdrId).HasColumnName("GDR_ID");

                entity.Property(e => e.PatDob)
                    .HasColumnName("PAT_DOB")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.PatFirstName)
                    .IsRequired()
                    .HasColumnName("PAT_FIRST_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatHeight).HasColumnName("PAT_HEIGHT");

                entity.Property(e => e.PatIsPregnant)
                    .HasColumnName("PAT_IS_PREGNANT")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.PatIsSmoker)
                    .HasColumnName("PAT_IS_SMOKER")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.PatLastName)
                    .IsRequired()
                    .HasColumnName("PAT_LAST_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatWeight).HasColumnName("PAT_WEIGHT");

                entity.Property(e => e.PtcId).HasColumnName("PTC_ID");

                entity.HasOne(d => d.Gdr)
                    .WithMany(p => p.PatientsPat)
                    .HasForeignKey(d => d.GdrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAT_GDR");

                entity.HasOne(d => d.Ptc)
                    .WithMany(p => p.PatientsPat)
                    .HasForeignKey(d => d.PtcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAT_PTC");
            });

            modelBuilder.Entity<PracticesPtc>(entity =>
            {
                entity.HasKey(e => e.PtcId)
                    .HasName("PK__practice__36D28CBBF617A143");

                entity.ToTable("practices_ptc");

                entity.HasIndex(e => e.PtcName)
                    .HasName("PTC_NAME")
                    .IsUnique();

                entity.Property(e => e.PtcId).HasColumnName("PTC_ID");

                entity.Property(e => e.PtcAddress)
                    .IsRequired()
                    .HasColumnName("PTC_ADDRESS")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PtcCity)
                    .IsRequired()
                    .HasColumnName("PTC_CITY")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PtcName)
                    .IsRequired()
                    .HasColumnName("PTC_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PtcZipCode)
                    .IsRequired()
                    .HasColumnName("PTC_ZIP_CODE")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReportsRpt>(entity =>
            {
                entity.HasKey(e => e.RptId)
                    .HasName("PK__reports___F6FF16285AD9F63C");

                entity.ToTable("reports_rpt");

                entity.HasIndex(e => e.CdnId)
                    .HasName("FK_RPT_CDN");

                entity.HasIndex(e => e.PatId)
                    .HasName("FK_RPT_PAT");

                entity.HasIndex(e => e.TmtId)
                    .HasName("FK_RPT_TMT");

                entity.Property(e => e.RptId).HasColumnName("RPT_ID");

                entity.Property(e => e.CdnId).HasColumnName("CDN_ID");

                entity.Property(e => e.PatId).HasColumnName("PAT_ID");

                entity.Property(e => e.RptComment)
                    .HasColumnName("RPT_COMMENT")
                    .IsUnicode(false);

                entity.Property(e => e.RptCreationDatetime)
                    .HasColumnName("RPT_CREATION_DATETIME")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.RptEditionDatetime)
                    .HasColumnName("RPT_EDITION_DATETIME")
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RptRating).HasColumnName("RPT_RATING");

                entity.Property(e => e.TmtId).HasColumnName("TMT_ID");

                entity.HasOne(d => d.Cdn)
                    .WithMany(p => p.ReportsRpt)
                    .HasForeignKey(d => d.CdnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RPT_CDN");

                entity.HasOne(d => d.Pat)
                    .WithMany(p => p.ReportsRpt)
                    .HasForeignKey(d => d.PatId)
                    .HasConstraintName("FK_RPT_PAT");

                entity.HasOne(d => d.Tmt)
                    .WithMany(p => p.ReportsRpt)
                    .HasForeignKey(d => d.TmtId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RPT_TMT");
            });

            modelBuilder.Entity<RolesRle>(entity =>
            {
                entity.HasKey(e => e.RleId)
                    .HasName("PK__roles_rl__7B4EEEA7CAF97737");

                entity.ToTable("roles_rle");

                entity.Property(e => e.RleId).HasColumnName("RLE_ID");

                entity.Property(e => e.RleName)
                    .IsRequired()
                    .HasColumnName("RLE_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TreatmentsTmt>(entity =>
            {
                entity.HasKey(e => e.TmtId)
                    .HasName("PK__treatmen__E18A30F46B7867C2");

                entity.ToTable("treatments_tmt");

                entity.Property(e => e.TmtId).HasColumnName("TMT_ID");

                entity.Property(e => e.TmtDescription)
                    .HasColumnName("TMT_DESCRIPTION")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TmtName).HasColumnName("TMT_NAME");
            });

            modelBuilder.Entity<UsersUsr>(entity =>
            {
                entity.HasKey(e => e.UsrId)
                    .HasName("PK__users_us__91DE227692FF1A2A");

                entity.ToTable("users_usr");

                entity.HasIndex(e => e.GdrId)
                    .HasName("FK_USR_GDR");

                entity.HasIndex(e => e.PtcId)
                    .HasName("FK_USR_PTC");

                entity.HasIndex(e => e.RleId)
                    .HasName("FK_USR_RLE");

                entity.HasIndex(e => e.UsrEmail)
                    .HasName("USR_EMAIL")
                    .IsUnique();

                entity.Property(e => e.UsrId).HasColumnName("USR_ID");

                entity.Property(e => e.GdrId).HasColumnName("GDR_ID");

                entity.Property(e => e.PtcId).HasColumnName("PTC_ID");

                entity.Property(e => e.RleId).HasColumnName("RLE_ID");

                entity.Property(e => e.UsrActive)
                    .HasColumnName("USR_ACTIVE")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.UsrCreationDatetime)
                    .HasColumnName("USR_CREATION_DATETIME")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.UsrEditDatetime)
                    .HasColumnName("USR_EDIT_DATETIME")
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsrEmail)
                    .IsRequired()
                    .HasColumnName("USR_EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrFirstName)
                    .IsRequired()
                    .HasColumnName("USR_FIRST_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrLastName)
                    .IsRequired()
                    .HasColumnName("USR_LAST_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrPassword)
                    .IsRequired()
                    .HasColumnName("USR_PASSWORD")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gdr)
                    .WithMany(p => p.UsersUsr)
                    .HasForeignKey(d => d.GdrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USR_GDR");

                entity.HasOne(d => d.Ptc)
                    .WithMany(p => p.UsersUsr)
                    .HasForeignKey(d => d.PtcId)
                    .HasConstraintName("FK_USR_PTC");

                entity.HasOne(d => d.Rle)
                    .WithMany(p => p.UsersUsr)
                    .HasForeignKey(d => d.RleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USR_RLE");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
