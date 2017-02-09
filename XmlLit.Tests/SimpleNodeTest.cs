using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace XmlLit.Tests {
  [TestClass]
  public class SimpleNodeTest {
    class Row {
      public int uid;
      public string id;
      public bool enable;
    }

    [TestMethod]
    public void ParseElements_InvalidNodePath () {
      string data = @"<?xml version='1.0' encoding='utf-8'?>
<data>
  <Row>
    <uid>1</uid>
    <id>A001</id>
    <enable>True</enable>
  </Row>
  <Row>
    <uid>2</uid>
    <id>A002</id>
    <enable>True</enable>
  </Row>";
      List<Row> rows = new List<Row>();
      using (TextReader text_reader = new StringReader(data)) {
        XmlReader reader = XmlReader.Create(text_reader);
        Node.Load(reader, delegate (Node element) {
          Row row = new Row();
          row.uid = int.Parse(element["uid"].Value);
          row.id = element["id"].Value;
          row.enable = bool.Parse(element["enable"].Value);
          rows.Add(row);
        }, "data/row");
      }
      Assert.AreEqual(2, rows.Count);
    }

    [TestMethod]
    public void ParseElements_UnclosedNode () {
      string data = @"<?xml version='1.0' encoding='utf-8'?>
<data>
  <Row>
    <uid>1</uid>
    <id>A001</id>
    <enable>True</enable>
  </Row>
  <Row>
    <uid>2</uid>
    <id>A002</id>
    <enable>True</enable>
  </Row>";
      List<Row> rows = new List<Row>();
      using (TextReader text_reader = new StringReader(data)) {
        XmlReader reader = XmlReader.Create(text_reader);
        Node.Load(reader, delegate (Node element) {
          Row row = new Row();
          row.uid = int.Parse(element["uid"].Value);
          row.id = element["id"].Value;
          row.enable = bool.Parse(element["enable"].Value);
          rows.Add(row);
        }, "data/Row");
      }
      Assert.AreEqual(2, rows.Count);
    }

    [TestMethod]
    public void ParseElements () {
      string data = @"<?xml version='1.0' encoding='utf-8'?>
<data>
  <Row>
    <uid>1</uid>
    <id>A001</id>
    <enable>True</enable>
  </Row>
  <Row>
    <uid>2</uid>
    <id>A002</id>
    <enable>True</enable>
  </Row>
</data>";
      List<Row> rows = new List<Row>();
      using (TextReader text_reader = new StringReader(data)) {
        XmlReader reader = XmlReader.Create(text_reader);
        Node.Load(reader, delegate (Node element) {
          Row row = new Row();
          row.uid = int.Parse(element["uid"].Value);
          row.id = element["id"].Value;
          row.enable = bool.Parse(element["enable"].Value);
          rows.Add(row);
        }, "data/Row");
      }
      Assert.AreEqual(2, rows.Count);
    }

    public class MapRecord {
      public string renderorder;
      public int height;
      public int width;

      public class Tileset {
        public int firstgid;
        public string name;

        public class Image {
          public string source;
          public int width;
          public int height;
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

      private List<Tileset> tilesets = new List<Tileset>();
      private List<Layer> layers = new List<Layer>();
    }
  }
}
