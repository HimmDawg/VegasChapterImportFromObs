using System.Collections.Generic;
using System.Windows.Forms;
using ScriptPortal.Vegas;
using System.Diagnostics;
using System.Text.Json;

public class EntryPoint
{
	Vegas _myVegas;

	public void FromVegas(Vegas vegas)
	{
		_myVegas = vegas;
		Media[] media = _myVegas.Project.MediaPool.GetSelectedMedia();

		if (media == null)
		{
			MessageBox.Show("Please select one video media.");
			return;
		}

		if (media.Length != 1)
		{
			MessageBox.Show("Please select ONLY one video media.");
			return;
		}

		string mediaFile = media[0].FilePath;
		ChapterList chapters = GetChaptersFromMetadata(mediaFile);

		if (chapters.Count == 0)
		{
			MessageBox.Show("No chapters found.");
		}

		List<Marker> markers = ChaptersToMarkers(chapters);
		foreach (Marker marker in markers)
		{
			_myVegas.Project.Markers.Add(marker);
		}

		MessageBox.Show(markers.Count + " markers added.");
	}

	private ChapterList GetChaptersFromMetadata(string filePath)
	{
		string metadata = RunFFProbe(filePath);
		if (metadata != null)
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

	private List<Marker> ChaptersToMarkers(ChapterList chapters)
	{
		List<Marker> result = new List<Marker>();
		for (int i = 0; i < chapters.Count; i++)
		{
			Chapter chapter = chapters[i];
			Timecode position = Timecode.FromMilliseconds(chapter.start);
			Marker marker = new Marker(position);
			result.Add(marker);
		}
		return result;
	}
}

public class ChapterList
{
	private Chapter[] _chapters = new Chapter[0];
	public Chapter[] chapters
	{
		get
		{
			return _chapters;
		}
		set
		{
			_chapters = value;
		}
	}
	public int Count
	{
		get
		{
			return chapters.Length;
		}
	}
	public Chapter this[int index]
	{
		get
		{
			return chapters[index];
		}
	}
}

public class Chapter
{
	private int _id;
	private long _start;
	private Dictionary<string, string> _tags = new Dictionary<string, string>();
	
	public int id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}
	public long start
	{
		get
		{
			return _start;
		}
		set
		{
			_start = value;
		}
	}
	public Dictionary<string, string> tags
	{
		get
		{
			return _tags;
		}
		set
		{
			_tags = value;
		}
	}
}

