﻿// <auto-generated />
using System;
using Cida.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace CIDA.Data.Migrations
{
    [DbContext(typeof(CidaDbContext))]
    [Migration("20240910185620_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CIDA.Domain.Entities.Arquivo", b =>
                {
                    b.Property<int>("IdArquivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_ARQUIVO");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdArquivo"));

                    b.Property<DateTime>("DataUpload")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("DATA_UPLOAD");

                    b.Property<string>("Extensao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("EXTENSAO");

                    b.Property<int?>("IdResumo")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_RESUMO");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_USUARIO");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("NOME");

                    b.Property<int>("Tamanho")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("TAMANHO");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("URL");

                    b.HasKey("IdArquivo");

                    b.HasIndex("IdResumo");

                    b.HasIndex("IdUsuario");

                    b.HasIndex("Url")
                        .IsUnique();

                    b.ToTable("T_OP_ARQUIVO");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Autenticacao", b =>
                {
                    b.Property<int>("IdAutenticacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_AUTENTICACAO");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAutenticacao"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("HashSenha")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("HASH_SENHA");

                    b.HasKey("IdAutenticacao");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("T_OP_AUTENTICACAO");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Insight", b =>
                {
                    b.Property<int>("IdInsight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_INSIGHT");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdInsight"));

                    b.Property<DateTime>("DataGeracao")
                        .HasColumnType("DATE")
                        .HasColumnName("DATA_GERACAO");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(8000)
                        .HasColumnType("NCLOB")
                        .HasColumnName("DESCRICAO");

                    b.Property<int?>("IdResumo")
                        .IsRequired()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_RESUMO");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_USUARIO");

                    b.HasKey("IdInsight");

                    b.HasIndex("IdResumo")
                        .IsUnique();

                    b.HasIndex("IdUsuario");

                    b.ToTable("T_OP_INSIGHT");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Resumo", b =>
                {
                    b.Property<int>("IdResumo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_RESUMO");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdResumo"));

                    b.Property<DateTime>("DataGeracao")
                        .HasColumnType("DATE")
                        .HasColumnName("DATA_GERACAO");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(8000)
                        .HasColumnType("NCLOB")
                        .HasColumnName("DESCRICAO");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_USUARIO");

                    b.HasKey("IdResumo");

                    b.HasIndex("IdUsuario");

                    b.ToTable("T_OP_RESUMO");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_USUARIO");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("DATE")
                        .HasColumnName("DATA_CRIACAO");

                    b.Property<int>("IdAutenticacao")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("ID_AUTENTICACAO");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("NOME");

                    b.Property<string>("NumDocumento")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("NUM_DOCUMENTO");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("STATUS");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)")
                        .HasColumnName("TELEFONE");

                    b.Property<string>("TipoDocumento")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("NVARCHAR2(4)")
                        .HasColumnName("TIPO_DOCUMENTO");

                    b.HasKey("IdUsuario");

                    b.HasIndex("IdAutenticacao")
                        .IsUnique();

                    b.HasIndex("NumDocumento", "IdAutenticacao")
                        .IsUnique();

                    b.ToTable("T_OP_USUARIO");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Arquivo", b =>
                {
                    b.HasOne("CIDA.Domain.Entities.Resumo", "Resumo")
                        .WithMany("Arquivos")
                        .HasForeignKey("IdResumo");

                    b.HasOne("CIDA.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Arquivos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resumo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Insight", b =>
                {
                    b.HasOne("CIDA.Domain.Entities.Resumo", "Resumo")
                        .WithMany("Insights")
                        .HasForeignKey("IdResumo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CIDA.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Insights")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resumo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Resumo", b =>
                {
                    b.HasOne("CIDA.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Resumos")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Usuario", b =>
                {
                    b.HasOne("CIDA.Domain.Entities.Autenticacao", "Autenticacao")
                        .WithOne("Usuario")
                        .HasForeignKey("CIDA.Domain.Entities.Usuario", "IdAutenticacao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autenticacao");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Autenticacao", b =>
                {
                    b.Navigation("Usuario")
                        .IsRequired();
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Resumo", b =>
                {
                    b.Navigation("Arquivos");

                    b.Navigation("Insights");
                });

            modelBuilder.Entity("CIDA.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Arquivos");

                    b.Navigation("Insights");

                    b.Navigation("Resumos");
                });
#pragma warning restore 612, 618
        }
    }
}
