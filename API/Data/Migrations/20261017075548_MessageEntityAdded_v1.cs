using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class MessageEntityAdded_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberLike_Members_SourceMemberId",
                table: "MemberLike");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberLike_Members_TargetMemberId",
                table: "MemberLike");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Members_RecipientId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Members_SenderId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberLike",
                table: "MemberLike");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "MemberLike",
                newName: "Likes");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_RecipientId",
                table: "Messages",
                newName: "IX_Messages_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_MemberLike_TargetMemberId",
                table: "Likes",
                newName: "IX_Likes_TargetMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                columns: new[] { "SourceMemberId", "TargetMemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Members_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Members_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Members_RecipientId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Members_SenderId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "MemberLike");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecipientId",
                table: "Message",
                newName: "IX_Message_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_TargetMemberId",
                table: "MemberLike",
                newName: "IX_MemberLike_TargetMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberLike",
                table: "MemberLike",
                columns: new[] { "SourceMemberId", "TargetMemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLike_Members_SourceMemberId",
                table: "MemberLike",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberLike_Members_TargetMemberId",
                table: "MemberLike",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Members_RecipientId",
                table: "Message",
                column: "RecipientId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Members_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
