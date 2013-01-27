using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace WF_Xtension
{
	/// <summary>
	/// Reads data from an XML file to customize Forms
	/// </summary>
	public sealed class FormDataReader
	{
		#region StaticMembers
		/// <summary>
		/// Primary instance of the FormDataReader.
		/// It must be initialized via the static method Init
		/// before it can be used.
		/// </summary>
		public static FormDataReader Instance = null;
		/// <summary>
		/// Initializes the Instance of the FormDataReader
		/// </summary>
		/// <param name="dataPath">Path of the XML file containing data</param>
		public static void Init(string dataPath)
		{
			if (Instance != null)
				throw new Exception("FormDataReader can only be initialized once.");
			if (!File.Exists(dataPath))
				throw new FileNotFoundException(dataPath);
			Instance = new FormDataReader(dataPath);
		}
		#endregion

		#region Variables
		XmlReader m_reader;
		private string m_path, m_form = null;
		#endregion

		/// <summary>
		/// Reads data from an XML file to customize forms
		/// </summary>
		/// <param name="path">Path of the XML file containing data</param>
		public FormDataReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException();
			if (!File.Exists(path))
				throw new FileNotFoundException(path);

			m_path = path;
			m_reader = XmlReader.Create(path);
			Closed = false;
		}

		#region Methods
		/// <summary>
		/// Finds the section of the file addressing a particular form
		/// </summary>
		/// <param name="name">Name of the form</param>
		public void GoToForm(string name)
		{
			if (Closed)
				throw new Exception("This DataReader has already been closed");
			name = name.ToLower();
			m_reader = XmlReader.Create(m_path);
			while (m_reader.Read())
			{
				if (m_reader.MoveToContent() == XmlNodeType.Element && m_reader.Name == "form")
				{
					if (m_reader.HasAttributes && m_reader.GetAttribute("name").ToLower() == name)
					{
						m_form = name;
						return;
					}
				}
			}
			throw new Exception("Form '" + name + "' does not exist in this file");
		}

		/// <summary>
		/// Reads data for a particular control
		/// </summary>
		/// <param name="name">Name of the control</param>
		/// <param name="type">Type of the data</param>
		/// <returns>Data in string format</returns>
		public string ReadInfo(string name, out string type)
		{
			if (Closed)
				throw new Exception("This DataReader has already been closed");
			if (m_form == null)
				throw new Exception("Must be reading info for a specified form");
			GoToForm(m_form);

			name = name.ToLower();
			while (m_reader.Read())
			{
				if (m_reader.MoveToContent() == XmlNodeType.Element && m_reader.Name == "info")
				{
					if (m_reader.HasAttributes && m_reader.GetAttribute("name").ToLower() == name)
					{
						type = m_reader.GetAttribute("type").ToLower();
						return m_reader.ReadString();
					}
				}
			}
			throw new Exception("Info '" + name + "' does not exist in this form");
		}

		/// <summary>
		/// Closes the FormDataReader and releases memory
		/// </summary>
		public void Close()
		{
			if (Closed)
				throw new Exception("This DataReader has already been closed");
			m_reader.Close();
			Closed = true;
		}
		#endregion

		#region Accessors
		/// <summary>
		/// Whether or not the FormDataReader has been closed
		/// </summary>
		public bool Closed { get; private set; }
		#endregion
	}
}