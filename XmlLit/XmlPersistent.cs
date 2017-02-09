using System.Collections.Generic;

namespace XmlLit {
  public class XmlPersistent<T> where T : class {
    static public List<T> Records { get { return records; } }
    protected static List<T> records = new List<T>();
  }
}