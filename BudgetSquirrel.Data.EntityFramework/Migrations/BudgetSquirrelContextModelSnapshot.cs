﻿// <auto-generated />
using System;
using BudgetSquirrel.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BudgetSquirrel.Data.EntityFramework.Migrations
{
    [DbContext(typeof(BudgetSquirrelContext))]
    partial class BudgetSquirrelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BudgetId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BudgetPeriodId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateFinalizedTo")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FundId")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PercentAmount")
                        .HasColumnType("REAL");

                    b.Property<decimal>("SetAmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("BudgetPeriodId");

                    b.HasIndex("FundId");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.BudgetDurationBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BudgetDurations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BudgetDurationBase");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.Fund", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DurationId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FundBalance")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentFundId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DurationId");

                    b.HasIndex("ParentFundId");

                    b.HasIndex("UserId");

                    b.ToTable("Funds");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.Tracking.BudgetPeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BudgetPeriods");
                });

            modelBuilder.Entity("BudgetSquirrel.Data.EntityFramework.Models.UserRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.DaySpanDuration", b =>
                {
                    b.HasBaseType("BudgetSquirrel.Business.BudgetPlanning.BudgetDurationBase");

                    b.Property<int>("NumberDays")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("DaySpanDuration");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.MonthlyBookEndedDuration", b =>
                {
                    b.HasBaseType("BudgetSquirrel.Business.BudgetPlanning.BudgetDurationBase");

                    b.Property<int>("EndDayOfMonth")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RolloverEndDateOnSmallMonths")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("MonthlyBookEndedDuration");
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.Budget", b =>
                {
                    b.HasOne("BudgetSquirrel.Business.BudgetPlanning.Budget", null)
                        .WithMany("SubBudgets")
                        .HasForeignKey("BudgetId");

                    b.HasOne("BudgetSquirrel.Business.Tracking.BudgetPeriod", "BudgetPeriod")
                        .WithMany()
                        .HasForeignKey("BudgetPeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetSquirrel.Business.BudgetPlanning.Fund", "Fund")
                        .WithMany()
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BudgetSquirrel.Business.BudgetPlanning.Fund", b =>
                {
                    b.HasOne("BudgetSquirrel.Business.BudgetPlanning.BudgetDurationBase", "Duration")
                        .WithMany()
                        .HasForeignKey("DurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BudgetSquirrel.Business.BudgetPlanning.Fund", "ParentFund")
                        .WithMany("SubFunds")
                        .HasForeignKey("ParentFundId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BudgetSquirrel.Data.EntityFramework.Models.UserRecord", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
