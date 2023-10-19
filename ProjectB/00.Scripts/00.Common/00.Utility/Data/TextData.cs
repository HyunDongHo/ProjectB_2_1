﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
public class TextData
{
	[XmlAttribute]
	public string ID;
	[XmlAttribute]
	public string kor;
	[XmlAttribute]
	public string eng;
	// ...
}

[Serializable, XmlRoot("ArrayOfTextData")]
public class TextDataLoader : ILoaderXml<string, TextData>
{
	[XmlElement("TextData")]
	public List<TextData> _textData = new List<TextData>();

	public Dictionary<string, TextData> MakeDic()
	{
		Dictionary<string, TextData> dic = new Dictionary<string, TextData>();

		foreach (TextData data in _textData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}