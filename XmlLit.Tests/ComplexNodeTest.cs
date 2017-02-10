using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace XmlLit.Tests {
  [TestClass]
  public class ComplexNodeTest {
    public class MapRecord {
      public string renderorder;

      public class Tileset {
        public int firstgid;

        public class Image {
          public string source;
        }
        public Image image = new Image();
      }
      public class Layer {
        public string name;
        public int width;
        public int height;

        public class Tile {
          private int gid;
        }

        public class Data {
          private List<Tile> tiles = new List<Tile>();
        }

        Data data = new Data();
      }

      public List<Tileset> tilesets = new List<Tileset>();
      public List<Layer> layers = new List<Layer>();
    }

    [TestMethod]
    public void ParseComplexDocument () {
      string data = @"<?xml version='1.0' encoding='utf-8'?>
<map renderorder='right-down'>
 <tileset firstgid='1'>
  <image source='hexagonTerrain_sheet_re.png'/>
  <tile id='13'/>
 </tileset>
 <layer name='Tile Layer 1' width='22' height='29'>
  <data>
   <tile gid='53'/>
   <tile gid='69'/>
  </data>
 </layer>
</map>";
      MapRecord record = new MapRecord();
      using (TextReader text_reader = new StringReader(data)) {
        XmlReader reader = XmlReader.Create(text_reader);
        Node.Load(reader, delegate (Node element) {
          record.renderorder = element["@renderorder"].Value;
          foreach (var tileset_element in element.GetElements("tileset")) {
            var tileset = new MapRecord.Tileset();
            tileset.image.source = tileset_element["image"]["@source"].Value;
            record.tilesets.Add(tileset);
          }
        }, "map");
      }
      Assert.AreEqual("right-down", record.renderorder);
      Assert.AreEqual("hexagonTerrain_sheet_re.png", record.tilesets[0].image.source);
    }
  }
}
