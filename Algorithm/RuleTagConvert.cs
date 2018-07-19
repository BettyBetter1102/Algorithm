using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    public class RuleTagConvert
    {
        string qianKuoHao = "{";
        string houKuoHao = "}";
        string huanhang = "\r\n";
        string baseCategory = "BaseMoviesCategory";
        string str= @"MovieSynopsis, //电影剧情梗概
        StrongRecentReleasedMovies, //同期上映电影（强）
        WeakRecentReleasedMovies, //同期上映电影（弱）
        InvestmentCost, //投资成本
        PositiveNetizensCommentsOnMovie, //网友评价电影（正）
        NegativeNetizensCommentsOnMovie, //网友评价电影（负）
        MovieProductionTeam, //电影制作团队
        MovieStarring, //电影主演
        BoxOfficeBoom, //票房大爆
        BoxOfficeDismal, //票房惨淡
        OpenNewCasting, //公布新选角
        MovieStarRedCarpetLook, //电影主演红毯形象
        MovieDirector, //电影导演
        MovieSoudtrack, //电影配乐
        MoviePlagiarism, //电影抄袭
        PositiveMovieCriticComments, //影评人评论（正）
        NegativeMovieCriticComments, //影评人评论（负）
        ShootingSite, //拍摄场地
        PositiveVisualEffects, //视觉效果（正）
        NegativeVisualEffects, //视觉效果（负）
        CrewAttendFestival, //剧组亮相电影节
        MovieReleaseTrailer, //电影发布预告片
        TrailerContent, //预告片内容
        MovieReleaseDate, //电影预计上映时间
        MoviemakerCooperation, //电影人合作
        MovieCooperationHistory, //历史电影合作";
//       string  str = @"MovieThemeSong, //影视主题曲
////MVContent, //MV内容
////MVDirector, //MV导演
////MVStarring, //MV主演
////PositiveNetizensCommentsOnMusic, //网友评价音乐作品（正）
////NegativeNetizensCommentsOnMusic, //网友评价音乐作品（负）
////NewAlbumRelease, //新专辑发布
////AlbumIntroduction, //专辑介绍
////PositiveAlbumCover, //专辑封面（正）
////NegativeAlbumCover, //专辑封面（负）
////PositivePopularityData, //热度数据（正）
////NegativePopularityData, //热度数据（负）
////MusicProductionTeam, //音乐制作团队
////PositiveMusicThemeStyle, //音乐主题与风格（正）
////NegativeMusicThemeStyle, //音乐主题与风格（负）
////ConcertBasicInformation, //演唱会基本信息
////ConcertTicketSales, //演唱会售票情况
////ConcertHighlights, //演唱会亮点
////PreviousConcerts, //往届演唱会情况
////MusicCreationCooperation, //创作/制作合作
////MusicCooperationHistory, //历史音乐合作
////CooperationExchangedComments, //合作双方互评
////NegativePersonalExternalImage, //个人外在形象（负面）";
        public void ConstructCatagory( string format)
        {
            
            format = "ParagraphCategory.{0},new {0}Category()";
            string[] strs = str.Split(new char[] { '\r', '\n' });
            string result = string.Empty;
            foreach (var item in strs)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                
                string[] names = item.Split(',');
                names[0]=names[0].Trim();
                names[1] = names[1].Trim();
                result += "{" + string.Format(format, names[0]) + "}," + names[1] + "\r\n";

            }

        }

        public void ConstructMethod(string format)
        {
           
            format = "{0}\r\npublic class {1}Category:{2}";
            string[] strs = str.Split(new char[] { '\r', '\n' });
            string result = string.Empty;
            foreach (var item in strs)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                string[] names = item.Split(',');
                result += string.Format(format, names[1], names[0], baseCategory) + huanhang + qianKuoHao + huanhang +
                    "public static List<string> TagList = null;" + huanhang + huanhang +
                    "public static List<string> ESQueries = new List<string>()" + huanhang +
                    qianKuoHao + huanhang +
                    houKuoHao + ";" + huanhang + huanhang +
                    "public override List<string> GetTagList()" + huanhang +
                    qianKuoHao + huanhang +
                    "return TagList;" + huanhang +
                    houKuoHao + huanhang + huanhang +
                    "public override List<string> GetESQueries()" + huanhang +
                    qianKuoHao + huanhang +
                    "return ESQueries;" + huanhang +
                    houKuoHao + huanhang +
                    houKuoHao +huanhang  ;
            }
    }
    }
}
