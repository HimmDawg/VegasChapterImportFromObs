using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using ScriptPortal.Vegas;

//Custom Vegas Pro Script
//Based upon https://github.com/HimmDawg/VegasChapterImportFromObs
//Which only works when time_base = 1/1000.
//Created 2025\06\10 by Randy Turner, Version: 1.0
//Imports mp4/m4v Chapters as Titled Markers using FFProbe.
//This version extended to include time_base in position calculation
//allowing general use on mp4\m4v files.

public class EntryPoint
{
	Vegas _myVegas;

	public void FromVegas(Vegas vegas)
	{
		_myVegas = vegas;
		Media[] media = _myVegas.Project.MediaPool.GetSelectedMedia();
		string msg = "Please select one mp4/m4v file in project media.";

		if (media.Length != 1)
		{
			MessageBox.Show(msg);
			return;
		}

		string mediaFile = media[0].FilePath;

		if (!IsMp4File(mediaFile))
		{
			MessageBox.Show(msg);
			return;
		}

		ChapterList chapters = GetChaptersFromMetadata(mediaFile);

		if (chapters.Count == 0)
		{
			MessageBox.Show("No chapters found.");
			return;
		}

		Marker[] markers = ChaptersToMarkers(chapters);
		foreach (Marker marker in markers)
		{
			_myVegas.Project.Markers.Add(marker);
		}

		MessageBox.Show(markers.Length + " markers added.");
	}

	private bool IsMp4File(string mediaFile)
	{
		bool rv = Path.GetExtension(mediaFile).Equals(".mp4", StringComparison.OrdinalIgnoreCase);
		if (rv == true){ return rv; }
		else
		{
			rv = Path.GetExtension(mediaFile).Equals(".m4v", StringComparison.OrdinalIgnoreCase);
			return rv;
		}
	}

	private ChapterList GetChaptersFromMetadata(string filePath)
	{
		string metadata = RunFFProbe(filePath);
		if (!string.IsNullOrEmpty(metadata))
		{
			return JsonSerializer.Deserialize<ChapterList>(metadata);
		}
		else
		{
			return new ChapterList();
		}
	}

	private string RunFFProbe(string videoFilePath)
	{
		Process process = new Process();
		process.StartInfo.FileName = "ffprobe";
		process.StartInfo.Arguments = "-i \"" + videoFilePath + "\" -print_format json -show_chapters -loglevel error";
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
        process.Start();
		string output = process.StandardOutput.ReadToEnd();
		process.WaitForExit();
		return output;
	}

	private Marker[] ChaptersToMarkers(ChapterList chapters)
	{
		Marker[] result = new Marker[chapters.Count];
		for (int i = 0; i < chapters.Count; i++)
		{
			Chapter chapter = chapters[i];
            long d = Convert.ToInt64(chapter.time_base.Substring(2));
			long startMs = ((chapter.start/d)*1000);
            Timecode position = Timecode.FromMilliseconds(startMs);
			Marker marker = new Marker(position, chapter.tags["title"]);
			result[i] = marker;
		}
		return result;
	}
}

public class ChapterList
{
	private Chapter[] _chapters = Array.Empty<Chapter>();
	public Chapter[] chapters
	{
		get {return _chapters;}
		set {_chapters = value;}
	}
	public int Count
	{
		get {return chapters.Length;}
	}
	public Chapter this[int index]
	{
		get {return chapters[index];}
	}
}

public class Chapter
{
	private int _id;
	private long _start;
	private string _time_base;
	private Dictionary<string, string> _tags = new Dictionary<string, string>();
	
	public int id
	{
		get {return _id;}
		set {_id = value;}
	}
	public long start
	{
		get {return _start;}
		set {_start = value;}
	}
	public string time_base
	{
		get { return _time_base; }
		set { _time_base = value; }
	}
	public Dictionary<string, string> tags
	{
		get {return _tags;}
		set {_tags = value;}
	}
}