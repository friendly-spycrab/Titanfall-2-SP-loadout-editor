using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Titanfall2ModdingLibrary
{
    public static class SQLiteLoader
    {
        public static List<Pointer> GetPointerListFromSQLiteFile(string File)
        {
            Dictionary<int, string> ModuleIndexes = new Dictionary<int, string>();

            SQLiteConnection Data = new SQLiteConnection("Data Source=" + File + "");
            Data.Open();
            SQLiteCommand Command = new SQLiteCommand("SELECT * FROM 'modules'",Data);
            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                var Name = Reader["name"];
                var id = Reader["moduleid"];
                ModuleIndexes.Add(Convert.ToInt32(id),Convert.ToString(Name));

            }

            Command = new SQLiteCommand("SELECT * FROM 'results'",Data);
            Reader = Command.ExecuteReader();


            List<Pointer> Pointers = new List<Pointer>();

            while (Reader.Read())
            {
                Pointer P = new Pointer();
                P.BaseAddress = (long)Reader["moduleoffset"];
                P.ModuleName = ModuleIndexes[Convert.ToInt32(Reader["moduleid"])];
                int OffsetCount = Convert.ToInt32(Reader["offsetcount"]);
                List<long> Offsets = new List<long>();
                for (int i = 1; i <= OffsetCount; i++)
                    Offsets.Add(Convert.ToInt32(Reader["offset" + i.ToString()]));

                P.offsets = Offsets.ToArray().Reverse().ToArray();
                Pointers.Add(P);

            }
            Data.Close();
            return Pointers;
        }

    }
}
