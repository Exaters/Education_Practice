using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

public static class DatabaseHelper
{
    private static readonly string DbFile = "results.db";
    private static readonly string ConnectionString = $"Data Source={DbFile};Version=3;";

    public static void InitializeDatabase()
    {
        if (!File.Exists(DbFile))
        {
            SQLiteConnection.CreateFile(DbFile);
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string createTable = @"
                    CREATE TABLE Results (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        X0 REAL,
                        Y0 REAL,
                        R REAL,
                        C REAL,
                        Direction TEXT,
                        N INTEGER,
                        FormulaResult REAL,
                        MonteCarloResult REAL,
                        Date TEXT
                    );";
                using (var cmd = new SQLiteCommand(createTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public static void AddResult(double x0, double y0, double r, double c, string direction, int n, double formula, double monteCarlo)
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SQLiteCommand(@"
                INSERT INTO Results (X0, Y0, R, C, Direction, N, FormulaResult, MonteCarloResult, Date)
                VALUES (@x0, @y0, @r, @c, @direction, @n, @formula, @monte, @date);", conn);

            cmd.Parameters.AddWithValue("@x0", x0);
            cmd.Parameters.AddWithValue("@y0", y0);
            cmd.Parameters.AddWithValue("@r", r);
            cmd.Parameters.AddWithValue("@c", c);
            cmd.Parameters.AddWithValue("@direction", direction);
            cmd.Parameters.AddWithValue("@n", n);
            cmd.Parameters.AddWithValue("@formula", formula);
            cmd.Parameters.AddWithValue("@monte", monteCarlo);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd.ExecuteNonQuery();
        }
    }

    public static DataTable GetAllResults()
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var dt = new DataTable();
            var cmd = new SQLiteCommand("SELECT * FROM Results ORDER BY Date DESC;", conn);
            using (var reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
            }
            return dt;
        }
    }

    public static void UpdateResult(int id, double formula, double monte)
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SQLiteCommand(@"
                UPDATE Results 
                SET FormulaResult = @formula, MonteCarloResult = @monte 
                WHERE Id = @id;", conn);

            cmd.Parameters.AddWithValue("@formula", formula);
            cmd.Parameters.AddWithValue("@monte", monte);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteResult(int id)
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SQLiteCommand("DELETE FROM Results WHERE Id = @id;", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteAllResult()
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new SQLiteCommand("DELETE FROM Results;", conn);
            cmd.ExecuteNonQuery();
        }
    }

    public static DataRow GetResultById(int id)
    {
        using (var conn = new SQLiteConnection(ConnectionString))
        {
            conn.Open();
            var dt = new DataTable();
            var cmd = new SQLiteCommand("SELECT * FROM Results WHERE Id = @id;", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                dt.Load(reader);
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}