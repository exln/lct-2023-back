using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;

namespace MediWingWebAPI.Utils;

public class Utilitas
{
    public static RusEsili ParseRusEsiliCode(string code)
    {
        char section = ' ';
        int block = -1;
        int number = -1;
        int subnumber = -1;
        int? subsubnumber = null;

        Match match = Regex.Match(code, @"^([A-Za-z]+)(?:(\d{1,2}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?$");
        if (match.Success)
        {
            section = char.ToUpper(match.Groups[1].Value[0]);
            block = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : -1;
            number = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : -1;
            subnumber = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : -1;
            subsubnumber = match.Groups[5].Success ? int.Parse(match.Groups[5].Value) : null;
        }
        
        RusEsili service = new()
        {
            Section = section,
            Block = block,
            Number = number,
            Subnumber = subnumber,
            Subsubnumber = subsubnumber,
        };

        return service;
    }

    public static Mkb10 ParseMkb10Code(string code)
    {
        string chapter = "1";
        char litera = ' ';
        int num = -1;
        int? subnum = null;

        Match match = Regex.Match(code, @"^([A-Za-z]+)(\d+)(?:\.(\d+))?$");
        
        if (match.Success) {
            litera = char.ToUpper(match.Groups[1].Value[0]);
            num = match.Groups[2].Success ? (int.Parse(match.Groups[2].Value)) : -1 ;
            subnum = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
            chapter = ParseMkb10Chapter(litera, num);
        }
        

        Mkb10 mkb = new()
        {
            Chapter = chapter,
            Litera = litera,
            Number = num,
            Subnumber = subnum,
        };
        
        return mkb;
    }
    
    public static string ParseMkb10Chapter(char litera, int number)
    {
        string str = null;
        if (litera == 'A' || litera == 'B') 
        {
            str = "I";
        }
        else if (litera == 'C' | (litera == 'D' && number < 49)) 
        {
            str = "II";
        }
        else if (litera == 'D' && number > 48) 
        {
            str = "III";
        }
        else if (litera == 'E')
        {
            str = "IV";
        }
        else if (litera == 'F')
        {
            str = "V";
        }
        else if (litera == 'G')
        {
            str = "VI";
        }
        else if (litera == 'H')
        {
            if (number < 60)
            {
                str = "VII";
            }
            else
            {
                str = "VIII";
            }
        }
        else if (litera == 'I')
        {
            str = "IX";
        }
        else if (litera == 'J')
        {
            str = "X";
        }
        else if (litera == 'K')
        {
            str = "XI";
        }
        else if (litera == 'L')
        {
            str = "XII";
        }
        else if (litera == 'M')
        {
            str = "XIII";
        }
        else if (litera == 'N')
        {
            str = "XIV";
        }
        else if (litera == 'O')
        {
            str = "XV";
        }
        else if (litera == 'P')
        {
            str = "XVI";
        }
        else if (litera == 'Q')
        {
            str = "XVII";
        }
        else if (litera == 'R')
        {
            str = "XVIII";
        }
        else if (litera == 'S' || litera == 'T')
        {
            str = "XIX";
        }
        else if (litera == 'U')
        {
            str = "XXII";
        }
        else if (litera == 'V' || litera == 'W' || litera == 'X' || litera == 'Y')
        {
            str = "XX";
        }
        else if (litera == 'Z')
        {
            str = "XXI";
        }
        return str;
    }
    
    public static Mkb10 SearchMkb10(ApiDbContext _context, Mkb10 searchMkb10)
    {
        IQueryable<Mkb10> resultData;
        if (searchMkb10.Subnumber == null)
        {
            resultData = from mkb10 in _context.Mkb10s
                where mkb10.Litera == searchMkb10.Litera && mkb10.Number == searchMkb10.Number &&
                      mkb10.Subnumber == null
                select mkb10;
        }
        else
        {
            resultData = from mkb10 in _context.Mkb10s
                where mkb10.Litera == searchMkb10.Litera && mkb10.Number == searchMkb10.Number &&
                      mkb10.Subnumber == searchMkb10.Subnumber
                select mkb10;
        }
        
        return resultData.FirstOrDefault();
    }

    public static RusEsili SearchRusEsili(ApiDbContext _context, RusEsili searchService)
    {
        IQueryable<RusEsili> resultData;
        if (searchService.Subsubnumber == null)
        {
            resultData = from service in _context.RusEsilis
                where service.Section == searchService.Section && service.Block == searchService.Block &&
                      service.Number == searchService.Number && service.Subnumber == searchService.Subnumber &&
                      service.Subsubnumber == null
                select service;
        }
        else
        {
            resultData = from service in _context.RusEsilis
                where service.Section == searchService.Section && service.Block == searchService.Block &&
                      service.Number == searchService.Number && service.Subnumber == searchService.Subnumber &&
                      service.Subsubnumber == searchService.Subsubnumber
                select service;
        }
        
        return resultData.FirstOrDefault();

    }
    
    public static string GetMkb10FullCode(Mkb10 mkb10)
    {
        string code = String.Format("{0}{1:D2}{2}", mkb10.Litera.ToString(), mkb10.Number.ToString(),  mkb10.Subnumber != null 
            ? "."+mkb10.Subnumber.ToString() : "");
        return code;
    }
}