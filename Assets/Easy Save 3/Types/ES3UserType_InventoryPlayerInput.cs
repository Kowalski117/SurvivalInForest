using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_InventoryPlayerInput : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_InventoryPlayerInput() : base(typeof(InventoryPlayerInput)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (InventoryPlayerInput)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (InventoryPlayerInput)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_InventoryPlayerInputArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_InventoryPlayerInputArray() : base(typeof(InventoryPlayerInput[]), ES3UserType_InventoryPlayerInput.Instance)
		{
			Instance = this;
		}
	}
}