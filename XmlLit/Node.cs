using System.Collections.Generic;
using System.Xml;

namespace XmlLit {
  public class Node {
    private string value;
    private string name;
    private List<Node> children = new List<Node>();
    private int line;

    private static void OnReadEntityReference (XmlReader reader) {
    }

    private static void OnReadDocType (XmlReader reader) {
    }

    private static void OnReadXmlDeclaration (XmlReader reader) {
    }

    private static void OnReadComment (XmlReader reader) {
    }

    private static void OnReadProcessingInstruction (XmlReader reader) {
    }

    private static void OnReadCData (XmlReader reader) {
    }

    private void OnReadText (XmlReader reader) {
      string text = reader.Value.Trim();
      value = text;
    }

    private void OnReadDocument (XmlReader reader) { }

    private void ReadAttributes (XmlReader reader) {
      while (reader.MoveToNextAttribute()) {
        Node element = new Node();
        element.name = string.Format("@{0}", reader.Name);
        element.value = reader.Value;
        element.line = ((IXmlLineInfo)reader).LineNumber;
        Children.Add(element);
      }
    }

    private void OnReadStartElement (XmlReader reader) {
      Node element = new Node();
      element.name = reader.Name;
      element.line = ((IXmlLineInfo)reader).LineNumber;
      if (reader.IsEmptyElement == false) {
        element.LoadNode(reader);
      }
      Children.Add(element);
    }

    private void OnReadEndElement (XmlReader reader) { }

    private void LoadNode (XmlReader reader) {
      ReadAttributes(reader);
      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.Element:
            OnReadStartElement(reader);
            break;
          case XmlNodeType.Text:
            OnReadText(reader);
            break;
          case XmlNodeType.CDATA:
            OnReadCData(reader);
            break;
          case XmlNodeType.ProcessingInstruction:
            OnReadProcessingInstruction(reader);
            break;
          case XmlNodeType.Comment:
            OnReadComment(reader);
            break;
          case XmlNodeType.XmlDeclaration:
            OnReadXmlDeclaration(reader);
            break;
          case XmlNodeType.Document:
            OnReadDocument(reader);
            break;
          case XmlNodeType.DocumentType:
            OnReadDocType(reader);
            break;
          case XmlNodeType.EntityReference:
            OnReadEntityReference(reader);
            break;
          case XmlNodeType.EndElement:
            OnReadEndElement(reader);
            return;
        }
      }
    }

    public static void Load (XmlReader reader, ReadElementCallback callback, string node_path) {
      if (callback == null) {
        return;
      }
      List<string> pathlist = new List<string>(node_path.Split('/'));
      Stack<string> cur_path = new Stack<string>();
      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.Element:
            cur_path.Push(reader.Name);
            if (string.IsNullOrEmpty(node_path)) {
              node_path = reader.Name;
            }
            if (pathlist[cur_path.Count - 1] != reader.Name) {
              break;
            }
            if (cur_path.Count != pathlist.Count) {
              break;
            }
            Node element = new Node();
            element.name = reader.Name;
            element.LoadNode(reader);
            cur_path.Pop();
            callback(element);
            break;
          case XmlNodeType.EndElement:
            cur_path.Pop();
            break;
        }
      }
    }

    public string Value { get { return value; } }
    public string Name { get { return name; } }
    public List<Node> Children { get { return children; } }
    public int Line { get { return line; } }
    public Node this[string element_name] {
      get {
        return GetElement(element_name);
      }
    }

    public Node GetElement (string element_name) {
      return children.Find(e => e.name == element_name);
    }

    public List<Node> GetElements (string element_name) {
      return children.FindAll(e => e.name == element_name);
    }


    public delegate void ReadElementCallback (Node element);
  }
}