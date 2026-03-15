using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWholeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Cities_CityId",
                table: "Ads");

            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Governorates_GovernorateId",
                table: "Ads");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Ads_AdId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Ads_CityId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_GovernorateId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Images");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdId",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Ads",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserSecurity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BlockedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginIp = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecurity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ads_Slug",
                table: "Ads",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurity_UserId",
                table: "UserSecurity",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Ads_AdId",
                table: "Images",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Ads_AdId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "UserSecurity");

            migrationBuilder.DropIndex(
                name: "IX_Ads_Slug",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Ads");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdId",
                table: "Images",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Images",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    Lat = table.Column<double>(type: "numeric(9,6)", nullable: true),
                    Lng = table.Column<double>(type: "numeric(9,6)", nullable: true),
                    NameAr = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "Id", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, "القاهرة", "Cairo" },
                    { 2, "الإسكندرية", "Alexandria" },
                    { 3, "الجيزة", "Giza" },
                    { 4, "بورسعيد", "PortSaid" },
                    { 5, "السويس", "Suez" },
                    { 6, "الأقصر", "Luxor" },
                    { 7, "أسوان", "Aswan" },
                    { 8, "أسيوط", "Asyut" },
                    { 9, "بني سويف", "BeniSuef" },
                    { 10, "الفيوم", "Faiyum" },
                    { 11, "الغربية", "Gharbia" },
                    { 12, "الإسماعيلية", "Ismailia" },
                    { 13, "المنوفية", "Menofia" },
                    { 14, "المنيا", "Minya" },
                    { 15, "القليوبية", "Qalyubia" },
                    { 16, "الوادي الجديد", "NewValley" },
                    { 17, "شمال سيناء", "NorthSinai" },
                    { 18, "الشرقية", "Sharqia" },
                    { 19, "سوهاج", "Sohag" },
                    { 20, "جنوب سيناء", "SouthSinai" },
                    { 21, "كفر الشيخ", "KafrElSheikh" },
                    { 22, "مطروح", "Matrouh" },
                    { 23, "قنا", "Qena" },
                    { 24, "دمياط", "Damietta" },
                    { 25, "الدقهلية", "Dakahlia" },
                    { 26, "البحر الأحمر", "RedSea" },
                    { 27, "البحيرة", "Beheira" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "GovernorateId", "Lat", "Lng", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 101, 1, 30.0444, 31.235700000000001, "القاهرة", "Cairo" },
                    { 102, 1, 30.091100000000001, 31.342400000000001, "مصر الجديدة", "Heliopolis" },
                    { 103, 1, 30.0626, 31.336099999999998, "مدينة نصر", "NasrCity" },
                    { 104, 1, 29.9602, 31.256900000000002, "المعادي", "Maadi" },
                    { 105, 1, 30.0626, 31.2197, "الزمالك", "Zamalek" },
                    { 106, 1, 30.103200000000001, 31.244199999999999, "شبرا", "Shubra" },
                    { 107, 1, 30.027100000000001, 31.496099999999998, "القاهرة الجديدة", "NewCairo" },
                    { 108, 1, 29.9391, 30.937000000000001, "السادس من أكتوبر", "SixthOfOctober" },
                    { 109, 1, 29.849799999999998, 31.3337, "حلوان", "Helwan" },
                    { 110, 1, 30.146899999999999, 31.617799999999999, "الشروق", "Shorouk" },
                    { 111, 1, 30.128599999999999, 31.724299999999999, "بدر", "Badr" },
                    { 112, 1, 30.209299999999999, 31.729099999999999, "العبور", "Obour" },
                    { 113, 1, 30.1218, 31.317499999999999, "عين شمس", "AinShams" },
                    { 114, 1, 30.144400000000001, 31.3537, "المرج", "Marg" },
                    { 115, 1, 30.216699999999999, 31.683299999999999, "مدينة المستقبل", "FutureCity" },
                    { 201, 2, 31.2881, 30.010000000000002, "المنتزه", "Montaza" },
                    { 202, 2, 31.215599999999998, 29.955300000000001, "سيدي جابر", "SidiGaber" },
                    { 203, 2, 31.206700000000001, 29.952200000000001, "سموحة", "Smouha" },
                    { 204, 2, 31.138100000000001, 29.779499999999999, "العجمي", "Agami" },
                    { 205, 2, 30.896899999999999, 29.59, "برج العرب", "BorgElArab" },
                    { 206, 2, 31.166699999999999, 29.850000000000001, "الدخيلة", "Dekheila" },
                    { 207, 2, 31.278400000000001, 29.991700000000002, "المندرة", "Mandara" },
                    { 208, 2, 31.223299999999998, 29.941700000000001, "جليم", "Glim" },
                    { 209, 2, 31.260100000000001, 29.9801, "ميامي", "Miami" },
                    { 210, 2, 31.197500000000002, 29.911100000000001, "باب شرقي", "BabSharqi" },
                    { 211, 2, 31.033300000000001, 29.800000000000001, "العامرية", "Ameria" },
                    { 212, 2, 31.318899999999999, 30.0639, "أبوقير", "AbuQir" },
                    { 301, 3, 30.039200000000001, 31.213100000000001, "الدقي", "Dokki" },
                    { 302, 3, 30.055599999999998, 31.199999999999999, "المهندسين", "Mohandessin" },
                    { 303, 3, 29.986999999999998, 31.1571, "الهرم", "Haram" },
                    { 304, 3, 29.969999999999999, 31.129999999999999, "فيصل", "Faisal" },
                    { 305, 3, 30.0731, 31.213100000000001, "إمبابة", "Imbaba" },
                    { 306, 3, 30.017499999999998, 30.9483, "الشيخ زايد", "SheikhZayed" },
                    { 307, 3, 29.9391, 30.937000000000001, "أكتوبر", "October" },
                    { 308, 3, 29.846299999999999, 31.252600000000001, "البدرشين", "Badrashin" },
                    { 309, 3, 29.600000000000001, 31.216699999999999, "العياط", "Ayat" },
                    { 310, 3, 29.5703, 31.279199999999999, "الصف", "Saf" },
                    { 311, 3, 29.416699999999999, 31.2667, "أطفيح", "Atfih" },
                    { 312, 3, 30.116700000000002, 31.116700000000002, "أوسيم", "Ausim" },
                    { 313, 3, 30.033300000000001, 31.116700000000002, "كرداسة", "Kerdasa" },
                    { 401, 4, 31.25, 32.333300000000001, "بورفؤاد", "PortFuad" },
                    { 402, 4, 31.2639, 32.302799999999998, "المناخ", "AlManakh" },
                    { 403, 4, 31.257200000000001, 32.297800000000002, "الزهور", "AlZuhur" },
                    { 404, 4, 31.268599999999999, 32.321399999999997, "الشرق", "AlSharq" },
                    { 405, 4, 31.216699999999999, 32.283299999999997, "الضواحي", "AlDawahi" },
                    { 406, 4, 31.2333, 32.2667, "العرب", "AlArab" },
                    { 407, 4, 31.204999999999998, 32.274999999999999, "الجنوب", "AlJanoub" },
                    { 501, 5, 29.973700000000001, 32.537599999999998, "الأربعين", "Arbaeen" },
                    { 502, 5, 29.949999999999999, 32.4833, "عتاقة", "Ataka" },
                    { 503, 5, 30.0167, 32.549999999999997, "فيصل", "FaisalSuez" },
                    { 504, 5, 29.933299999999999, 32.5167, "الجناين", "Ganayen" },
                    { 601, 6, 25.687200000000001, 32.639600000000002, "الأقصر", "LuxorCity" },
                    { 602, 6, 25.616700000000002, 32.533299999999997, "أرمنت", "Armant" },
                    { 603, 6, 25.293299999999999, 32.557200000000002, "إسنا", "Esna" },
                    { 604, 6, 25.716699999999999, 32.683300000000003, "طيبة الجديدة", "NewTiba" },
                    { 605, 6, 25.75, 32.616700000000002, "البياضية", "Bayadiya" },
                    { 701, 7, 24.088899999999999, 32.899799999999999, "أسوان", "AswanCity" },
                    { 702, 7, 24.977799999999998, 32.873899999999999, "إدفو", "Edfu" },
                    { 703, 7, 24.472200000000001, 32.948900000000002, "كوم أمبو", "KomOmbo" },
                    { 704, 7, 24.404199999999999, 32.9375, "دراو", "Daraw" },
                    { 705, 7, 22.337199999999999, 31.625800000000002, "أبو سمبل", "AbuSimbel" },
                    { 706, 7, 23.566700000000001, 32.916699999999999, "ناصر", "Nasr" },
                    { 707, 7, 23.583300000000001, 32.883299999999998, "كلابشة", "Kalabsha" },
                    { 801, 8, 27.1783, 31.1859, "أسيوط", "AsiutCity" },
                    { 802, 8, 27.2667, 31.149999999999999, "أبنوب", "Abnub" },
                    { 803, 8, 27.050000000000001, 31.316700000000001, "أبوتيج", "AbuTig" },
                    { 804, 8, 27.550000000000001, 30.816700000000001, "ديروط", "Dairut" },
                    { 805, 8, 27.3108, 30.9697, "منفلوط", "Manfalut" },
                    { 806, 8, 27.066700000000001, 31.216699999999999, "ساحل سليم", "SahelSelim" },
                    { 807, 8, 27.438300000000002, 30.819700000000001, "القوصية", "Qusiya" },
                    { 808, 8, 26.9833, 31.416699999999999, "البداري", "Badari" },
                    { 809, 8, 27.199999999999999, 31.25, "أسيوط الجديدة", "NewAsiut" },
                    { 901, 9, 29.066099999999999, 31.099399999999999, "بني سويف", "BeniSuefCity" },
                    { 902, 9, 28.916699999999999, 31.083300000000001, "ببا", "Biba" },
                    { 903, 9, 28.816700000000001, 30.883299999999998, "الفشن", "Fashn" },
                    { 904, 9, 29.033300000000001, 30.9833, "ناصر", "Nasser" },
                    { 905, 9, 28.7333, 30.850000000000001, "سمسطا", "Sumusta" },
                    { 906, 9, 29.083300000000001, 30.933299999999999, "إهناسيا المدينة", "IhnasiaElMadina" },
                    { 907, 9, 29.350000000000001, 31.216699999999999, "الواسطى", "Wasta" },
                    { 1001, 10, 29.308399999999999, 30.8428, "الفيوم", "FaiyumCity" },
                    { 1002, 10, 29.350000000000001, 30.683299999999999, "إبشواي", "Ibsheway" },
                    { 1003, 10, 29.2333, 30.7333, "إطسا", "Itsa" },
                    { 1004, 10, 29.416699999999999, 30.883299999999998, "سنورس", "Sinnuris" },
                    { 1005, 10, 29.183299999999999, 30.649999999999999, "يوسف الصديق", "YusufSediq" },
                    { 1006, 10, 29.4833, 30.949999999999999, "طامية", "Tamiya" },
                    { 1101, 11, 30.7865, 31.000399999999999, "طنطا", "Tanta" },
                    { 1102, 11, 30.972799999999999, 31.162199999999999, "المحلة الكبرى", "Mahalla" },
                    { 1103, 11, 30.816700000000001, 30.816700000000001, "كفر الزيات", "KafrElZayat" },
                    { 1104, 11, 30.716699999999999, 31.25, "زفتى", "Zefta" },
                    { 1105, 11, 30.966699999999999, 31.25, "سمنود", "Samanoud" },
                    { 1106, 11, 30.883299999999998, 30.9833, "بسيون", "Basyoun" },
                    { 1107, 11, 30.850000000000001, 30.933299999999999, "قطور", "Qutur" },
                    { 1108, 11, 30.949999999999999, 30.883299999999998, "السنطة", "Santa" },
                    { 1201, 12, 30.596499999999999, 32.271500000000003, "الإسماعيلية", "IsmailiACity" },
                    { 1202, 12, 30.316700000000001, 32.350000000000001, "فايد", "Fayed" },
                    { 1203, 12, 30.833300000000001, 32.333300000000001, "القنطرة شرق", "QantaraEast" },
                    { 1204, 12, 30.850000000000001, 32.316699999999997, "القنطرة غرب", "QantaraWest" },
                    { 1205, 12, 30.5167, 32.416699999999999, "أبوعطوة", "AbuAtewa" },
                    { 1206, 12, 30.550000000000001, 32.149999999999999, "القصاصين", "Kasaseen" },
                    { 1207, 12, 30.583300000000001, 32.083300000000001, "التل الكبير", "TallElKabir" },
                    { 1301, 13, 30.566700000000001, 30.966699999999999, "شبين الكوم", "ShebinElKom" },
                    { 1302, 13, 30.300000000000001, 30.9833, "أشمون", "Ashmoun" },
                    { 1303, 13, 30.466699999999999, 30.933299999999999, "منوف", "Menouf" },
                    { 1304, 13, 30.416699999999999, 30.883299999999998, "بركة السبع", "BirketElSab" },
                    { 1305, 13, 30.550000000000001, 30.850000000000001, "سرس الليان", "SersElLian" },
                    { 1306, 13, 30.666699999999999, 30.916699999999999, "تلا", "Tala" },
                    { 1307, 13, 30.5, 31.083300000000001, "قويسنا", "Quesna" },
                    { 1308, 13, 30.366700000000002, 30.5167, "مدينة السادات", "Sadat" },
                    { 1309, 13, 30.5, 30.966699999999999, "الباجور", "Bagour" },
                    { 1401, 14, 28.0871, 30.761800000000001, "المنيا", "MinyaCity" },
                    { 1402, 14, 27.933299999999999, 30.850000000000001, "أبوقرقاص", "AbuQurqas" },
                    { 1403, 14, 28.5, 30.800000000000001, "بني مزار", "BeniMazar" },
                    { 1404, 14, 27.649999999999999, 30.850000000000001, "دير مواس", "DeirMawas" },
                    { 1405, 14, 28.649999999999999, 30.850000000000001, "مغاغة", "Maghagha" },
                    { 1406, 14, 27.7333, 30.833300000000001, "ملوي", "Mallawi" },
                    { 1407, 14, 28.416699999999999, 30.783300000000001, "مطاي", "Matay" },
                    { 1408, 14, 28.316700000000001, 30.699999999999999, "سمالوط", "Samalut" },
                    { 1409, 14, 28.100000000000001, 30.800000000000001, "المنيا الجديدة", "NewMinya" },
                    { 1501, 15, 30.466699999999999, 31.183299999999999, "بنها", "Banha" },
                    { 1502, 15, 30.183299999999999, 31.199999999999999, "قليوب", "Qalyub" },
                    { 1503, 15, 30.128900000000002, 31.243600000000001, "شبرا الخيمة", "ShubralKheima" },
                    { 1504, 15, 30.216699999999999, 31.366700000000002, "الخانكة", "Khanka" },
                    { 1505, 15, 30.283300000000001, 31.199999999999999, "قها", "Qaha" },
                    { 1506, 15, 30.350000000000001, 31.199999999999999, "طوخ", "Tukh" },
                    { 1507, 15, 30.550000000000001, 31.2667, "كفر شكر", "KafrShukr" },
                    { 1508, 15, 30.166699999999999, 31.300000000000001, "الخصوص", "Khosous" },
                    { 1601, 16, 25.443899999999999, 30.551600000000001, "الخارجة", "Kharga" },
                    { 1602, 16, 25.488900000000001, 28.988, "الداخلة", "Dakhla" },
                    { 1603, 16, 27.059999999999999, 27.969999999999999, "الفرافرة", "Farafra" },
                    { 1604, 16, 24.649999999999999, 30.683299999999999, "باريس", "Baris" },
                    { 1605, 16, 25.5167, 29.2667, "بلاط", "Balat" },
                    { 1701, 17, 31.128900000000002, 33.7986, "العريش", "Arish" },
                    { 1702, 17, 31.216699999999999, 34.049999999999997, "الشيخ زويد", "SheikhZuweid" },
                    { 1703, 17, 31.283300000000001, 34.2333, "رفح", "Rafah" },
                    { 1704, 17, 30.933299999999999, 33.0167, "بئر العبد", "BirElAbd" },
                    { 1705, 17, 30.466699999999999, 33.799999999999997, "حسنة", "Hasna" },
                    { 1706, 17, 29.916699999999999, 33.7333, "نخل", "Nakhl" },
                    { 1801, 18, 30.587700000000002, 31.502099999999999, "الزقازيق", "Zagazig" },
                    { 1802, 18, 30.4222, 31.5611, "بلبيس", "Belbeis" },
                    { 1803, 18, 30.5167, 31.350000000000001, "منيا القمح", "MinyaElQamh" },
                    { 1804, 18, 30.7667, 31.666699999999999, "أبوحماد", "AbuHammad" },
                    { 1805, 18, 30.7333, 31.666699999999999, "أبوكبير", "AbuKabir" },
                    { 1806, 18, 30.7333, 31.800000000000001, "فاقوس", "Faqous" },
                    { 1807, 18, 30.649999999999999, 31.583300000000001, "ههيا", "Hehia" },
                    { 1808, 18, 30.916699999999999, 31.866700000000002, "كفر صقر", "KafrSaqr" },
                    { 1809, 18, 30.383299999999998, 31.4833, "القنايات", "Qenayet" },
                    { 1810, 18, 30.633299999999998, 31.5, "ديرب نجم", "DiyarbNegm" },
                    { 1811, 18, 30.933299999999999, 32.049999999999997, "الصالحية الجديدة", "SalhiyaElGedida" },
                    { 1812, 18, 30.866700000000002, 32.0167, "الحسينية", "Husseiniya" },
                    { 1813, 18, 30.300000000000001, 31.783300000000001, "العاشر من رمضان", "TenthOfRamadan" },
                    { 1901, 19, 26.556899999999999, 31.694800000000001, "سوهاج", "SohagCity" },
                    { 1902, 19, 26.566700000000001, 31.75, "أخميم", "Akhmim" },
                    { 1903, 19, 26.2333, 31.9833, "البلينا", "Balyana" },
                    { 1904, 19, 26.4833, 31.899999999999999, "دار السلام", "DarElSalam" },
                    { 1905, 19, 26.333300000000001, 31.883299999999998, "جرجا", "Gerga" },
                    { 1906, 19, 26.666699999999999, 31.416699999999999, "جهينة", "Juhayna" },
                    { 1907, 19, 26.699999999999999, 31.583300000000001, "المراغة", "Maragha" },
                    { 1908, 19, 26.616700000000002, 31.7667, "ساقلتة", "Saqultah" },
                    { 1909, 19, 26.7667, 31.5, "طهطا", "Tahta" },
                    { 1910, 19, 26.9833, 31.433299999999999, "تيما", "Tima" },
                    { 2001, 20, 27.915800000000001, 34.329999999999998, "شرم الشيخ", "SharmElSheikh" },
                    { 2002, 20, 28.488600000000002, 34.513599999999997, "دهب", "Dahab" },
                    { 2003, 20, 29.033300000000001, 34.666699999999999, "نويبع", "Nuweiba" },
                    { 2004, 20, 29.497199999999999, 34.8919, "طابا", "Taba" },
                    { 2005, 20, 29.583300000000001, 32.716700000000003, "رأس سدر", "RasSidr" },
                    { 2006, 20, 28.555, 33.976100000000002, "سانت كاترين", "SaintCatherine" },
                    { 2007, 20, 28.966699999999999, 33.200000000000003, "أبورديس", "AbuRudeis" },
                    { 2008, 20, 28.2333, 33.616700000000002, "طور سيناء", "TorSinai" },
                    { 2101, 21, 31.112200000000001, 30.939399999999999, "كفر الشيخ", "KafrElSheikhCity" },
                    { 2102, 21, 31.566700000000001, 31.083300000000001, "بلطيم", "Baltim" },
                    { 2103, 21, 31.166699999999999, 30.783300000000001, "بيلا", "Biyala" },
                    { 2104, 21, 31.133299999999998, 30.649999999999999, "دسوق", "Desouk" },
                    { 2105, 21, 31.199999999999999, 30.566700000000001, "فوه", "Fuwwa" },
                    { 2106, 21, 31.183299999999999, 30.833300000000001, "مطوبس", "Metoubes" },
                    { 2107, 21, 31.083300000000001, 30.833300000000001, "قلين", "Qallin" },
                    { 2108, 21, 31.283300000000001, 30.783300000000001, "سيدي سالم", "SidiSalem" },
                    { 2109, 21, 31.216699999999999, 30.850000000000001, "الحامول", "Hambat" },
                    { 2201, 22, 31.3538, 27.2453, "مرسى مطروح", "MarsaMatruh" },
                    { 2202, 22, 31.616700000000002, 25.916699999999999, "سيدي براني", "SidiBarrani" },
                    { 2203, 22, 29.203299999999999, 25.5197, "سيوة", "Siwa" },
                    { 2204, 22, 31.533300000000001, 25.166699999999999, "السلوم", "Salloum" },
                    { 2205, 22, 30.949999999999999, 28.433299999999999, "الضبعة", "Daba" },
                    { 2206, 22, 30.829999999999998, 28.949999999999999, "العلمين", "Alamein" },
                    { 2207, 22, 31.383299999999998, 26.666699999999999, "النجيلة", "Negila" },
                    { 2301, 23, 26.155100000000001, 32.716000000000001, "قنا", "QenaCity" },
                    { 2302, 23, 26.083300000000001, 32.166699999999999, "أبوتشت", "AbuTesht" },
                    { 2303, 23, 26.116700000000002, 32.450000000000003, "دشنا", "Deshna" },
                    { 2304, 23, 26.033300000000001, 32.183300000000003, "فرشوط", "Farshut" },
                    { 2305, 23, 25.899999999999999, 32.700000000000003, "نقادة", "Naqada" },
                    { 2306, 23, 26.048100000000002, 32.2453, "نجع حمادي", "NagHammadi" },
                    { 2307, 23, 25.9833, 32.816699999999997, "قفط", "Quft" },
                    { 2308, 23, 25.916699999999999, 32.75, "قوص", "Qus" },
                    { 2401, 24, 31.416499999999999, 31.813300000000002, "دمياط", "DamiettaCity" },
                    { 2402, 24, 31.333300000000001, 31.716699999999999, "فارسكور", "Faraskur" },
                    { 2403, 24, 31.366700000000002, 31.75, "كفر سعد", "KafrSaad" },
                    { 2404, 24, 31.350000000000001, 31.866700000000002, "الزرقا", "Zarqa" },
                    { 2405, 24, 31.399999999999999, 31.75, "دمياط الجديدة", "NewDamietta" },
                    { 2406, 24, 31.4833, 31.833300000000001, "رأس البر", "RasElBar" },
                    { 2501, 25, 31.0364, 31.380700000000001, "المنصورة", "Mansoura" },
                    { 2502, 25, 31.050000000000001, 31.366700000000002, "طلخا", "Talkha" },
                    { 2503, 25, 30.916699999999999, 31.316700000000001, "أجا", "Aga" },
                    { 2504, 25, 31.066700000000001, 31.566700000000001, "بلقاس", "Belqas" },
                    { 2505, 25, 31.083300000000001, 31.583300000000001, "دكرنس", "Dekernes" },
                    { 2506, 25, 31.366700000000002, 31.866700000000002, "جمصة", "Gamasa" },
                    { 2507, 25, 31.283300000000001, 31.916699999999999, "المنزلة", "Manzala" },
                    { 2508, 25, 30.716699999999999, 31.2667, "ميت غمر", "MeetGhamr" },
                    { 2509, 25, 30.816700000000001, 31.433299999999999, "شربين", "Sherbeen" },
                    { 2510, 25, 30.866700000000002, 31.5167, "سنبلاوين", "Sinbillawein" },
                    { 2511, 25, 31.183299999999999, 31.883299999999998, "المطرية", "Matariya" },
                    { 2512, 25, 31.083300000000001, 31.816700000000001, "منية النصر", "MinyatElNasr" },
                    { 2601, 26, 27.257400000000001, 33.812899999999999, "الغردقة", "Hurghada" },
                    { 2602, 26, 26.7333, 33.933300000000003, "سفاجا", "Safaga" },
                    { 2603, 26, 26.097799999999999, 34.281399999999998, "القصير", "Quseer" },
                    { 2604, 26, 25.066700000000001, 34.883299999999998, "مرسى علم", "MarsaAlam" },
                    { 2605, 26, 23.116700000000002, 35.583300000000001, "شلاتين", "Shalateen" },
                    { 2606, 26, 22.199999999999999, 36.633299999999998, "حلايب", "Halaib" },
                    { 2607, 26, 28.350000000000001, 33.083300000000001, "رأس غارب", "RasGharib" },
                    { 2701, 27, 31.034400000000002, 30.468499999999999, "دمنهور", "Damanhur" },
                    { 2702, 27, 31.133299999999998, 30.133299999999998, "كفر الدوار", "KafrElDawwar" },
                    { 2703, 27, 31.403600000000001, 30.415800000000001, "رشيد", "Rashid" },
                    { 2704, 27, 30.899999999999999, 30.25, "أبوحمص", "AbuHomos" },
                    { 2705, 27, 30.7667, 30.283300000000001, "أبومطامير", "AbuMatamir" },
                    { 2706, 27, 30.866700000000002, 30.533300000000001, "دلنجات", "Delengat" },
                    { 2707, 27, 31.300000000000001, 30.283300000000001, "إدكو", "Edku" },
                    { 2708, 27, 30.899999999999999, 30.583300000000001, "حوش عيسى", "HoshIssa" },
                    { 2709, 27, 30.783300000000001, 30.616700000000002, "إيتاي البارود", "ItayElBaroud" },
                    { 2710, 27, 30.7667, 30.683299999999999, "كوم حمادة", "KomHamada" },
                    { 2711, 27, 31.183299999999999, 30.533300000000001, "المحمودية", "Mahmoudiya" },
                    { 2712, 27, 30.583300000000001, 30.316700000000001, "النوبارية", "Nubaria" },
                    { 2713, 27, 30.916699999999999, 30.416699999999999, "الرحمانية", "Rahmaniya" },
                    { 2714, 27, 30.9833, 30.616700000000002, "شبراخيت", "Shubrakhit" },
                    { 2715, 27, 30.283300000000001, 30.283300000000001, "وادي النطرون", "WadiElNatrun" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ads_CityId",
                table: "Ads",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_GovernorateId",
                table: "Ads",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovernorateId",
                table: "Cities",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Id_GovernorateId",
                table: "Cities",
                columns: new[] { "Id", "GovernorateId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Cities_CityId",
                table: "Ads",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Governorates_GovernorateId",
                table: "Ads",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Ads_AdId",
                table: "Images",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id");
        }
    }
}
