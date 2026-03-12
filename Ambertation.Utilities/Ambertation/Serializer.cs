using System;
using System.IO;
using System.Xml.Serialization;

namespace Ambertation;

internal class Serializer
{
	public static void Serialize(object o, string flname)
	{
		Stream stream = File.Create(flname);
		try
		{
			Serialize(o, stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public static object DeSerialize(Type t, string flname)
	{
		Stream stream = File.OpenRead(flname);
		try
		{
			return DeSerialize(t, stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public static void Serialize(object o, Stream s)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
		xmlSerializer.Serialize(s, o);
	}

	public static object DeSerialize(Type t, Stream s)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(t);
		return xmlSerializer.Deserialize(s);
	}
}
