using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20231224161837 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "项目名"),
                    Path = table.Column<string>(type: "text", nullable: false, comment: "项目路径"),
                    GitBranch = table.Column<string>(type: "text", nullable: false, comment: "Git分支"),
                    AddEnumRemark = table.Column<bool>(type: "boolean", nullable: false, comment: "是否添加枚举的remark信息"),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                },
                comment: "项目定义");

            migrationBuilder.CreateTable(
                name: "Maids",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "名称"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    MaidWork = table.Column<int>(type: "integer", nullable: false, comment: "功能(0-配置同步功能,1-DTO同步,2-HTTP客户端生成,3-controller同步,4-生成 Masstransit Consumer)"),
                    SourcePath = table.Column<string>(type: "text", nullable: false, comment: "原路径"),
                    DestinationPath = table.Column<string>(type: "text", nullable: false, comment: "目标路径"),
                    Autonomous = table.Column<bool>(type: "boolean", nullable: false, comment: "是否自动修复"),
                    Setting = table.Column<string>(type: "text", nullable: false, comment: "设置"),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maids_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "功能");

            migrationBuilder.CreateTable(
                name: "ClassDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaidId = table.Column<long>(type: "bigint", nullable: false, comment: "Maid对象Id"),
                    NameSpace = table.Column<string>(type: "text", nullable: true, comment: "命名空间"),
                    Modifiers = table.Column<string>(type: "text", nullable: true, comment: "修饰符"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "类名"),
                    Summary = table.Column<string>(type: "text", nullable: true, comment: "注释"),
                    Base = table.Column<string>(type: "text", nullable: true, comment: "基类或者接口名称"),
                    Using = table.Column<string>(type: "text", nullable: false, comment: "类引用的命名空间"),
                    LeadingTrivia = table.Column<string>(type: "text", nullable: true, comment: "前导"),
                    MemberType = table.Column<int>(type: "integer", nullable: false, comment: "成员类型(0-类,1-接口,2-记录,3-结构体)"),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassDefinitions_Maids_MaidId",
                        column: x => x.MaidId,
                        principalTable: "Maids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "类定义");

            migrationBuilder.CreateTable(
                name: "EnumDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameSpace = table.Column<string>(type: "text", nullable: true, comment: "命名空间"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "枚举名"),
                    Summary = table.Column<string>(type: "text", nullable: true, comment: "注释"),
                    LeadingTrivia = table.Column<string>(type: "text", nullable: true, comment: "前导"),
                    MaidId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumDefinitions_Maids_MaidId",
                        column: x => x.MaidId,
                        principalTable: "Maids",
                        principalColumn: "Id");
                },
                comment: "枚举定义");

            migrationBuilder.CreateTable(
                name: "EnumMemberDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "枚举名称"),
                    Value = table.Column<int>(type: "integer", nullable: false, comment: "枚举值"),
                    Summary = table.Column<string>(type: "text", nullable: true, comment: "注释"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "描述"),
                    EnumDefinitionId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumMemberDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumMemberDefinitions_EnumDefinitions_EnumDefinitionId",
                        column: x => x.EnumDefinitionId,
                        principalTable: "EnumDefinitions",
                        principalColumn: "Id");
                },
                comment: "枚举成员定义");

            migrationBuilder.CreateTable(
                name: "PropertyDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassDefinitionId = table.Column<long>(type: "bigint", nullable: false, comment: "所属类Id"),
                    LeadingTrivia = table.Column<string>(type: "text", nullable: true, comment: "前导"),
                    Summary = table.Column<string>(type: "text", nullable: true, comment: "注释"),
                    Remark = table.Column<string>(type: "text", nullable: true, comment: "备注"),
                    FullText = table.Column<string>(type: "text", nullable: false, comment: "完整文本内容"),
                    Modifiers = table.Column<string>(type: "text", nullable: false, comment: "修饰符"),
                    Initializer = table.Column<string>(type: "text", nullable: true, comment: "初始化器"),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "属性名称"),
                    Type = table.Column<string>(type: "text", nullable: false, comment: "数据类型"),
                    IsEnum = table.Column<bool>(type: "boolean", nullable: false, comment: "是否是枚举"),
                    HasGet = table.Column<bool>(type: "boolean", nullable: false, comment: "是否包含Get"),
                    Get = table.Column<string>(type: "text", nullable: true, comment: "Get方法体"),
                    HasSet = table.Column<bool>(type: "boolean", nullable: false, comment: "是否包含Set"),
                    Set = table.Column<string>(type: "text", nullable: true, comment: "Set方法体"),
                    EnumDefinitionId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyDefinitions_ClassDefinitions_ClassDefinitionId",
                        column: x => x.ClassDefinitionId,
                        principalTable: "ClassDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyDefinitions_EnumDefinitions_EnumDefinitionId",
                        column: x => x.EnumDefinitionId,
                        principalTable: "EnumDefinitions",
                        principalColumn: "Id");
                },
                comment: "类定义");

            migrationBuilder.CreateTable(
                name: "AttributeDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Attribute名称"),
                    Text = table.Column<string>(type: "text", nullable: false, comment: "Attribute文本"),
                    ArgumentsText = table.Column<string>(type: "text", nullable: true, comment: "参数文本"),
                    Arguments = table.Column<string>(type: "text", nullable: true, comment: "参数"),
                    PropertyDefinitionId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeDefinitions_PropertyDefinitions_PropertyDefinition~",
                        column: x => x.PropertyDefinitionId,
                        principalTable: "PropertyDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "属性定义");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDefinitions_PropertyDefinitionId",
                table: "AttributeDefinitions",
                column: "PropertyDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_MaidId",
                table: "ClassDefinitions",
                column: "MaidId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions",
                columns: new[] { "Name", "MaidId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnumDefinitions_MaidId",
                table: "EnumDefinitions",
                column: "MaidId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumMemberDefinitions_EnumDefinitionId",
                table: "EnumMemberDefinitions",
                column: "EnumDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Maids_ProjectId",
                table: "Maids",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_ClassDefinitionId",
                table: "PropertyDefinitions",
                column: "ClassDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_EnumDefinitionId",
                table: "PropertyDefinitions",
                column: "EnumDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_Name_ClassDefinitionId",
                table: "PropertyDefinitions",
                columns: new[] { "Name", "ClassDefinitionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeDefinitions");

            migrationBuilder.DropTable(
                name: "EnumMemberDefinitions");

            migrationBuilder.DropTable(
                name: "PropertyDefinitions");

            migrationBuilder.DropTable(
                name: "ClassDefinitions");

            migrationBuilder.DropTable(
                name: "EnumDefinitions");

            migrationBuilder.DropTable(
                name: "Maids");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
