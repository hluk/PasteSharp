/*
    Copyright (c) 2016, Lukas Holecek <hluk@email.cz>

    This file is part of PasteSharp.

    PasteSharp is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PasteSharp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PasteSharp.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

using Mono.Data.Sqlite;
using System.Configuration;

public class SqliteItemStore
{
	private SqliteConnection dbConnection;

	public SqliteItemStore()
	{
		var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		var dbFilePath = config.FilePath + ".sqlite";

		var createDb = !System.IO.File.Exists(dbFilePath);
		if (createDb)
		{
			SqliteConnection.CreateFile(dbFilePath);
		}

		var uri = new Uri(dbFilePath);
		dbConnection = new SqliteConnection(String.Format("Data Source={0};Version=3;", uri.AbsolutePath));
		dbConnection.Open();

		if (createDb)
		{
			Clear();
		}
	}

	public void Clear()
	{
		Command(@"
			drop table if exists items;
			create table items (text varchar(20))"
		       ).ExecuteNonQuery();
	}

	public void AddText(string text)
	{
		var command = Command("insert into items (text) values (@text)");
		command.Parameters.Add(new SqliteParameter("@text", text));
		command.ExecuteNonQuery();
	}

	public string GetText(int index)
	{
		var command = Command("select text from items where rowid = @row");
		command.Parameters.Add(new SqliteParameter("@row", index + 1));
		var reader = command.ExecuteReader();
		try
		{
			if (!reader.Read())
				throw new Exception("No text available");
			return reader.GetString(reader.GetOrdinal("text"));
		}
		finally
		{
			reader.Close();
		}
	}

	public int RowCount()
	{
		var reader = Command("select count(*) from items").ExecuteReader();
		try
		{
			reader.Read();
			return reader.GetInt32(0);
		}
		finally
		{
			reader.Close();
		}
	}

	private SqliteCommand Command(string query)
	{
		return new SqliteCommand(query, dbConnection);
	}
}
