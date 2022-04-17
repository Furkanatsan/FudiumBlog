using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FudiumBlog.Data.Migrations
{
    public partial class SeedingCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)//bir şey eklemek yada düzenlemek için
        {
            migrationBuilder.Sql(
                 "INSERT INTO [FudiumBlogDB].dbo.Categories (Name,Description,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Python','Python Dili ile İlgili En Güncel Bilgiler','Python Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");
            migrationBuilder.Sql(
                            "INSERT INTO [FudiumBlogDB].dbo.Categories (Name,Description,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Java','Java Dili ile İlgili En Güncel Bilgiler','Java Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");
            migrationBuilder.Sql(
                            "INSERT INTO [FudiumBlogDB].dbo.Categories (Name,Description,Note,CreatedDate,CreatedByName,ModifiedDate,ModifiedByName,IsActive,IsDeleted) VALUES ('Dart','Dart Dili ile İlgili En Güncel Bilgiler','Dart Kategorisi',GETDATE(),'Migration',GETDATE(),'Migration',1,0)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)//bir şeyllerin silinmesi için
        {
            
        }
    }
}
