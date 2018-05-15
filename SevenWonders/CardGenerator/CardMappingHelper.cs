using SevenWonder.Helper;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace SevenWonder.CardGenerator
{
    public static class CardMappingHelper
    {
        public static CardCollection ReadMainXmlFile()
        {
            var assembly = typeof(CardCollection).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("SevenWonder.Resources.seven_wonders_deck.xml"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var streamResult = reader.ReadToEnd();
                    var xmlDoc = XDocument.Parse(streamResult);
                    return Serializer.Deserialize<CardCollection>(xmlDoc.ToString());
                }
            }
        }
    }
}
