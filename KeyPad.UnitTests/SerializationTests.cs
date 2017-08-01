using KeyPad.Serializer;
using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyPad.UnitTests {

	[TestClass]
	public class SerializationTests {

		[TestMethod]
		public void JsonSerialization_Success() {
			TestClass testClass = new TestClass() {
				Id = 1,
				Message = "Hello"
			};
			SettingsJsonSerializer serializer = new SettingsJsonSerializer();
			string serializedData = serializer.Serialize(testClass);

			Assert.IsTrue(serializedData != null && !String.IsNullOrEmpty(serializedData));
			Assert.AreEqual(
				"{\"Id\":1,\"Message\":\"Hello\"}",
				serializedData
			);
		}

		[TestMethod]
		public void JsonDeserialization_Success() {
			try {
				var serializer = new SettingsJsonSerializer();
				string jsonString = "{\"Id\":1,\"Message\":\"Hello\"}";
				TestClass testClass = serializer.Deserialize<TestClass>(jsonString);
				Assert.IsTrue(testClass != null);
				Assert.IsTrue(testClass.Id == 1 && testClass.Message.Equals("Hello"));
			}
			catch {
				Assert.IsTrue(false, "Deserializing threw an exception");
			}
		}

	}

	[DataContract]
	internal class TestClass {

		[DataMember] public int Id { get; set; }
		[DataMember] public string Message { get; set; }
		
	}

}